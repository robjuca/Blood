/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetTarget
  {
    #region Constructor
    public GadgetTarget ()
    {
      Id = Guid.Empty;
      Enabled = false;
    }

    public GadgetTarget (GadgetTarget alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (TEntityAction action)
    {
      if (action.NotNull ()) {
        Id = action.ModelAction.ComponentInfoModel.Id;
        Enabled = action.ModelAction.ComponentInfoModel.Enabled;
      }
    }

    public void CopyFrom (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        Enabled = alias.Enabled;
      }
    }
    #endregion

    #region Static
    public static GadgetTarget CreateDefault => (new GadgetTarget ());
    #endregion
  };
  //---------------------------//

}  // namespace