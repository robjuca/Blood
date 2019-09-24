﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Infrastructure
{
  //-----TOperation
  public enum TOperation
  {
    None,
    Change,
    Collection,
    Insert,
    Remove,
    Select,
    Validate,
  };
  //---------------------------//

  //-----TExtension
  public enum TExtension
  {
    None,
    Active,
    ById,
    Full,
    Idle,
    Many,
    Minimum,
    Node,
    Relation,
    Settings,
    Summary,
    Zap,
  };
  //---------------------------//

  //----- TCategory
  public enum TCategory
  {
    None        = 0,

    // gadget
    Material    = 200,
    Target      = 210,
    Test        = 220,

    Settings    = 300,
  };
  //---------------------------//

  //----- TExtensionMask

  // extension mask (bit position) 
  //      7       6         5         4          3         2         1           0         (bit)                            
  // ----------------------------------------------------------------------------------
  // |        |        |   text   |  node   | layout  |  image   | geometry | document |
  // ----------------------------------------------------------------------------------

  // TODO: review
  //  Document = 00011111 binary -> 1F hex -> 31 decimal
  //  Image    = 00111100 binary -> 3C hex -> 60 decimal
  //  Bag      = 00011000 binary -> 18 hex -> 24 decimal
  //  Shelf    = 00001010 binary -> 0A hex -> 10 decimal
  //  Drawer   = 00101010 binary -> 2A hex -> 42 decimal
  //  Chest    = 00000000 binary -> 40 hex -> 00 decimal


  // TODO: review
  [Flags]
  public enum TComponentExtensionName : short
  {
    None        = 0,
    Document    = 1,
    Geometry    = 2,
    Target      = 4,
    Layout        = 8,
    Node        = 16,
    Image       = 32,
    Text        = 64,
  };
  //---------------------------//

}  // namespace