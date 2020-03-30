/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Server.Models.Infrastructure;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetMaterial : TGadgetBase
  {
    #region Constructor
    public GadgetMaterial ()
      : base (TCategory.Material)
    {
    }

    public GadgetMaterial (GadgetMaterial alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (GadgetMaterial alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);
      }
    }

    public void Change (GadgetMaterial alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);
      }
    }

    public GadgetMaterial Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }
    #endregion

    #region Static
    public static GadgetMaterial CreateDefault => (new GadgetMaterial ());
    #endregion
  };
  //---------------------------//

}  // namespace