/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class Settings
  {
    #region Constructor
    public Settings ()
    {
      MyName = "robjuca";
    }

    public Settings (Settings alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (Settings alias)
    {
      if (alias.NotNull ()) {
        MyName = alias.MyName;
      }
    }

    public void Change (Settings alias)
    {
      if (alias.NotNull ()) {
      }
    }
    #endregion

    #region Static
    public static Settings CreateDefault => (new Settings ());
    #endregion
  };
  //---------------------------//

}  // namespace