/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetRegistration
  {
    public Guid                   Id { get; set; }

    public string                 Name { get; set; }
    public string                 Description { get; set; }
    public string                 ExternalLink { get; set; }
    public Collection<byte>       Image { get; private set; }
    public bool                   Enabled { get; set; }
  }
  //---------------------------//

}  // namespace