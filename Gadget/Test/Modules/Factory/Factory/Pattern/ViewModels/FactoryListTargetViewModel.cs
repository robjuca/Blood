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
  [Export ("ModuleFactoryListTargetViewModel", typeof (IFactoryListTargetViewModel))]
  public class TFactoryListTargetViewModel : TViewModelAware<TFactoryListTargetModel>, IHandleMessageInternal, IFactoryListTargetViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListTargetViewModel (IFactoryPresentation presentation)
      : base (presentation, new TFactoryListTargetModel ())
    {
      TypeName = GetType ().Name;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.NotNull ()) {
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
              if (message.Support.Argument.Types.IsOperation (TOperation.Collection, TExtension.Full)) {
                if (message.Result.IsValid) {
                  // Gadget Target
                  if (message.Support.Argument.Types.IsOperationCategory (TCategory.Target)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                  }
                }
              }

              // Select-ById
              if (message.Support.Argument.Types.IsOperation (TOperation.Select, TExtension.ById)) {
                if (message.Result.IsValid) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, action);
                }
              }
            }

            // Reload
            if (message.IsAction (TInternalMessageAction.Reload)) {
              TDispatcher.Invoke (RefreshAllDispatcher);
              TDispatcher.Invoke (RequestDataDispatcher);
            }
          }

          // from sibling
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
              var propertyName = message.Support.Argument.Args.PropertyName;

              if (propertyName.Equals ("edit", StringComparison.InvariantCulture)) {
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
    }
    #endregion

    #region View Event
    public void OnGadgetChanged ()
    {
      TDispatcher.Invoke (GadgetChangedDispatcher);
    }

    public void OnGadgetItemChecked (GadgetTarget gadget)
    {
      TDispatcher.BeginInvoke (ItemCheckedChangedDispatcher, gadget);
    }

    public void OnGadgetItemUnchecked (GadgetTarget gadget)
    {
      TDispatcher.BeginInvoke (ItemCheckedChangedDispatcher, gadget);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      ApplyChanges ();

      RefreshCollection ("TargetModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Target - Collection - Full 
      var action = TEntityAction.Create (
        TCategory.Target,
        TOperation.Collection,
        TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      // Collection - Full (Target list)
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void ResponseSelectByIdDispatcher (TEntityAction action)
    {
      // to Sibling (Select By Id)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (TComponentModelItem.Create (action));

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void GadgetChangedDispatcher ()
    {
      if (Model.GadgetChanged ()) {
        // to Sibling (Select)
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
        //message.Support.Argument.Args.Select (gadgetComponent);
        // TODO: what for??
        //DelegateCommand.PublishInternalMessage.Execute (message);
      }

      else {
        // to Sibling (Cleanup)
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void RequestDesignDispatcher (TEntityAction action)
    {
      Model.RequestModel (action);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ItemCheckedChangedDispatcher (GadgetTarget gadget)
    {
      Model.GadgetItemChecked (gadget);

      TDispatcher.Invoke (RefreshAllDispatcher);

      var component = TActionComponent.Create (TCategory.Target);
      component.Models.GadgetTargetModel.CopyFrom (gadget);

      Model.Request (component);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (component);
      message.Support.Argument.Args.Select (gadget.IsChecked ? "GadgetAdd" : "GadgetRemove");

      if (Model.IsEditMode || Model.HasGadgetChecked) {
        message.Support.Argument.Types.ReportData.SelectLock ();
      }

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