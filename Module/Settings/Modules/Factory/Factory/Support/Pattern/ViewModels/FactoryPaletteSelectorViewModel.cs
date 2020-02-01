/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.Message;

using Module.Settings.Factory.Support.Presentation;
using Module.Settings.Factory.Support.Pattern.Models;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.ViewModels
{
  [Export ("ModuleSettingsFactorySupportPaletteSelectorViewModel", typeof (IFactoryPaletteSelectorViewModel))]
  public class TFactoryPaletteSelectorViewModel : TViewModelAware<TFactoryPaletteSelectorModel>, IHandleMessageModule, IFactoryPaletteSelectorViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryPaletteSelectorViewModel (IFactorySupportPresentation presentation)
      : base (new TFactoryPaletteSelectorModel ())
    {
      TypeName = GetType ().Name;

      presentation.ViewModel = this;
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        if (message.IsAction (TMessageAction.DatabaseValidated)) {
          TDispatcher.Invoke (IniFileManagerDispatcher);
        }
      }
    }
    #endregion

    #region Event
    public void OnProcessChecked (object obj)
    {
      if (obj is TProcessInfo info) {
        TDispatcher.BeginInvoke (ProcessSelectedDispatcher, info);
      }
    }

    public void OnApplyCommadClicked ()
    {
      TDispatcher.Invoke (ApplyPaletteDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshDispatcher ()
    {
      RaiseChanged ();
      RefreshCollection ("ProcessInfoViewSource");
    }

    void IniFileManagerDispatcher ()
    {
      Model.CleanupProcess ();

      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      var iniFileManager = TIniFileManager.CreatDefault;
      iniFileManager.SelectPath (filePath, fileName);

      if (iniFileManager.ValidatePath ().IsValid) {
        // Process Module section
        if (iniFileManager.ContainsSection (TProcess.PROCESSMODULESSECTION)) {
          // Process Names section
          if (iniFileManager.ContainsSection (TProcess.PROCESSNAMESSECTION)) {
            // [ProcessNamesSection]
            // ProcessNames=ModuleSettings? GadgetMaterial?GadgetTarget? GadgetTest?GadgetRegistration? GadgetTests?GadgetReport                   
            
            var processNames = iniFileManager.RequestKey (TProcess.PROCESSNAMESSECTION, TProcess.PROCESSNAMES);
            var allProcessNames = processNames.Split ('?');

            for (int i = 0; i < allProcessNames.Length; i++) {
              var processNameSection = allProcessNames [i];

              if (iniFileManager.ContainsSection (processNameSection)) {
                /* 
                 [ModuleSettings]
                  ProcessIsAlive=True
                  PaletteTheme=light
                  PalettePrimary=blue
                  PaletteAccent=lime
                */
                
                var isAlive = iniFileManager.RequestKey (processNameSection, TProcess.PROCESSISALIVE);

                var baseTheme = iniFileManager.RequestKey (processNameSection, TProcess.PALETTETHEME);
                var primaryColor = iniFileManager.RequestKey (processNameSection, TProcess.PALETTEPRIMARY);
                var accentColor = iniFileManager.RequestKey (processNameSection, TProcess.PALETTEACCENT);

                var paletteInfo = TPaletteInfo.Create(baseTheme, primaryColor, accentColor);

                Model.AddProcessInfo (processNameSection, bool.Parse (isAlive), paletteInfo);
              }
            }

            if (Model.SelectProcess ()) {
              TDispatcher.BeginInvoke (ProcessSelectedDispatcher, Model.CurrentProcess);
            }
          }
        }
      }

      TDispatcher.Invoke (RefreshDispatcher);
    }

    void ProcessSelectedDispatcher (TProcessInfo processInfo)
    {
      Model.ProcessSelected (processInfo);

      TDispatcher.Invoke (RefreshDispatcher);
    }

    void ApplyPaletteDispatcher ()
    {
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      var iniFileManager = TIniFileManager.CreatDefault;
      iniFileManager.SelectPath (filePath, fileName);

      if (iniFileManager.ValidatePath ().IsValid) {
        var processNameSection = Model.CurrentProcessName;

        if (iniFileManager.ContainsSection (processNameSection)) {
          /* 
           [ModuleSettings]
            ProcessIsAlive=True
            PaletteTheme=light
            PalettePrimary=blue
            PaletteAccent=lime
          */

          iniFileManager.ChangeKey (processNameSection, TProcess.PALETTETHEME, Model.CurrentProcess.PaletteInfo.BaseTheme);
          iniFileManager.ChangeKey (processNameSection, TProcess.PALETTEPRIMARY, Model.CurrentProcess.PaletteInfo.PalettePrimary);
          iniFileManager.ChangeKey (processNameSection, TProcess.PALETTEACCENT, Model.CurrentProcess.PaletteInfo.PaletteAccent);

          iniFileManager.SaveChanges ();
        }
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
  };
  //---------------------------//

}  // namespace