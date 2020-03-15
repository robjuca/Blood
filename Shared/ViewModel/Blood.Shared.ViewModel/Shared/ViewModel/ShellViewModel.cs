/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

using rr.Library.Helper;
using rr.Library.Infrastructure;
using rr.Library.Types;
using rr.Library.Communication;

using Shared.Types;
using Shared.Communication;
using Shared.Resources;
using Shared.Message;
//---------------------------//

namespace Shared.ViewModel
{
  public class TShellViewModel<T> : TViewModelAware<T>, IShellViewModel
    where T : TShellModelReference
  {
    #region Property
    public string ProcessName
    {
      get;
    } 
    #endregion

    #region Constructor
    public TShellViewModel (T model, string processName)
      : base (model)
    {
      ProcessName = processName;

      TypeName = GetType ().Name;

      m_ModalCount = 0;

      m_DataComm = TDataComm.CreateDefault;

      m_Communication = new TMessagingComm<TDataComm> (m_DataComm);
      m_Communication.Handle += OnCommunicationHandle;
    }

    public TShellViewModel (IPresentation presentation, T model, string processName)
      : this (model, processName)
    {
      if (presentation.NotNull ()) {
        presentation.ViewModel = this;
      }
    }
    #endregion

    #region Interface Members
    public void Message (TMessageModule message)
    {
      if (message.NotNull ()) {
        // error
        if (message.IsAction (TMessageAction.Error)) {
          TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, message.Support.ErrorMessage);
        }

        // modal enter
        if (message.IsAction (TMessageAction.ModalEnter)) {
          if (m_ModalCount.Equals (0)) {
            Model.ModalEnter ();
            Model.ShowPanels ();

            ApplyChanges ();
          }

          m_ModalCount++;
        }

        // modal leave
        if (message.IsAction (TMessageAction.ModalLeave)) {
          if (m_ModalCount > 0) {
            m_ModalCount--;

            if (m_ModalCount.Equals (0)) {
              Model.ModalLeave ();
              Model.ClearPanels ();

              ApplyChanges ();
            }
          }
        }

        // edit enter
        if (message.IsAction (TMessageAction.EditEnter)) {
          Model.EditEnter ();
          ApplyChanges ();
        }

        // edit leave
        if (message.IsAction (TMessageAction.EditLeave)) {
          Model.EditLeave ();
          ApplyChanges ();
        }

        // show service report
        if (message.IsAction (TMessageAction.ReportShow)) {
          Model.ServiceReportShow (message.Support.Argument.Types.ReportData.Message);
          ApplyChanges ();
        }

        // clear service report
        if (message.IsAction (TMessageAction.ReportClear)) {
          Model.ServiceReportClear ();
          ApplyChanges ();
        }

        // Update
        if (message.IsAction (TMessageAction.Update)) {
          NotifyProcess (TCommandComm.Refresh);
        }

        ProcessMessage (message);
      }
    }

    public void SelectAuthentication (TAuthentication authentication)
    {
      Model.Select (authentication);
    }

    public void NotifyProcess (TCommandComm command)
    {
      m_DataComm.Select (command, ProcessName);

      m_Communication.Publish (m_DataComm);
    }
    #endregion

    #region Virtal Members
    public virtual void ProcessMessage (TMessageModule message)
    {
    }

    public virtual void RefreshProcess ()
    {
    }
    #endregion

    #region Event
    void OnCommunicationHandle (object sender, TMessagingEventArgs<TDataComm> e)
    {
      switch (e.Data.Command) {
        case TCommandComm.Refresh:
          if (e.Data.ClientName.NotEquals (ProcessName)) {
            RefreshProcess ();
          }
          break;
      }
    }

    void OnClosing (object sender, CancelEventArgs e)
    {
      NotifyProcess (TCommandComm.Closed);
    }
    #endregion

    #region Dispatcher
    public void ShowErrorBoxDispatcher (TErrorMessage errorMessage)
    {
      Model.ShowErrorBox (errorMessage);

      ApplyChanges ();
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      // restore palette
      var filePath = Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      var iniFileManager = TIniFileManager.CreatDefault;
      iniFileManager.SelectPath (filePath, fileName);

      if (iniFileManager.ValidatePath ().IsValid) {
        if (iniFileManager.ContainsSection (ProcessName)) {
          /* 
           [ModuleSettings]
            ProcessIsAlive=True
            PaletteTheme=light
            PalettePrimary=blue
            PaletteAccent=lime
          */

          var theme = iniFileManager.RequestKey (ProcessName, TProcess.PALETTETHEME);
          bool isDark = theme.Equals (TProcess.PALETTETHEMEDARK, StringComparison.InvariantCulture);
          var palettePrimary = iniFileManager.RequestKey (ProcessName, TProcess.PALETTEPRIMARY);
          var paletteAccent = iniFileManager.RequestKey (ProcessName, TProcess.PALETTEACCENT);

          var primaryColor = Colors.Blue;
          var secondaryColor = Colors.Lime;

          if (string.IsNullOrEmpty (palettePrimary).IsFalse ()) {
            var c = System.Drawing.Color.FromName (palettePrimary);
            primaryColor = Color.FromRgb (c.R, c.G, c.B);
          }

          if (string.IsNullOrEmpty (paletteAccent).IsFalse ()) {
            var c = System.Drawing.Color.FromName (paletteAccent);
            secondaryColor = Color.FromRgb (c.R, c.G, c.B);
          }

          var paletteHelper = new PaletteHelper ();

          ITheme themes = paletteHelper.GetTheme ();
          themes.SetBaseTheme (isDark ? Theme.Dark : Theme.Light);
          themes.SetPrimaryColor (primaryColor);
          themes.SetSecondaryColor (secondaryColor);

          paletteHelper.SetTheme (themes);
        }
      }
    }

    protected override void AllDone ()
    {
      (FrameworkElementView as System.Windows.Window).Closing += OnClosing;
    }
    #endregion

    #region Fields
    readonly TMessagingComm<TDataComm>                          m_Communication;
    readonly TDataComm                                          m_DataComm;
    int                                                         m_ModalCount;
    #endregion
  };
  //---------------------------//

}  // namespace