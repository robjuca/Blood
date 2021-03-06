﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.DataAnnotations;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionContent
  {
    [Key]
    public Guid         Id { get; set; }

    public int          Category { get; set; }
    public string       Contents { get; set; }
  };
  //---------------------------//

}  // namespace