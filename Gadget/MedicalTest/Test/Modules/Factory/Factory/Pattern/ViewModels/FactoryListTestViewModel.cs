/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryListTestViewModel", typeof (IFactoryListTestViewModel))]
  public class TFactoryListTestViewModel : TViewModelAware<TFactoryListTestModel>, IHandleMessageInternal, IInternalHandle<TFactoryMessageInternal>, IFactoryListTestViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListTestViewModel (IFactoryPresentation presentation)
      : base (new TFactoryListTestModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);

      UseChildViewModel = true;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from parent
        if (message.Node.IsParentToMe (TChild.List)) {
          // DatabaseValidated
          if (message.IsAction (TInternalMessageAction.DatabaseValidated)) {
            TDispatcher.Invoke (RequestDataDispatcher);
          }

          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Collection - Full
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full)) {
              if (message.Result.IsValid) {
                // Gadget Test
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Test)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }

                // Gadget Material
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Material)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (RefreshModelDispatcher, action);
                }
              }
            }

            // Select - ById
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
              if (message.Result.IsValid) {
                // Gadget Test
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Test)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, action);
                }
              }
            }
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.List)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            if (message.Support.Argument.Args.PropertyName.Equals ("Edit")) {
              TDispatcher.BeginInvoke (EditDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
            }
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestDesignDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            Model.Cleanup ();

            TDispatcher.Invoke (RefreshAllDispatcher);
            TDispatcher.Invoke (RequestDataDispatcher);
          }
        }
      }
    }

    public void InternalHandle (TFactoryMessageInternal message)
    {
      // used to parentView comunicate with childView (UseChildViewModel = true)
      if (message.NotNull ()) {
        if (message is TFactoryMessageInternal msg) {
          // Select
          if (msg.IsAction (TInternalMessageAction.Select)) {
            // material
            if (msg.Support.Argument.Types.Item.Category.Equals (Server.Models.Infrastructure.TCategory.Material)) {
              Model.MaterialItemChanged (msg.Support.Argument.Types.Item.GadgetMaterialModel.Material);

              TDispatcher.Invoke (RefreshAllDispatcher);
            }
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnGadgetItemChecked (TFactoryListItemInfo itemInfo)
    {
      TDispatcher.BeginInvoke (ItemCheckedChangedDispatcher, itemInfo);
    }

    public void OnGadgetItemUnchecked (TFactoryListItemInfo itemInfo)
    {
      TDispatcher.BeginInvoke (ItemCheckedChangedDispatcher, itemInfo);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      RefreshCollection ("TestModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Collection - Full 
      var action = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Test,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void RequestDesignDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.RequestModel (action);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseSelectByIdDispatcher (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      if (action.Param2 is TFactoryListItemInfo itemInfo) {
        itemInfo.ModelItem.GadgetTestModel.UpdateContents (action);

        Model.GadgetItemChecked (itemInfo, isChecked: itemInfo.IsChecked);

        TDispatcher.Invoke (RefreshAllDispatcher);

        // to Sibling
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.List, TypeInfo);
        message.Support.Argument.Types.Item.CopyFrom (itemInfo.ModelItem);
        message.Support.Argument.Args.Select (itemInfo.IsChecked ? "GadgetAdd" : "GadgetRemove");

        if (Model.HasGadgetChecked) {
          message.Support.Argument.Types.ReportData.SelectLock ();
        }

        DelegateCommand.PublishInternalMessage.Execute (message);

        // to parent view
        NotifyParentViewModel (message);
      }
    }

    void RefreshModelDispatcher (Server.Models.Component.TEntityAction action)
    {
      // refresh model
      //Model.RefreshModel (action);
      TDispatcher.Invoke (RefreshAllDispatcher);

      // to parent (RefreshModel)
      var message = new TFactoryMessageInternal (TInternalMessageAction.RefreshModel, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ItemCheckedChangedDispatcher (TFactoryListItemInfo itemInfo)
    {
      // to parent
      // Collection - Full 
      var action = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Test,
        Server.Models.Infrastructure.TOperation.Select,
        Server.Models.Infrastructure.TExtension.ById
      );

      action.Id = itemInfo.Id;
      action.Param2 = itemInfo; // preserve

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Edit (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }
    #endregion

    #region Property
    IDelegateCommand DelegateCommand
    {
      get
      {
        return (PresentationCommand as IDelegateCommand);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace