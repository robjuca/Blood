﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Action;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;
using Shared.Gadget.Models.Action;
using Shared.Gadget.Test;

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryDesignViewModel", typeof (IFactoryDesignViewModel))]
  public class TFactoryDesignViewModel : TViewModelAware<TFactoryDesignModel>, IHandleMessageInternal, IFactoryDesignViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryDesignViewModel (IFactoryPresentation presentation)
      : base (presentation, new TFactoryDesignModel ())
    {
      TypeName = GetType ().Name;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.NotNull ()) {
        if (message.IsModule (TResource.TModule.Factory)) {
          // from Sibling
          if (message.Node.IsSiblingToMe (TChild.Design, TypeInfo)) {
            // PropertySelect
            if (message.IsAction (TInternalMessageAction.PropertySelect)) {
              var propertyName = message.Support.Argument.Args.PropertyName;

              if (propertyName.Equals ("GadgetAdd", StringComparison.InvariantCulture)) {
                if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                  Model.AddModel (component);
                }

                TDispatcher.Invoke (RefreshDesignDispatcher);
              }

              if (propertyName.Equals ("GadgetRemove", StringComparison.InvariantCulture)) {
                if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                  Model.RemoveModel (component);
                }

                TDispatcher.Invoke (RefreshDesignDispatcher);
              }

              if (propertyName.Equals ("edit", StringComparison.InvariantCulture)) {
                if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                  Model.Edit (component);
                }

                TDispatcher.Invoke (RefreshDesignDispatcher);
              }

              var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);

              if (action.NotNull ()) {
                action.Param1 = propertyName;
                TDispatcher.BeginInvoke (PropertySelectDispatcher, action);
              }
            }

            // Cleanup
            if (message.IsAction (TInternalMessageAction.Cleanup)) {
              Model.Cleanup ();
              TDispatcher.Invoke (RefreshDesignDispatcher);
            }
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnDesignLoaded (object control)
    {
      if (control is TComponentDesignControl) {
        m_DesignControl = m_DesignControl ?? (TComponentDesignControl) control;
      }
    }
    #endregion

    #region Dispatcher
    void RefreshDesignDispatcher ()
    {
      if (m_DesignControl.NotNull ()) {
        m_DesignControl.RefreshDesign ();
        ApplyChanges ();
      }
    }

    void PropertySelectDispatcher (TEntityAction action)
    {
      if (action.Param1 is string propertyName) {
        if (propertyName.Equals ("DescriptionProperty", StringComparison.InvariantCulture) || propertyName.Equals ("TextProperty", StringComparison.InvariantCulture) || propertyName.Equals ("ExternalLinkProperty", StringComparison.InvariantCulture)) {
          Model.SelectModel (action);
          TDispatcher.Invoke (RefreshDesignDispatcher);
        }
      }
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
    TComponentDesignControl                                     m_DesignControl;
    #endregion
  };
  //---------------------------//

}  // namespace