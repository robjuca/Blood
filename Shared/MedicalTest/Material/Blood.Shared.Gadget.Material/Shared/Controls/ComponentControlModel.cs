﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Gadget.Material
{
  public class TComponentControlModel
  {
    #region Property
    public Server.Models.Gadget.GadgetMaterial ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = Server.Models.Gadget.GadgetMaterial.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (Server.Models.Action.TEntityAction action)
    {
      if (action.NotNull ()) {
        //ControlModel.CopyFrom (action.ModelAction.GadgetMaterialModel);
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace