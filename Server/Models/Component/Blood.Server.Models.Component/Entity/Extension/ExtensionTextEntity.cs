/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionText
  {
    #region Constructor
    public ExtensionText ()
    {
      Id = Guid.Empty;
      Text = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      IsCommit = false;
    }

    public ExtensionText (ExtensionText alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ExtensionText alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Text = alias.Text;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        IsCommit = alias.IsCommit;
      }
    }

    public void Change (ExtensionText alias)
    {
      if (alias.NotNull ()) {
        Text = alias.Text;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        IsCommit = alias.IsCommit;
      }
    }
    #endregion

    #region Static
    public static ExtensionText CreateDefault => (new ExtensionText ());
    #endregion
  };
  //---------------------------//

}  // namespace