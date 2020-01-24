/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Action;
using Server.Models.Infrastructure;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;
using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryListTestViewModel", typeof (IFactoryListTestViewModel))]
  public class TFactoryListTestViewModel : TViewModelAware<TFactoryListTestModel>, IHandleMessageInternal, IFactoryListTestViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListTestViewModel (IFactoryPresentation presentation)
      : base (new TFactoryListTestModel ())
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
            // Collection - Full
            if (message.Support.Argument.Types.IsOperation (TOperation.Collection, TExtension.Full)) {
              if (message.Result.IsValid) {
                // Gadget Test
                if (message.Support.Argument.Types.IsOperationCategory (TCategory.Test)) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }
              }
            }

            // Select - ById
            if (message.Support.Argument.Types.IsOperation (TOperation.Select, TExtension.ById)) {
              if (message.Result.IsValid) {
                // Gadget Test
                if (message.Support.Argument.Types.IsOperationCategory (TCategory.Test)) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, action);
                }
              }
            }
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.List, TypeInfo)) {
          // Select
          if (message.IsAction (TInternalMessageAction.Select)) {
            // material
            if (message.Support.Argument.Args.Param1 is TActionComponent component) {
              Model.MaterialItemChanged (component);
            }

            TDispatcher.Invoke (RefreshAllDispatcher);
          }

          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            if (message.Support.Argument.Args.PropertyName.Equals ("edit")) {
              if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                TDispatcher.BeginInvoke (EditDispatcher, component);
              }
            }
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestDesignDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
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
    public void OnGadgetItemChecked (GadgetTest gadget)
    {
      TDispatcher.BeginInvoke (ItemCheckedChangedDispatcher, gadget);
    }

    public void OnGadgetItemUnchecked (GadgetTest gadget)
    {
      TDispatcher.BeginInvoke (ItemCheckedChangedDispatcher, gadget);
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
      var action = TEntityAction.Create (
        TCategory.Test,
        TOperation.Collection,
        TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      Model.Select (action);  // Test collection

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void RequestDesignDispatcher (TEntityAction action)
    {
      Model.RequestModel (action);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseSelectByIdDispatcher (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      if (entityAction.Param2 is GadgetTest gadget) {
        if (gadget.IsContentTarget) {
          var component = TActionComponent.Create (TCategory.Test);
          component.Models.GadgetTestModel.CopyFrom (gadget);

          TActionConverter.Select (TCategory.Test, component, entityAction);
          gadget.CopyFrom (component.Models.GadgetTestModel);

          Model.GadgetItemChecked (gadget, isChecked: gadget.IsChecked);

          // to Sibling
          var message = new TFactorySiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.List, TypeInfo);
          message.Support.Argument.Args.Select (component);
          message.Support.Argument.Args.Select (gadget.IsChecked ? "GadgetAdd" : "GadgetRemove");

          if (Model.HasGadgetChecked) {
            message.Support.Argument.Types.ReportData.SelectLock ();
          }

          DelegateCommand.PublishInternalMessage.Execute (message);
        }

        TDispatcher.Invoke (RefreshAllDispatcher);
      }
    }

    void ItemCheckedChangedDispatcher (GadgetTest gadget)
    {
      // to parent
      // Select = ById (Test) 
      var action = TEntityAction.Create (
        TCategory.Test,
        TOperation.Select,
        TExtension.ById
      );

      action.Id = gadget.Id;
      action.Param2 = gadget; // preserve

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void EditDispatcher (TActionComponent component)
    {
      Model.Edit (component);

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