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
  public class GadgetTarget : TGadgetBase
  {
    #region Constructor
    public GadgetTarget ()
      : base (TCategory.Target)
    {
      // Has only one child node (GadgetMaterial)
    }

    public GadgetTarget (GadgetTarget alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);
      }
    }

    public void Change (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);
      }
    }

    public GadgetTarget Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }
    #endregion

    #region Static
    public static GadgetTarget CreateDefault => (new GadgetTarget ());
    #endregion
  };
  //---------------------------//

}  // namespace