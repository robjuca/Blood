/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Models.Action;
//---------------------------//

namespace Shared.Gadget.Material
{
  public class TComponentControlModel
  {
    #region Property
    public TGadgetMaterialModel ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = TGadgetMaterialModel.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TGadgetMaterialModel model)
    {
      if (model.NotNull ()) {
        ControlModel.CopyFrom (model);
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace