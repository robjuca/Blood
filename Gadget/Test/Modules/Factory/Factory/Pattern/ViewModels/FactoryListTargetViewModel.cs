﻿/*----------------------------------------------------------------
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
  [Export ("ModuleFactoryListTargetViewModel", typeof (IFactoryListTargetViewModel))]
  public class TFactoryListTargetViewModel : TViewModelAware<TFactoryListTargetModel>, IHandleMessageInternal, IFactoryListTargetViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListTargetViewModel (IFactoryPresentation presentation)
      : base (new TFactoryListTargetModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
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
            // Collection-Full
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full)) {
              if (message.Result.IsValid) {
                // Gadget Target
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Target)) {
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

            // Select-ById
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
              if (message.Result.IsValid) {
                var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                TDispatcher.BeginInvoke (ResponseModelDispatcher, action);
              }
            }
          }

          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            TDispatcher.Invoke (RefreshAllDispatcher);
            TDispatcher.Invoke (RequestDataDispatcher);
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.List)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            if (message.Support.Argument.Args.PropertyName.Equals ("all")) {
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
    #endregion

    #region View Event
    public void OnGadgetSelectionChanged (int selectedIndex)
    {
      TDispatcher.BeginInvoke (GadgedSelectionChangedDispatcher, selectedIndex);
    }

    public void OnGadgetChanged (TComponentModelItem item)
    {
      TDispatcher.BeginInvoke (GadgetChangedDispatcher, item);
    }

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

      RefreshCollection ("MaterialModelItemsViewSource");
      RefreshCollection ("TargetModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Collection - Full 
      var action = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Target,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      // Collection - Full (Target list)
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);

      // to parent
      // Collection - Full (Material list - used to send RefreshModel)
      var entityAction = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Material,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (entityAction);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseModelDispatcher (Server.Models.Component.TEntityAction action)
    {
      // to Sibling (Select)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (TComponentModelItem.Create (action));

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RefreshModelDispatcher (Server.Models.Component.TEntityAction action)
    {
      // refresh model
      Model.RefreshModel (action);
      TDispatcher.Invoke (RefreshAllDispatcher);

      // to parent (RefreshModel)
      var message = new TFactoryMessageInternal (TInternalMessageAction.RefreshModel, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void GadgedSelectionChangedDispatcher (int selectedIndex)
    {
      Model.GadgetSelectionItemChanged (selectedIndex);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void GadgetChangedDispatcher (TComponentModelItem item)
    {
      if (item.IsNull ()) {
        // to Sibling (Cleanup)
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      else {
        // to Sibling (Select)
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
        message.Support.Argument.Types.Item.CopyFrom (item);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void RequestDesignDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.RequestModel (action);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ItemCheckedChangedDispatcher (TFactoryListItemInfo itemInfo)
    {
      Model.GadgetItemChecked (itemInfo, isChecked: itemInfo.IsChecked);

      TDispatcher.Invoke (RefreshAllDispatcher);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (itemInfo.ModelItem);
      message.Support.Argument.Args.Select (itemInfo.IsChecked ? "GadgetAdd" : "GadgetRemove");

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