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

namespace Shared.Gadget.Registration
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetRegistration ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetRegistration.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TActionComponent component)
    {
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Registration)) {
          ControlModel.CopyFrom (component.Models.GadgetRegistrationModel);
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