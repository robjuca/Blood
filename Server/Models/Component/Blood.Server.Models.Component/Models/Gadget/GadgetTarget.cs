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
    public Guid         Id { get; set; }
    public Guid         MaterialId { get; set; }

    public string       Target { get; set; }
    public string       Description { get; set; }
    public string       ExternalLink { get; set; }
    public bool         Enabled { get; set; }
  }
  //---------------------------//

}  // namespace