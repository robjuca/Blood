/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

using rr.Library.Types;
using rr.Library.Helper;
using rr.Library.Message;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Types;
using Shared.Resources;
using Shared.Message;
using Shared.ViewModel;
using Shared.Communication;

using Module.Settings.Shell.Presentation;
using Module.Settings.Shell.Pattern.Models;
//---------------------------//

namespace Module.Settings.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TShellViewModel<TShellModel>, IShellViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (presentation, new TShellModel (), TProcess.MODULESETTINGS)
    {
    }
    #endregion

    #region View Event
    public void OnSettingsReportCommadClicked ()
    {
      DelegateCommand.NotifyNavigateRequestMessage.Execute (new TNavigateRequestMessage (TNavigateMessage.TSender.Shell, TNavigateMessage.TWhere.Report));

      ApplyChanges ();
    }

    public void OnFactoryDatabaseCommadClicked ()
    {
      DelegateCommand.NotifyNavigateRequestMessage.Execute (new TNavigateRequestMessage (TNavigateMessage.TSender.Shell, TNavigateMessage.TWhere.Database));

      ApplyChanges ();
    }

    public void OnFactorySupportCommadClicked ()
    {
      DelegateCommand.NotifyNavigateRequestMessage.Execute (new TNavigateRequestMessage (TNavigateMessage.TSender.Shell, TNavigateMessage.TWhere.Support));

      ApplyChanges ();
    }
    #endregion

    #region Overrides
    public override void ProcessMessage (TMessageModule message)
    {
      if (message.NotNull ()) {
        // services
        if (message.IsModule (TResource.TModule.Services)) {
          SelectAuthentication (message.Support.Argument.Types.Authentication);

          // SettingsValidated
          if (message.IsAction (TMessageAction.SettingsValidated)) {
            // Success
            if (message.Support.IsActionStatus (TActionStatus.Success)) {
              TDispatcher.Invoke (DatabaseSettingsSuccessDispatcher);
            }

            // Error
            if (message.Support.IsActionStatus (TActionStatus.Error)) {
              TDispatcher.Invoke (DatabaseSettingsErrorDispatcher);
            }
          }

          // Response
          if (message.IsAction (TMessageAction.Response)) {
            // (Select - Settings)
            if (message.Support.Argument.Types.IsOperation (TOperation.Select, TExtension.Settings)) {
              var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);

              TDispatcher.BeginInvoke (SelectSettingsDispatcher, action);
            }

            // (Change - Settings)
            if (message.Support.Argument.Types.IsOperation (TOperation.Change, TExtension.Settings)) {
              var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);

              TDispatcher.BeginInvoke (SelectSettingsDispatcher, action);
            }
          }
        }

        // factory
        if (message.IsModule (TResource.TModule.Factory)) {
          // Changed
          if (message.IsAction (TMessageAction.Changed)) {
            Model.Lock ();
            Model.ShowPanels ();
            Model.SnackbarContent.SetMessage (Properties.Resource.RES_VALIDATING);
            ApplyChanges ();

            TDispatcher.Invoke (OpenSnackbarDispatcher);
            TDispatcher.BeginInvoke (SaveSettingsDispatcher, message.Support.Argument.Types.ConnectionData);
          }

          // Authentication
          if (message.IsAction (TMessageAction.Authentication)) {
            Model.Lock ();
            Model.ShowPanels ();
            Model.SnackbarContent.SetMessage (Properties.Resource.RES_VALIDATING);
            ApplyChanges ();

            TDispatcher.Invoke (OpenSnackbarDispatcher);
            TDispatcher.BeginInvoke (ChangeAuthenticationDispatcher, message.Support.Argument.Types.Authentication);
          }

          // Request
          if (message.IsAction (TMessageAction.Request)) {
            Model.Lock ();
            Model.ShowPanels ();
            Model.SnackbarContent.SetMessage (Properties.Resource.RES_VALIDATING);
            ApplyChanges ();

            TDispatcher.Invoke (OpenSnackbarDispatcher);

            var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);

            var msg = new TShellMessage (TMessageAction.Request, TypeInfo);
            msg.Support.Argument.Types.Select (action);

            DelegateCommand.PublishModuleMessage.Execute (msg);
          }
        }
      }
    }
    #endregion

    #region Dispatcher
    void ShowSnackbarDispatcher (bool shutdown = false)
    {
      Task.Factory.StartNew (() =>
        {
        Thread.Sleep (500);
        }, 
        CancellationToken.None, 
        TaskCreationOptions.None, 
        TaskScheduler.Default).
        ContinueWith (t =>
          {
            if (FrameworkElementView.FindName ("MainSnackbar") is MaterialDesignThemes.Wpf.Snackbar bar) {
              bar.MessageQueue.Enqueue (Model.SnackbarContent.Message);
            }

            if (shutdown) {
              TDispatcher.Invoke (ShutdownDispatcher);
            }
          }, 
          TaskScheduler.FromCurrentSynchronizationContext ()
      );
    }

    void OpenSnackbarDispatcher ()
    {
      if (FrameworkElementView.FindName ("SnackbarActive") is System.Windows.Controls.CheckBox box) {
        box.IsChecked = true;
      }
    }

    void CloseSnackbarDispatcher ()
    {
      if (FrameworkElementView.FindName ("SnackbarActive") is System.Windows.Controls.CheckBox box) {
        box.IsChecked = false;
      }
    }

    void LoadSettingsDispatcher ()
    {
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      LoadSupportSettings (filePath, fileName);
      LoadDatabaseSettings (filePath, fileName);
    }

    void SaveSettingsDispatcher (TDatabaseAuthentication databaseAuthentication)
    {
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      DatabaseConnection = DatabaseConnection ?? new TDatabaseConnection (filePath, fileName);

      if (DatabaseConnection.Save (databaseAuthentication)) {
        DatabaseConnection.Request (); // update

        // notify edit
        var message = new TShellMessage (TMessageAction.Response, TypeInfo);
        message.Support.Argument.Types.ConnectionData.CopyFrom (databaseAuthentication);

        DelegateCommand.PublishModuleMessage.Execute (message);

        // settings validating
        TDispatcher.Invoke (RequestServiceValidationDispatcher);
      }

      else {
        TDispatcher.Invoke (CloseSnackbarDispatcher);

        var errorMessage = new TErrorMessage (Properties.Resource.RES_ERROR, Properties.Resource.RES_SAVE, (string) DatabaseConnection.Result.ErrorContent)
        {
          Severity = TSeverity.Hight
        };

        TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, errorMessage);
      }
    }

    void ChangeAuthenticationDispatcher (TAuthentication authentication)
    {
      DatabaseConnection.ChangeAuthentication (authentication);

      TDispatcher.Invoke (LoadSettingsDispatcher);
    }

    void RequestServiceValidationDispatcher ()
    {
      // settings validating
      var message = new TShellMessage (TMessageAction.SettingsValidating, TypeInfo);
      message.Support.Argument.Types.ConnectionData.CopyFrom (DatabaseConnection.CurrentDatabase);

      DelegateCommand.PublishModuleMessage.Execute (message);
    }

    void DatabaseSettingsSuccessDispatcher ()
    {
      // notify main process
      NotifyProcess (TCommandComm.Success);

      // update INI file
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      var data = new TDatabaseConnection (filePath, fileName);

      if (data.Request ()) {
        TDispatcher.Invoke (CloseSnackbarDispatcher);

        Model.SnackbarContent.SetMessage (Properties.Resource.RES_WELLCOME);
        TDispatcher.BeginInvoke (ShowSnackbarDispatcher, false);

        TDispatcher.Invoke (DatabaseValidateDispatcher);
      }

      else {
        var errorMessage = new TErrorMessage (Properties.Resource.RES_ERROR, Properties.Resource.RES_LOAD, (string) data.Result.ErrorContent)
        {
          Severity = TSeverity.Hight
        };

        TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, errorMessage);
      }
    }

    void DatabaseSettingsErrorDispatcher ()
    {
      // notify main process
      NotifyProcess (TCommandComm.Error);

      Model.ClearPanels ();
      Model.DatabaseStatus (false);
      Model.Unlock ();

      ApplyChanges ();

      TDispatcher.Invoke (CloseSnackbarDispatcher);

      OnFactoryDatabaseCommadClicked (); // database factory
    }

    void DatabaseValidateDispatcher ()
    {
      if (m_DatabaseValidatingInProgress.IsFalse ()) {
        Model.ClearPanels ();
        Model.DatabaseStatus (true);
        Model.Unlock ();

        // open and validate current database (for sure)
        if (DatabaseConnection.IsAuthentication) {
          // to services (Select - Settings)
          var action = TEntityAction.Create (TCategory.Settings, TOperation.Select, TExtension.Settings);

          var message = new TShellMessage (TMessageAction.Request, TypeInfo);
          message.Support.Argument.Types.Select (action);

          DelegateCommand.PublishModuleMessage.Execute (message);

          m_DatabaseValidatingInProgress = true;

          Model.Lock ();
          Model.MenuLeftDisable ();
        }
      }

      ApplyChanges ();
    }

    void SelectSettingsDispatcher (TEntityAction action)
    {
      m_DatabaseValidatingInProgress = false;

      if (action.Result.IsValid) {
        Model.Unlock ();
        Model.MenuLeftEnable ();

        Model.Select (action);

        // to module
        var entityAction = TEntityAction.CreateDefault;
        entityAction.Param1 = Model.ComponentModelItem;

        var message = new TShellMessage (TMessageAction.DatabaseValidated, TypeInfo);
        message.Support.Argument.Types.Select (entityAction);

        DelegateCommand.PublishModuleMessage.Execute (message);

        // update INI support section
        //SupportSettings.Change ("ColumnWidth", action.ModelAction.SettingsModel.ColumnWidth.ToString ()); 

        OnSettingsReportCommadClicked (); // show current settings
      }

      else {
        Model.MenuLeftDisable ();
      }

      ApplyChanges ();
    }

    void ShutdownDispatcher ()
    {
      if (Properties.Settings.Default.Shutdown) {
        Properties.Settings.Default.Shutdown = false;
        Properties.Settings.Default.Save ();

        Task.Factory.StartNew (() =>
          {
            Thread.Sleep (4500);
          }, 
          CancellationToken.None, 
          TaskCreationOptions.None, 
          TaskScheduler.Default)
          .ContinueWith (t =>
            {
              NotifyProcess (TCommandComm.Shutdown);

              (FrameworkElementView as System.Windows.Window).Close ();
            }, 
            TaskScheduler.FromCurrentSynchronizationContext ()
        );
      }
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      base.Initialize (); // must be called to apply theme
        
      OnFactoryDatabaseCommadClicked (); // show current database settings

      Model.Lock ();
      Model.ShowPanels ();
      Model.SnackbarContent.SetMessage (Properties.Resource.RES_VALIDATING);
      ApplyChanges ();

      TDispatcher.Invoke (OpenSnackbarDispatcher);
      TDispatcher.Invoke (LoadSettingsDispatcher);
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

    TDatabaseConnection DatabaseConnection
    {
      get;
      set;
    }

    TSupportSettings SupportSettings
    {
      get;
      set;
    }
    #endregion

    #region Field
    bool                                    m_DatabaseValidatingInProgress;
    #endregion

    #region Support
    void LoadDatabaseSettings (string filePath, string fileName)
    {
      DatabaseConnection = DatabaseConnection ?? new TDatabaseConnection (filePath, fileName);

      // database
      if (DatabaseConnection.Request ()) {
        // notify factory database
        // SQL
        var message = new TShellMessage (TMessageAction.Response, TypeInfo);
        message.Support.Argument.Types.ConnectionData.CopyFrom (DatabaseConnection.Request (TAuthentication.SQL));

        DelegateCommand.PublishModuleMessage.Execute (message);

        // Windows
        message = new TShellMessage (TMessageAction.Response, TypeInfo);
        message.Support.Argument.Types.ConnectionData.CopyFrom (DatabaseConnection.Request (TAuthentication.Windows));

        DelegateCommand.PublishModuleMessage.Execute (message);

        // settings validating
        TDispatcher.Invoke (RequestServiceValidationDispatcher);
      }

      else {
        var errorMessage = new TErrorMessage (Properties.Resource.RES_ERROR, Properties.Resource.RES_LOAD_DATABASE, (string) DatabaseConnection.Result.ErrorContent)
        {
          Severity = TSeverity.Hight
        };

        TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, errorMessage);
      }
    }

    void LoadSupportSettings (string filePath, string fileName)
    {
      SupportSettings = SupportSettings ?? TSupportSettings.Create (filePath, fileName);

      // supprt
      if (SupportSettings.Validate ().IsFalse ()) {
        var errorMessage = new TErrorMessage (Properties.Resource.RES_ERROR, Properties.Resource.RES_LOAD_SUPPORT, (string) SupportSettings.Result.ErrorContent)
        {
          Severity = TSeverity.Hight
        };

        TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, errorMessage);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace