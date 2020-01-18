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
    None                = 0,
    Dummy               = 1,

    // gadget Test
    Material            = 200,
    Target              = 210,
    Test                = 220,

    // gadget Care
    Registration        = 400,
    Result              = 410,
    Report              = 420,

    // module
    Settings            = 300,
  };
  //---------------------------//

  //----- TExtensionMask

  // extension mask (bit position) 
  //      7       6         5         4          3         2         1           0         (bit)                            
  // ----------------------------------------------------------------------------------
  // |        |        |   text   |  node   | layout  |  image   | geometry | document |
  // ----------------------------------------------------------------------------------

  // TODO: review
  // gadget Test
  //  Material          = 00100100 binary -> 24 hex -> 36 decimal
  //  Target            = 00110000 binary -> 30 hex -> 48 decimal
  //  Test              = 00110100 binary -> 34 hex -> 52 decimal

  // gadget Care
  //  Registration      = 00100100 binary -> 24 hex -> 36 decimal
  //  Result            = 00100000 binary -> 20 hex -> 32 decimal
  //  Report            = 00000000 binary -> 00 hex -> 00 decimal


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