/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionContent
  {
    #region Constructor
    public ExtensionContent ()
    {
      Id = Guid.Empty;

      Category = 0;
      Contents = string.Empty;
    }

    public ExtensionContent (ExtensionContent alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ExtensionContent alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;

        Category = alias.Category;
        Contents = alias.Contents;
      }
    }

    public void Change (ExtensionContent alias)
    {
      if (alias.NotNull ()) {
        Category = alias.Category;
        Contents = alias.Contents;
      }
    }
    #endregion

    #region Static
    public static ExtensionContent CreateDefault => new ExtensionContent ();
    #endregion
  };
  //---------------------------//

}  // namespace