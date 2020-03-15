/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Launcher.Shell.Presentation
{
  public class TPresentationCommand : IDelegateCommand
  {
    #region IDelegateCommand Members


    #endregion

    #region Constructor
    public TPresentationCommand (TShellPresentation presentation)
    {
    }

    #region Interface
    public void DoNothing ()
    {
      // dummy
    } 
    #endregion
    #endregion
  }
  //---------------------------//

}  // namespace