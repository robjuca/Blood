/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Infrastructure;
using Server.Models.Action;

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
  [Export ("ModuleFactoryListModifyViewModel", typeof (IFactoryListModifyViewModel))]
  public class TFactoryListModifyViewModel : TViewModelAware<TFactoryListModifyModel>, IHandleMessageInternal, IFactoryListModifyViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListModifyViewModel (IFactoryPresentation presentation)
      : base (presentation, new TFactoryListModifyModel ())
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
            // Response
            if (message.IsAction (TInternalMessageAction.Response)) {
              // Change - Many
              if (message.Support.Argument.Types.IsOperation (TOperation.Change, TExtension.Many)) {
                TDispatcher.Invoke (ChangeSuccessDispatcher);
              }
            }
          }

          // from Sibling
          if (message.Node.IsSiblingToMe (TChild.List, TypeInfo)) {
            // PropertySelect
            if (message.IsAction (TInternalMessageAction.PropertySelect)) {
              var propertyName = message.Support.Argument.Args.PropertyName;

              if (propertyName.Equals ("modify", StringComparison.InvariantCulture)) {
                if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                  TDispatcher.BeginInvoke (ModifyDispatcher, component);
                }
              }
            }

            // Cleanup
            if (message.IsAction (TInternalMessageAction.Cleanup)) {
              Model.Cleanup ();
              TDispatcher.Invoke (RefreshAllDispatcher);
            }
          }
        }
      }
     }
    #endregion

    #region Event
    public void OnSelectorContentTestChecked ()
    {
      TDispatcher.Invoke (SelectorContentTestCheckedDispatcher);
    }

    public void OnSelectorContentTargetChecked ()
    {
      TDispatcher.Invoke (SelectorContentTargetCheckedDispatcher);
    }

    public void OnContentTestTargetSelectionChanged (object context)
    {
      // content Test
      if (context is GadgetTest gadget) {
        TDispatcher.BeginInvoke (ContentTestTargetChangedDispatcher, gadget);
      }
    }

    public void OnContentTargetSelectionChanged (object context)
    {
      // content Target
      if (context is GadgetTest gadget) {
        TDispatcher.BeginInvoke (ContentTargetChangedDispatcher, gadget);
      }
    }

    public void OnModifyCommandClicked ()
    {
      TDispatcher.Invoke (ModifyCommandDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      ApplyChanges ();

      RefreshCollection ("ContentTestModelItemsViewSource");
      RefreshCollection ("ContentTargetModelItemsViewSource");
    }

    void ModifyDispatcher (TActionComponent component)
    {
      if (component.NotNull ()) {
        Model.ModifyEnter (component);
      }

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void SelectorContentTestCheckedDispatcher ()
    {
      Model.SelectorContentTestIsChecked ();
      ApplyChanges ();
    }

    void SelectorContentTargetCheckedDispatcher ()
    {
      Model.SelectorContentTargetIsChecked ();
      ApplyChanges ();
    }

    void ContentTestTargetChangedDispatcher (GadgetTest gadget)
    {
      Model.ContentTestTargetChanged (gadget);
      ApplyChanges ();
    }

    void ContentTargetChangedDispatcher (GadgetTest gadget)
    {
      Model.ContentTargetChanged (gadget);
      ApplyChanges ();
    }

    void ModifyCommandDispatcher ()
    {
      var component = TActionComponent.Create (TCategory.Result);
      Model.Request (component);

      var entityAction = TEntityAction.Create (TCategory.Result, TOperation.Change, TExtension.Many);
      TActionConverter.Modify (TCategory.Result, component, entityAction);

      // to parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (entityAction);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ChangeSuccessDispatcher ()
    {
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