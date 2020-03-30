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
  public class GadgetReport : TGadgetBase
  {
    #region Constructor
    public GadgetReport ()
      : base (TCategory.Report)
    {
    }

    public GadgetReport (GadgetReport alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (GadgetReport alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);
      }
    }

    public void Change (GadgetReport alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);
      }
    }

    public GadgetReport Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }
    #endregion

    #region Static
    public static GadgetReport CreateDefault => (new GadgetReport ());
    #endregion
  };
  //---------------------------//

}  // namespace