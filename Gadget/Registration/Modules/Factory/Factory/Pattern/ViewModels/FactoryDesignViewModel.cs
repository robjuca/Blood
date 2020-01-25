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

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;
using Shared.Gadget.Models.Action;
using Shared.Gadget.Registration;

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
      : base (new TFactoryDesignModel ())
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
        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.Design, TypeInfo)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            if (message.Support.Argument.Args.Param1 is TActionComponent component) {
              var propertyName = message.Support.Argument.Args.PropertyName;

              Model.SelectModel (propertyName, component);
            }

            TDispatcher.Invoke (RefreshDesignDispatcher);
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestDesignDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            TDispatcher.Invoke (RefreshDesignDispatcher);
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
        RaiseChanged ();
      }
    }

    void RequestDesignDispatcher (TEntityAction action)
    {
      // to Sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.Design, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
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