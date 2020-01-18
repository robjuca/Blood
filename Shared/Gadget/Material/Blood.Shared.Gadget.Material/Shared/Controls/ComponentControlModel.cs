/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Material
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetMaterial ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetMaterial.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TActionComponent component)
    {
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Material)) {
          ControlModel.CopyFrom (component.Models.GadgetMaterialModel);
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