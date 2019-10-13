/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Gadget.Target
{
  public class TComponentControlModel
  {
    #region Property
    public Server.Models.Component.GadgetTarget ControlModel
    {
      get;
    }

    public Server.Models.Component.GadgetMaterial ChildControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = Server.Models.Component.GadgetTarget.CreateDefault;
      ChildControlModel= Server.Models.Component.GadgetMaterial.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        ControlModel.CopyFrom (action.ModelAction.GadgetTargetModel);
        ChildControlModel.CopyFrom (action.ModelAction.GadgetMaterialModel);

        if (ControlModel.Id.IsEmpty ()) {
          ChildControlModel.Id = (Guid) action.SupportAction.SelectionInfo.Tag;
          ChildControlModel.Material = action.SupportAction.SelectionInfo.Name;
          ChildControlModel.SetImage (action.SupportAction.SelectionInfo.GetImage ());
        }
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace