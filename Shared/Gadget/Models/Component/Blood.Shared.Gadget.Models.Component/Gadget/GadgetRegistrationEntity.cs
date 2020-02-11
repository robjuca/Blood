/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetRegistration : TGadgetBase
  {
    #region Constructor
    public GadgetRegistration ()
      : base ()
    {
    }

    public GadgetRegistration (GadgetRegistration alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (GadgetRegistration alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);
      }
    }

    public void Change (GadgetRegistration alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);
      }
    }

    public GadgetRegistration Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }
    #endregion

    #region Static
    public static GadgetRegistration CreateDefault => (new GadgetRegistration ());
    #endregion
  };
  //---------------------------//

}  // namespace