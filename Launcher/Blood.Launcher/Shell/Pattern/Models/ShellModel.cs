/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
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

    public bool IsEnabledMedicalTestMaterial
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetMaterial));
      }
    }

    public bool IsEnabledMedicalTestTarget
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetTarget));
      }
    }

    public bool IsEnabledMedicalTest
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetTest));
      }
    }

    public bool IsEnabledMedicalCareRegistration
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetRegistration));
      }
    }

    public bool IsEnabledMedicalCareTests
    {
      get
      {
        return (m_IsMenuEnabled && IsProcessAlive (TProcess.TName.GadgetTests));
      }
    }

    public bool IsEnabledMedicalCareReport
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