/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Models.Component;
using Shared.Gadget.Models.Action;
//---------------------------//

namespace Shared.Gadget.Result
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetResult ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetResult.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TActionComponent component)
    {
      if (component.NotNull ()) {
        ControlModel.CopyFrom (component.Models.GadgetResultModel);
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace