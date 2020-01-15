/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Server.Models.Action;

using Shared.Gadget.Models.Action;

using Shared.Gadget.Test;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryDesignModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TFactoryDesignModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void SelectModel (TEntityAction action)
    {
      //ComponentControlModel.SelectModel (action);
    }

    internal void AddModel (TGadgetTestComponent gadgetComponent) 
    {
      gadgetComponent.ThrowNull ();

      ComponentControlModel.AddComponent (gadgetComponent);
    }

    internal void RemoveModel (TGadgetTestComponent gadgetComponent)
    {
      gadgetComponent.ThrowNull ();

      ComponentControlModel.RemoveComponent (gadgetComponent);
    }

    internal void Cleanup ()
    {
      ComponentControlModel.Cleanup ();
    }

    internal void Edit (TGadgetTestModel gadget)
    {
      gadget.ThrowNull ();

      ComponentControlModel.SelectModel (gadget);
    }
    #endregion
  };
  //---------------------------//


}  // namespace
