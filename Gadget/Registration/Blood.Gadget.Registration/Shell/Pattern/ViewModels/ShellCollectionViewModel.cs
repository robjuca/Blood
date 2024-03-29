﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Shared.Message;

using Gadget.Registration.Shell.Presentation;
using Gadget.Registration.Shell.Pattern.Models;
//---------------------------//

namespace Gadget.Registration.Shell.Pattern.ViewModels
{
  [Export ("ShellCollectionViewModel", typeof (IShellCollectionViewModel))]
  public class TShellCollectionViewModel : TViewModelAware<TShellCollectionModel>, IHandleNavigateResponse, IShellCollectionViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellCollectionViewModel (IShellPresentation presentation)
      : base (presentation, new TShellCollectionModel ())
    {
    }
    #endregion

    #region IHandle
    public void Handle (TNavigateResponseMessage message)
    {
      if (message.NotNull ()) {
        if (message.IsActionNavigateTo) {
          if (message.IsSender (TNavigateMessage.TSender.Shell)) {
            if (message.IsWhere (TNavigateMessage.TWhere.Collection)) {
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