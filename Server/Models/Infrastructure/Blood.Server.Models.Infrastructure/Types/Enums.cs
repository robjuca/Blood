/*----------------------------------------------------------------
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
  //  Material      = 00100100 binary -> 24 hex -> 36 decimal
  //  Target        = 00000000 binary -> 00 hex -> 00 decimal
  //  Test          = 00000000 binary -> 00 hex -> 00 decimal


  // TODO: review
  [Flags]
  public enum TComponentExtensionName : short
  {
    None        = 0,
    Document    = 1,
    Geometry    = 2,
    Image       = 4,
    Layout      = 8,
    Node        = 16,
    Text        = 32,
  };
  //---------------------------//

}  // namespace