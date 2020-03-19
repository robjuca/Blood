/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Message;
using rr.Library.Helper;

using Shared.Message;
using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Gadget.Target.Shell.Presentation;
using Gadget.Target.Shell.Pattern.Models;
//---------------------------//

namespace Gadget.Target.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TShellViewModel<TShellModel>, IShellViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (presentation, new TShellModel (), TProcess.GADGETTARGET)
    {
    }
    #endregion

    #region Overrides
    public override void ProcessMessage (TMessageModule message)
    {
      if (message.NotNull ()) {
        // services
        if (message.IsModule (TResource.TModule.Services)) {
          // SettingsValidated
          if (message.IsAction (TMessageAction.SettingsValidated)) {
            SelectAuthentication (message.Support.Argument.Types.Authentication);

            // sucess
            if (message.Support.IsActionStatus (TActionStatus.Success)) {
              TDispatcher.Invoke (DatabaseSettingsSuccessDispatcher);
            }

            // error
            if (message.Support.IsActionStatus (TActionStatus.Error)) {
              TDispatcher.Invoke (DatabaseSettingsErrorDispatcher);
            }
          }
        }

        // focus
        if (message.IsAction (TMessageAction.Focus)) {
          if (message.Support.Argument.Args.IsWhere (TWhere.Collection)) {
            OnCollectionCommadClicked ();
          }

          if (message.Support.Argument.Args.IsWhere (TWhere.Factory)) {
            OnFactoryCommadClicked ();
          }
        }
      }
    }

    public override void RefreshProcess ()
    {
      // notify modules
      var message = new TShellMessage (TMessageAction.RefreshProcess, TypeInfo);
      DelegateCommand.PublishModuleMessage.Execute (message);
    }
    #endregion

    #region View Event
    public void OnCollectionCommadClicked ()
    {
      DelegateCommand.NotifyNavigateRequestMessage.Execute (new TNavigateRequestMessage (TNavigateMessage.TSender.Shell, TNavigateMessage.TWhere.Collection));

      ApplyChanges ();
    }

    public void OnFactoryCommadClicked ()
    {
      DelegateCommand.NotifyNavigateRequestMessage.Execute (new TNavigateRequestMessage (TNavigateMessage.TSender.Shell, TNavigateMessage.TWhere.Factory));

      ApplyChanges ();
    }
    #endregion

    #region Dispatcher
    void RequestServiceValidationDispatcher ()
    {
      // settings validating
      var message = new TShellMessage (TMessageAction.SettingsValidating, TypeInfo);
      DelegateCommand.PublishModuleMessage.Execute (message);
    }

    void DatabaseSettingsSuccessDispatcher ()
    {
      Model.ClearPanels ();
      Model.DatabaseStatus (true);
      Model.Unlock ();

      ApplyChanges ();

      OnCollectionCommadClicked ();

      // notify modules
      var message = new TShellMessage (TMessageAction.DatabaseValidated, TypeInfo);
      DelegateCommand.PublishModuleMessage.Execute (message);
    }

    void DatabaseSettingsErrorDispatcher ()
    {
      Model.ClearPanels ();

      ApplyChanges ();
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      base.Initialize (); // must be called to apply theme

      TDispatcher.Invoke (RequestServiceValidationDispatcher);
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