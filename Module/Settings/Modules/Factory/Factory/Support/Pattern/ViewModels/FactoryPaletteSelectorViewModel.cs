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

    #region Dispatcher
    void RefreshDispatcher ()
    {
      RaiseChanged ();
      RefreshCollection ("ProcessInfoViewSource");
    }

    void IniFileManagerDispatcher ()
    {
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      var iniFileManager = TIniFileManager.CreatDefault;
      iniFileManager.SelectPath (filePath, fileName);

      if (iniFileManager.ValidatePath ().IsValid) {
        if (iniFileManager.ContainsSection (TProcess.PROCESSMODULESSECTION)) {
          var names = iniFileManager.RequestKey (TProcess.PROCESSMODULESSECTION, TProcess.PROCESSNAME);
          var alive = iniFileManager.RequestKey (TProcess.PROCESSMODULESSECTION, TProcess.PROCESSISALIVE);

          Model.Select (names, alive);

          TDispatcher.Invoke (RefreshDispatcher);
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