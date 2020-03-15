/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

using Shared.Message;
using Shared.ViewModel;
//---------------------------//

namespace Module.Settings.Factory.Database.Presentation
{
  public class TPresentationCommand : IDelegateCommand
  {
    #region IDelegateCommand Members
    public DelegateCommand<TMessageModule> PublishMessage
    {
      get;
      private set;
    }

    public DelegateCommand<TMessageInternal> PublishInternalMessage
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TPresentationCommand (TFactoryDatabasePresentation presentation)
    {
      if (presentation.NotNull ()) {
        PublishMessage = new DelegateCommand<TMessageModule> (presentation.PublishMessageHandler);
        PublishInternalMessage = new DelegateCommand<TMessageInternal> (presentation.PublishInternalMessageHandler);
      }
    }

    #region Interface
    public void DoNothing ()
    {
      // do nothing
    } 
    #endregion
    #endregion
  };
  //---------------------------//

}  // namespace