/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Controls;
using rr.Library.Helper;
using rr.Library.Types;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;
using Shared.Gadget.Models.Action;

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryPropertyViewModel", typeof (IFactoryPropertyViewModel))]
  public class TFactoryPropertyViewModel : TViewModelAware<TFactoryPropertyModel>, IHandleMessageInternal, IFactoryPropertyViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryPropertyViewModel (IFactoryPresentation presentation)
      : base (new TFactoryPropertyModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);

      Model.PropertyChanged += OnModelPropertyChanged;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from parent
        if (message.Node.IsParentToMe (TChild.Property)) {
          // RefreshModel
          if (message.IsAction (TInternalMessageAction.RefreshModel)) {
            var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
            TDispatcher.BeginInvoke (RefreshCollectionDispatcher, action);
          }

          // Edit
          if (message.IsAction (TInternalMessageAction.Edit)) {
            TDispatcher.BeginInvoke (EditDispatcher, message.Support.Argument.Args.Param1);
          }

          // EditLeave
          if (message.IsAction (TInternalMessageAction.EditLeave)) {
            if (IsViewModeEdit) {
              OnCancelCommadClicked ();
            }
          }

          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Insert
            if (message.Support.Argument.Types.IsOperation (TOperation.Insert)) {
              TDispatcher.Invoke (InsertSuccessDispatcher);
            }

            // Change - Full
            if (message.Support.Argument.Types.IsOperation (TOperation.Change, TExtension.Full)) {
              TDispatcher.Invoke (ChangeSuccessDispatcher);
            }
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnPropertyGridComponentLoaded (object control)
    {
      if (control is TPropertyGrid) {
        m_PropertyGridComponent = m_PropertyGridComponent ?? (TPropertyGrid) control;
      }
    }

    public void OnPropertyGridExtensionLoaded (object control)
    {
      if (control is TPropertyGrid) {
        m_PropertyGridExtension = m_PropertyGridExtension ?? (TPropertyGrid) control;
      }
    }

    void OnModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      PropertySelect (e.PropertyName);
    }

    public void OnApplyCommadClicked ()
    {
      Model.ShowPanels ();
      RaiseChanged ();

      var action = TEntityAction.Create (TCategory.Material, TOperation.Insert);

      if (IsViewModeEdit) {
        action = TEntityAction.Create (TCategory.Material, TOperation.Change, TExtension.Full);
      }

      Model.RequestModel (action);

      TDispatcher.BeginInvoke (ApplyDispatcher, action);
    }

    public void OnCancelCommadClicked ()
    {
      TDispatcher.Invoke (CleanupDispatcher);
      TDispatcher.Invoke (EditLeaveDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      if (m_PropertyGridComponent.NotNull ()) {
        m_PropertyGridComponent.RefreshPropertyList ();
      }

      if (m_PropertyGridExtension.NotNull ()) {
        m_PropertyGridExtension.RefreshPropertyList ();
      }
    }

    void CleanupDispatcher ()
    {
      Cleanup ();

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);

      PropertySelect ("all");
    }

    void ApplyDispatcher (TEntityAction action)
    {
      // to parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Property, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void InsertSuccessDispatcher ()
    {
      TDispatcher.Invoke (CleanupDispatcher);
      TDispatcher.Invoke (ReloadDispatcher);
    }

    void ChangeSuccessDispatcher ()
    {
      TDispatcher.Invoke (CleanupDispatcher);
      TDispatcher.Invoke (ReloadDispatcher);
      TDispatcher.Invoke (EditLeaveDispatcher);
    }

    void ReloadDispatcher ()
    {
      // to parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Reload, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void EditEnterDispatcher ()
    {
      // to parent request focus
      var message = new TFactoryMessageInternal (TInternalMessageAction.NavigateForm, TChild.Property, TypeInfo);
      message.Support.Argument.Args.Select (TWhere.Factory);
      DelegateCommand.PublishInternalMessage.Execute (message);

      // to parent
      var msg = new TFactoryMessageInternal (TInternalMessageAction.EditEnter, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (msg);
    }

    void EditLeaveDispatcher ()
    {
      // to parent leave focus
      var message = new TFactoryMessageInternal (TInternalMessageAction.NavigateForm, TChild.Property, TypeInfo);
      message.Support.Argument.Args.Select (TWhere.Collection);
      DelegateCommand.PublishInternalMessage.Execute (message);

      // to parent
      var msg = new TFactoryMessageInternal (TInternalMessageAction.EditLeave, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (msg);
    }

    void EditDispatcher (object param1)
    {
      if (param1 is TActionComponent model) {
        SelectViewMode (TViewMode.Edit);

        Model.SelectModel (model);

        TDispatcher.Invoke (RefreshAllDispatcher);
        TDispatcher.Invoke (EditEnterDispatcher);

        // to Sibling
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Edit, TChild.Property, TypeInfo);
        message.Support.Argument.Args.Select (model);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void RefreshCollectionDispatcher (TEntityAction action)
    {
      Model.RefreshModel (action);
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      Model.Cleanup ();
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

    #region Fields
    TPropertyGrid                           m_PropertyGridComponent;
    TPropertyGrid                           m_PropertyGridExtension;
    #endregion

    #region Support
    void PropertySelect (string propertyName)
    {
      if (Model.ValidateProperty (propertyName)) {
        // to Sibling
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.Property, TypeInfo);
        message.Support.Argument.Args.Select (Model.GadgetModel);
        message.Support.Argument.Args.Select (propertyName);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      RaiseChanged ();
    }

    void Cleanup ()
    {
      Model.Cleanup ();
      RaiseChanged ();

      CleanupPropertyControl ();

      ResetViewMode ();

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void CleanupPropertyControl ()
    {
      if (m_PropertyGridComponent.NotNull ()) {
        m_PropertyGridComponent.Cleanup ();
      }

      if (m_PropertyGridExtension.NotNull ()) {
        m_PropertyGridExtension.Cleanup ();
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace76