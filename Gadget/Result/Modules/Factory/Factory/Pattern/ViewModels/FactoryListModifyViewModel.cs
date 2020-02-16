/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;

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
      : base (new TFactoryListModifyModel ())
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

        }

        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.List, TypeInfo)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            var propertyName = message.Support.Argument.Args.PropertyName;

            if (propertyName.Equals ("modify")) {
              if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                TDispatcher.BeginInvoke (ModifyDispatcher, component);
              }
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

    public void OnTestSelectionChanged ()
    {
      TDispatcher.Invoke (TestChangedDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      RefreshCollection ("TestModelItemsViewSource");
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
      RaiseChanged ();
    }

    void SelectorContentTargetCheckedDispatcher ()
    {
      Model.SelectorContentTargetIsChecked ();
      RaiseChanged ();
    }
    void TestChangedDispatcher ()
    {
      Model.TestChanged ();
      RaiseChanged ();
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