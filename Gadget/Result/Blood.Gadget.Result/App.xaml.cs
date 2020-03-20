/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
//---------------------------//

namespace Gadget.Result
{
  public partial class TApp : Application
  {
    #region Overrides
    protected override void OnStartup (StartupEventArgs eventArgs)
    {
      if (eventArgs.NotNull ()) {
        if (eventArgs.Args.Length > 0) {
          var key = eventArgs.Args [0];

          if (key.Contains ("Gadget.Result")) {
            rr.Library.Types.TSingleInstance.Make ();

            base.OnStartup (eventArgs);
          }

          else {
            Shutdown ();
          }
        }

        else {
          Shutdown ();
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace