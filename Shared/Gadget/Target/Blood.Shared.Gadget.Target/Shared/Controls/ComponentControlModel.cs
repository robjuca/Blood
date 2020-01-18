/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Models.Component;
using Shared.Gadget.Models.Action;
//---------------------------//

namespace Shared.Gadget.Target
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetTarget ControlModel
    {
      get;
    }

    public GadgetMaterial ChildControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetTarget.CreateDefault;
      ChildControlModel= GadgetMaterial.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TGadgetTargetModel model)
    {
      if (model.NotNull ()) {
        ControlModel.CopyFrom (model.Model);
        ChildControlModel.CopyFrom (model.MaterialModel.Model);
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace