/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using Shared.Resources;
//---------------------------//

namespace Launcher.Shell.Pattern.Models
{
  public class TShellModel
  {
    #region Property
    public bool IsEnabledSettings
    {
      get
      {
        return (m_IsSettingsEnabled && IsProcessAlive (TProcess.TName.ModuleSettings));
      }
    }

    public bool IsEnabledGadgetMaterial
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetMaterial));
      }
    }

    public bool IsEnabledGadgetTarget
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetTarget));
      }
    }

    public bool IsEnabledGadgetTest
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetTest));
      }
    }

    public bool IsEnabledGadgetRegistration
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetRegistration));
      }
    }

    public bool IsEnabledGadgetResult
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetResult));
      }
    }

    public bool IsEnabledGadgetReport
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetReport));
      }
    }
    #endregion

    #region Constructor
    public TShellModel ()
    {
      m_IsMenuEnabled = false;
      m_IsSettingsEnabled = true;

      m_Process = new Dictionary<TProcess.TName, bool> ();
    }
    #endregion

    #region Members
    internal void EnableAll ()
    {
      m_IsMenuEnabled = true;
      m_IsSettingsEnabled = true;
    }

    internal void DisableAll ()
    {
      m_IsMenuEnabled = false;
      m_IsSettingsEnabled = false;
    }

    internal void MenuOnly ()
    {
      m_IsMenuEnabled = true;
      m_IsSettingsEnabled = false;
    }

    internal void SettingsOnly ()
    {
      m_IsMenuEnabled = false;
      m_IsSettingsEnabled = true;
    }

    internal void ProcessAlive (TProcess.TName name, bool alive)
    {
      if (m_Process.ContainsKey (name)) {
        m_Process [name] = alive;
      }

      else {
        m_Process.Add (name, alive);
      }
    }

    internal string RequestAllProcessNames ()
    {
      string [] names = new string [m_Process.Count];
      int index = 0;

      foreach (var item in m_Process) {
        var name = item.Key.ToString ();

        if (string.IsNullOrEmpty (name).IsFalse ()) {
          names [index++] = item.Key.ToString ();
        }
      }

      string allNames = string.Empty;

      for (int i = 0; i < names.Length; i++) {
        allNames += names [i];

        if (i.Equals (names.Length - 1).IsFalse ()) {
          allNames += "?";
        }
      }

      // processName?...
      return (allNames);
    }

    internal Dictionary<TProcess.TName, bool> RequestProcess ()
    {
      return (m_Process);
    }
    #endregion

    #region Fields
    readonly Dictionary<TProcess.TName, bool>                             m_Process;
    bool                                                                  m_IsMenuEnabled;
    bool                                                                  m_IsSettingsEnabled;
    #endregion

    #region Support
    bool IsProcessAlive (TProcess.TName name)
    {
      return (m_Process.ContainsKey (name) ? m_Process [name] : false);
    } 
    #endregion
  };
  //---------------------------//

}  // namespace