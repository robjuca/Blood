﻿/*----------------------------------------------------------------
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

using Server.Models.Action;
using Server.Models.Infrastructure;

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
      : base (presentation, new TFactoryPropertyModel ())
    {
      TypeName = GetType ().Name;

      Model.PropertyChanged += OnModelPropertyChanged;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.NotNull ()) {
        if (message.IsModule (TResource.TModule.Factory)) {
          // from parent
          if (message.Node.IsParentToMe (TChild.Property)) {
            // RefreshModel
            if (message.IsAction (TInternalMessageAction.RefreshModel)) {
              TDispatcher.BeginInvoke (RefreshModelDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
            }

            // Edit
            if (message.IsAction (TInternalMessageAction.Edit)) {
              if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                TDispatcher.BeginInvoke (EditDispatcher, component);
              }
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

          // from Sibling
          if (message.Node.IsSiblingToMe (TChild.Property, TypeInfo)) {
            // Response
            if (message.IsAction (TInternalMessageAction.Response)) {
              TDispatcher.BeginInvoke (ResponseModelDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
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
      ApplyChanges ();

      // Insert
      var action = TEntityAction.Create (TCategory.Registration, TOperation.Insert);

      if (IsViewModeEdit) {
        // Change-Full
        action = TEntityAction.Create (TCategory.Registration, TOperation.Change, TExtension.Full);
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
      ApplyChanges ();

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

    void ResponseModelDispatcher (TEntityAction action)
    {
      TDispatcher.BeginInvoke (ApplyDispatcher, action);  
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

    void EditDispatcher (TActionComponent component)
    {
      // Id must exist
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Registration)) {
          SelectViewMode (TViewMode.Edit);

          Model.SelectModel (component);

          TDispatcher.Invoke (RefreshAllDispatcher);
          TDispatcher.Invoke (EditEnterDispatcher);

          // to Sibling
          var message = new TFactorySiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.Property, TypeInfo);
          message.Support.Argument.Args.Select (component);
          message.Support.Argument.Args.Select ("edit");

          DelegateCommand.PublishInternalMessage.Execute (message);
        }
      }
    }

    void RefreshModelDispatcher (TEntityAction action)
    {
      action.ThrowNull ();
      //TODO: what for??
      
      ApplyChanges ();
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
      var component = TActionComponent.Create (TCategory.Registration);
      Model.Request (component);

      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.Property, TypeInfo);
      message.Support.Argument.Args.Select (propertyName);
      message.Support.Argument.Args.Select (component);

      DelegateCommand.PublishInternalMessage.Execute (message);

      ApplyChanges ();
    }

    void Cleanup ()
    {
      Model.Cleanup ();
      ApplyChanges ();

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