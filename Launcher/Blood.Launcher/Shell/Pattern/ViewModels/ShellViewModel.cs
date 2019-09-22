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
    }
    #endregion

    #region View Event
    public void OnSettingsCommadClicked ()
    {
      THelper.DispatcherLater (StartSettingsProcessDispatcher);
    }

    public void OnMedicalTestMaterialCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetMaterial);
    }

    public void OnMedicalTestTargetCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetTarget);
    }

    public void OnMedicalTestCommadClicked ()
    {
      TDispatcher.BeginInvoke (StartProcessDispatcher, TProcess.TName.GadgetTest);
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
        if (name.Equals (TProcess.TName.ModuleSettings)) {
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
                }
                break;

              case TCommandComm.Error: {
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

    #region Fields
    readonly TMessagingComm<TDataComm>                                    m_Communication;
    readonly TDataComm                                                    m_DataComm;
    readonly Dictionary<TProcess.TName, Process>                          m_Process;
    #endregion

    #region Support
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
    #endregion
  };
  //---------------------------//

}  // namespace