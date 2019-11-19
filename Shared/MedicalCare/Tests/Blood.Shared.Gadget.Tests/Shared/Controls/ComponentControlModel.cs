/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Gadget.Tests
{
  public class TComponentControlModel
  {
    #region Property
    public Server.Models.Component.GadgetTests ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = Server.Models.Component.GadgetTests.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        ControlModel.CopyFrom (action.ModelAction.GadgetTestsModel);
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace