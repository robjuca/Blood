/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Gadget.Result
{
  public class TComponentControlModel
  {
    #region Property
    public Server.Models.Component.GadgetResult ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = Server.Models.Component.GadgetResult.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        ControlModel.CopyFrom (action.ModelAction.GadgetResultModel);
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace