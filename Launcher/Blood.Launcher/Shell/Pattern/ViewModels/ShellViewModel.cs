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

using Shared.Communication;

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

      m_Process = new Dictionary<string, Process> ();
      m_Modules = new Dictionary<string, string> ();

      string [] keys = new string []
        {
          "Module.Settings",
        };

      for (int i = 0; i < Modules.Length; i++) {
        m_Modules.Add (Modules [i], keys [i]);
      }

      m_CurrentModule = TProcessName.Settings;

      m_DataComm = TDataComm.CreateDefault;

      m_Communication = new TMessagingComm<TDataComm> (m_DataComm);
      m_Communication.Handle += OnCommunicationHandle; // Attach event handler for incoming messages
    }
    #endregion

    #region View Event
    public void OnSettingsCommadClicked ()
    {
      m_CurrentModule = TProcessName.Settings; 

      THelper.DispatcherLater (StartSettingsProcessDispatcher);
    }
    #endregion

    #region Dispatcher
    void StartSettingsProcessDispatcher ()
    {
      if (m_CurrentModule.Equals (TProcessName.Settings)) {
        var module = m_CurrentModule.ToString ();
        var key = m_Modules [module];
        var processName = $"Blood.Module.{module}.exe";

        if (m_Process.ContainsKey (key)) {
          if (m_Process [key].HasExited) {
            m_Process [key].Start ();
          }
        }

        else {
          var processKey = key;

          Process process = new Process { StartInfo = new ProcessStartInfo (processName, processKey) };

          try {
            process.Start ();
            m_Process.Add (key, process);
          }

          catch (Exception) {
            string error = $"Process --{processName}-- NOT FOUND! Launcher will ABORT";
            throw new Exception (error);
          }
        }

        Model.DisableAll ();
        RaiseChanged ();
      }
    }

    void StartProcessDispatcher (string processName)
    {
      var module = m_CurrentModule.ToString ();
      var key = m_Modules [module];

      processName += $".{module}.exe";

      if (m_Process.ContainsKey (key)) {
        if (m_Process [key].HasExited) {
          m_Process [key].Start ();
        }
      }

      else {
        var processKey = key;

        Process process = new Process { StartInfo = new ProcessStartInfo (processName, processKey) };
        process.Start ();

        m_Process.Add (key, process);
      }

      Model.MenuOnly ();
      RaiseChanged ();
    }

    void RemoveProcessPartialDispatcher ()
    {
      foreach (var module in Modules) {
        if (module.Equals (TProcessName.Settings.ToString())) {
          continue;
        }

        RemoveProcess (module);
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
      var module = Enum.Parse (typeof (TProcessName), e.Data.ClientName);

      switch (module) {
        case TProcessName.Settings: {
            switch (e.Data.Command) {
              case TCommandComm.Shutdown: {
                  RemoveProcess (SETTINGS);
                }
                break;

              case TCommandComm.Closed: {
                  RemoveProcess (SETTINGS);
                  Model.EnableAll ();
                  RaiseChanged ();
                }
                break;

              case TCommandComm.Success: {
                }
                break;

              case TCommandComm.Error: {
                  Model.SettingsOnly ();
                  RaiseChanged ();

                  THelper.DispatcherLater (RemoveProcessPartialDispatcher);
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

      OnSettingsCommadClicked ();
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

    #region Data
    enum TProcessName
    {
      Settings,
    };
    #endregion

    #region Property
    static string SETTINGS
    {
      get
      {
        return (TProcessName.Settings.ToString ());
      }
    }
    #endregion

    #region Fields
    readonly TMessagingComm<TDataComm>                                    m_Communication;
    readonly TDataComm                                                    m_DataComm;
    readonly Dictionary<string, Process>                                  m_Process;
    readonly Dictionary<string, string>                                   m_Modules;
    TProcessName                                                          m_CurrentModule;
    static readonly string []                                             Modules = new string [] { SETTINGS };
    #endregion

    #region Support
    void RemoveProcess (string moduleName)
    {
      // remove process
      var key = m_Modules [moduleName];

      if (m_Process.ContainsKey (key)) {
        try {
          var process = m_Process [key];

          if (process.HasExited.IsFalse ()) {
            process.Kill ();
          }

          m_Process.Remove (key);

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
    #endregion
  };
  //---------------------------//

}  // namespace