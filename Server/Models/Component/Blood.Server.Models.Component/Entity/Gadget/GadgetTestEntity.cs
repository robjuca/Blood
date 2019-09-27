/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetTest
  {
    #region Constructor
    public GadgetTest ()
    {
      Id = Guid.Empty;
      Enabled = false;
    }

    public GadgetTest (GadgetTest alias)
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

    public void CopyFrom (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Enabled = alias.Enabled;
      }
    }
    #endregion

    #region Static
    public static GadgetTest CreateDefault => (new GadgetTest ());
    #endregion
  };
  //---------------------------//

}  // namespace