/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.DataAnnotations;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionText
  {
    [Key]
    public Guid         Id { get; set; }

    public string       Text { get; set; }
    public string       Description { get; set; }
    public string       Reference { get; set; }
    public string       Value { get; set; }
    public string       ExternalLink { get; set; }
    public bool         IsCommit { get; set; }
  }
  //---------------------------//

}  // namespace