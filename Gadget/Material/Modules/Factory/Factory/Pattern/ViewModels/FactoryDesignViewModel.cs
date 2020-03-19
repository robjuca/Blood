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
using Shared.Gadget.Models.Action;
using Shared.Gadget.Material;

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
            // Edit
            if (message.IsAction (TInternalMessageAction.Edit)) {
              if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                Model.SelectModel (component);
              }

              TDispatcher.Invoke (RefreshDesignDispatcher);
            }

            // PropertySelect
            if (message.IsAction (TInternalMessageAction.PropertySelect)) {
              if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                Model.SelectModel (component);
              }

              TDispatcher.Invoke (RefreshDesignDispatcher);
            }

            // Cleanup
            if (message.IsAction (TInternalMessageAction.Cleanup)) {
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