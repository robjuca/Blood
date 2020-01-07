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
        //ControlModel.CopyFrom (action.ModelAction.GadgetTargetModel);
        //ChildControlModel.CopyFrom (action.ModelAction.GadgetMaterialModel);

        //if (ControlModel.Id.IsEmpty ()) {
        //  if (action.SupportAction.SelectionInfo.Tag is Guid id) {
        //    ChildControlModel.Id = id;
        //    ChildControlModel.Material = action.SupportAction.SelectionInfo.Name;
        //    ChildControlModel.SetImage (action.SupportAction.SelectionInfo.GetImage ());
        //  }
        //}
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace