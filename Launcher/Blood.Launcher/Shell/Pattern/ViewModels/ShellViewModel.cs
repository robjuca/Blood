/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Diagnostics;

using rr.Library.Infrastructure;
using rr.Library.Helper;
using rr.Library.Communication;

using Shared.Resources;
using Shared.Communication;
using Shared.Types;

using Launcher.Shell.Presentation;
using Launcher.Shell.Pattern.Models;
//---------------------------//

namespace Launcher.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TViewModelAware<TShellModel>, IShellViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (new TShellModel ())
    {
      TypeName = GetType ().Name;

      presentation.ViewModel = this;

      m_Process = new Dictionary<TProcess.TName, Process> ();

      m_DataComm = TDataComm.CreateDefault;

      m_Communication = new TMessagingComm<TDataComm> (m_DataComm);
      m_Communication.Handle += OnCommunicationHandle; // Attach event handler for incoming messages

      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      IniFileManager = TIniFileManager.CreatDefault;
      IniFileManager.SelectPath (filePath, fileName);
    }
    #endregion

    #region View Event
    public void OnAlertsLoaded (object control)
    {
      if (control is TAlerts) {
        m_AlertsCintrol = m_AlertsCintrol ?? (TAlerts) control;
      }
    }

    public void OnSettingsCommadClicked ()
    {
      THelper.DispatcherLater (StartSettingsProcessDispatcher);
    }

    public void OnGadgetMaterialCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetMaterial);
    }

    public void OnGadgetTargetCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetTarget);
    }

    public void OnGadgetTestCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetTest);
    }

    public void OnGadgetRegistrationCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetRegistration);
    }

    public void OnGadgetResultCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetResult);
    }

    public void OnGadgetReportCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetReport);
    }
    #endregion

    #region Dispatcher
    void StartSettingsProcessDispatcher ()
    {
      StartProcess (TProcess.TName.ModuleSettings);

      Model.DisableAll ();
      RaiseChanged ();
    }

    void StartProcessDispatcher (TProcess.TName name)
    {
      StartProcess (name);

      Model.MenuOnly ();
      RaiseChanged ();
    }

    void RemoveProcessPartialDispatcher ()
    {
      foreach (var name in Enum.GetNames (typeof (TProcess.TName))) {
        if (name.Equals (TProcess.TName.ModuleSettings.ToString ())) {
          continue;
        }

        else {
          RemoveProcess ((TProcess.TName) Enum.Parse (typeof (TProcess.TName), name));
        }
      }
    }
    #endregion

    #region Event
    void OnClosing (object sender, System.ComponentModel.CancelEventArgs e)
    {
      foreach (var process in m_Process) {
        try {
          if (process.Value.HasExited.IsFalse ()) {
            process.Value.Kill ();
          }
        }

        catch (Exception ex) {
          throw new Exception (ex.Message);
        }
      }

      m_Process.Clear ();
    }
    #endregion

    #region MessageEvent
    void OnCommunicationHandle (object sender, TMessagingEventArgs<TDataComm> e)
    {
      TProcess.TName module = (TProcess.TName) Enum.Parse (typeof (TProcess.TName), e.Data.ClientName);

      switch (module) {
        case TProcess.TName.ModuleSettings: {
            switch (e.Data.Command) {
              case TCommandComm.Shutdown: {
                  RemoveProcess (module);
                }
                break;

              case TCommandComm.Closed: {
                  RemoveProcess (module);
                  Model.EnableAll ();
                  RaiseChanged ();
                }
                break;

              case TCommandComm.Success: {
                  Model.SettingsValidated ();
                  RaiseChanged ();
                }
                break;

              case TCommandComm.Error: {
                  Model.SettingsHasError ();
                  RaiseChanged ();

                  THelper.DispatcherLater (RemoveProcessPartialDispatcher);
                }
                break;
            }
          }
          break;

        case TProcess.TName.GadgetMaterial: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (module);
                }
                break;
            }
          }
          break;

        case TProcess.TName.GadgetTarget: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (module);
                }
                break;
            }
          }
          break;

        case TProcess.TName.GadgetTest: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (module);
                }
                break;
            }
          }
          break;

        case TProcess.TName.GadgetRegistration: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (module);
                }
                break;
            }
          }
          break;

        case TProcess.TName.GadgetResult: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (module);
                }
                break;
            }
          }
          break;

        case TProcess.TName.GadgetReport: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (module);
                }
                break;
            }
          }
          break;
      }
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      (FrameworkElementView as System.Windows.Window).Closing += OnClosing;

      ValidateProcessAlive ();
      UpdateIniSettings ();

      if (Model.Result.IsValid) {
        OnSettingsCommadClicked ();
      }

      Model.ShowError ();

      if (m_AlertsCintrol.NotNull ()) {
        m_AlertsCintrol.RefreshModel ();
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

    #region Property
    TIniFileManager IniFileManager
    {
      get;
    } 
    #endregion

    #region Fields
    readonly TMessagingComm<TDataComm>                                    m_Communication;
    readonly TDataComm                                                    m_DataComm;
    readonly Dictionary<TProcess.TName, Process>                          m_Process;
    TAlerts                                                               m_AlertsCintrol;
    #endregion

    #region Support
    void ValidateProcessAlive ()
    {
      foreach (var name in Enum.GetNames (typeof (TProcess.TName))) {
        var processName = (TProcess.TName) Enum.Parse (typeof (TProcess.TName), name);
        var processExecutable = TProcess.ModuleExecutable [processName];

        bool alive = System.IO.File.Exists (processExecutable);

        Model.ProcessAlive (processName, alive);
      }

      RaiseChanged ();
    }

    void StartProcess (TProcess.TName name)
    {
      if (m_Process.ContainsKey (name)) {
        var currentProcess = m_Process [name];

        if (currentProcess.HasExited) {
          currentProcess.Start ();
        }
      }

      else {
        var moduleKey = TProcess.ModuleKey [name];
        var processExecutable = TProcess.ModuleExecutable [name];

        Process process = new Process { StartInfo = new ProcessStartInfo (processExecutable, moduleKey) };

        try {
          process.Start ();
          m_Process.Add (name, process);
        }

        catch (Exception) {
          string error = $"Process --{processExecutable}-- NOT FOUND! Launcher will ABORT";
          throw new Exception (error);
        }
      }
    }

    void RemoveProcess (TProcess.TName name)
    {
      // remove process
      if (m_Process.ContainsKey (name)) {
        try {
          var process = m_Process [name];

          if (process.HasExited.IsFalse ()) {
            process.Kill ();
          }

          m_Process.Remove (name);

          if (m_Process.Count.Equals (0)) {
            Model.EnableAll ();
            RaiseChanged ();
          }
        }

        catch (Exception ex) {
          throw new Exception (ex.Message);
        }
      }
    }

    bool UpdateIniSettings ()
    {
      if (IniFileManager.IniFileExists ().IsFalse ()) {
        IniFileManager.SaveChanges ();  // create empty INI file
      }

      Model.Result.CopyFrom (IniFileManager.ValidatePath ());

      if (Model.Result.IsValid) {
        // first time only
        if (IniFileManager.ContainsSection (TProcess.PROCESSMODULESSECTION).IsFalse ()) {
          // Module Process section
          var token = IniFileManager.AddSection (TProcess.PROCESSMODULESSECTION);
          TIniFileManager.AddTrailingComment (token, "Module Process"); // comment.

          // Process Names section
          token = IniFileManager.AddSection (TProcess.PROCESSNAMESSECTION);
          TIniFileManager.AddTrailingComment (token, "Process Names"); // comment.

          // key (all process names separated by '?')
          TIniFileManager.AddKey (token, TProcess.PROCESSNAMES, Model.RequestAllProcessNames ());

          foreach (var process in Model.RequestProcess ()) {
            var section = process.Key.ToString ();
            var key = process.Value.ToString ();

            // section (Process name)
            token = IniFileManager.AddSection (section);

            // key
            TIniFileManager.AddKey (token, TProcess.PROCESSISALIVE, key);
            TIniFileManager.AddKey (token, TProcess.PALETTETHEME, string.Empty);
            TIniFileManager.AddKey (token, TProcess.PALETTEPRIMARY, string.Empty);
            TIniFileManager.AddKey (token, TProcess.PALETTEACCENT, string.Empty);
          }
        }

        // update ini
        else {
          foreach (var process in Model.RequestProcess ()) {
            var section = process.Key.ToString ();
            var key = process.Value.ToString ();

            // key
            IniFileManager.ChangeKey (section, TProcess.PROCESSISALIVE, key);
          }
        }

        IniFileManager.SaveChanges ();
      }

      return (Model.Result.IsValid);
    }
    #endregion
  };
  //---------------------------//

}  // namespace