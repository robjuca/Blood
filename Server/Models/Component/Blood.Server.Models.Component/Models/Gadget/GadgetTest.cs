﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetTest
  {
    public Guid                   Id { get; set; }

    public string                 Test { get; set; }
    public string                 Description { get; set; }
    public string                 ExternalLink { get; set; }
    public bool                   Enabled { get; set; }
    public int                    RelationCategory { get; private set; }
    public Collection<Guid>       Targets { get; private set; }
  }
  //---------------------------//

}  // namespace