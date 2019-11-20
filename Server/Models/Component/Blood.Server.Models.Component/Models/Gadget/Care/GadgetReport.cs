/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetReport
  {
    public Guid                   Id { get; set; }

    public string                 Name { get; set; }
    public string                 Description { get; set; }
    public DateTime               Date { get; private set; }
    public bool                   Enabled { get; set; }
  }
  //---------------------------//

}  // namespace