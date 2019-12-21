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
      Reference = string.Empty;
      Extension = string.Empty;
      Value = string.Empty;
      ExternalLink = string.Empty;
      IsCommit = false;
      Date = DateTime.Now;
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
        Reference = alias.Reference;
        Extension = alias.Extension;
        Value = alias.Value;
        ExternalLink = alias.ExternalLink;
        IsCommit = alias.IsCommit;
        Date = alias.Date.IsNull () ? DateTime.Now : alias.Date;
      }
    }

    public void Change (ExtensionText alias)
    {
      if (alias.NotNull ()) {
        Text = alias.Text;
        Description = alias.Description;
        Reference = alias.Reference;
        Extension = alias.Extension;
        Value = alias.Value;
        ExternalLink = alias.ExternalLink;
        IsCommit = alias.IsCommit;
        Date = alias.Date.IsNull () ? DateTime.Now : alias.Date;
      }
    }
    #endregion

    #region Static
    public static ExtensionText CreateDefault => (new ExtensionText ());
    #endregion
  };
  //---------------------------//

}  // namespace