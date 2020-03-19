/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

using Shared.Message;
//---------------------------//

namespace Gadget.Test.Shell.Presentation
{
  public class TPresentationCommand : IDelegateCommand
  {
    #region IDelegateCommand Members
    public DelegateCommand<TMessageModule> PublishModuleMessage
    {
      get;
      private set;
    }

    #region Notify
    public DelegateCommand<TNavigateRequestMessage> NotifyNavigateRequestMessage
    {
      get;
      private set;
    }
    #endregion        
    #endregion

    #region Constructor
    public TPresentationCommand (TShellPresentation presentation)
    {
      if (presentation.NotNull ()) {
        PublishModuleMessage = new DelegateCommand<TMessageModule> (presentation.PublishModuleMessageHandler);

        NotifyNavigateRequestMessage = new DelegateCommand<TNavigateRequestMessage> (presentation.NotifyNavigateRequestMessageHandler);
      }
      }
    #endregion

    #region Interface
    public void DoNothing ()
    {
      // do nothing
    }
    #endregion
  }
  //---------------------------//

}  // namespace