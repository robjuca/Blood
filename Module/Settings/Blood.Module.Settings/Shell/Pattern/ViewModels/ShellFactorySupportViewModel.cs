/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Shared.Message;

using Module.Settings.Shell.Presentation;
using Module.Settings.Shell.Pattern.Models;
//---------------------------//

namespace Module.Settings.Shell.Pattern.ViewModels
{
  [Export ("ModuleShellFactorySupportViewModel", typeof (IShellFactorySupportViewModel))]
  public class TShellFactorySupportViewModel : TViewModelAware<TShellFactorySupportModel>, IHandleNavigateResponse, IShellFactorySupportViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellFactorySupportViewModel (IShellPresentation presentation)
      : base (presentation, new TShellFactorySupportModel ())
    {
      TypeName = GetType ().Name;
    }
    #endregion

    #region IHandle
    public void Handle (TNavigateResponseMessage message)
    {
      if (message.NotNull ()) {
        if (message.IsActionNavigateTo) {
          if (message.IsSender (TNavigateMessage.TSender.Shell)) {
            if (message.IsWhere (TNavigateMessage.TWhere.Support)) {
              ShowViewAnimation ();
            }

            else {
              HideViewAnimation ();
            }
          }
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