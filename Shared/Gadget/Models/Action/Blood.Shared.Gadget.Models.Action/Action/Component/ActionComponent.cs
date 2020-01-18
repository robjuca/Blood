/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public class TActionComponent
  {
    #region Property
    public TActionModel Models
    {
      get; 
    }

    public TCategory Category
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TActionComponent (TCategory category)
      : this ()
    {
      Category = category;
    }

    TActionComponent ()
    {
      Models = TActionModel.CreateDefault;

      Category = TCategory.None;
    }
    #endregion

    #region Members
    public bool IsCategory (TCategory category)
    {
      return (Category.Equals (category));
    }

    public void Select (TCategory category)
    {
      Category = category;
    }
    #endregion

    #region Static
    public static TActionComponent Create (TCategory category) => new TActionComponent (category);
    public static TActionComponent CreateDefault => new TActionComponent (); 
    #endregion
  };
  //---------------------------//

}  // namespace
