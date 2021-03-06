﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Server.Models.Action;
using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;

using Shared.Gadget.Test;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryDesignModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TFactoryDesignModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void SelectModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
        var component = TActionComponent.Create (TCategory.Test);
        TActionConverter.Select (TCategory.Test, component, entityAction);

        ComponentControlModel.SelectModel (component);
      }
    }

    internal void AddModel (TActionComponent component) 
    {
      component.ThrowNull ();

      ComponentControlModel.AddComponent (component);
    }

    internal void RemoveModel (TActionComponent component)
    {
      component.ThrowNull ();

      ComponentControlModel.RemoveComponent (component);
    }

    internal void Cleanup ()
    {
      ComponentControlModel.Cleanup ();
    }

    internal void Edit (TActionComponent component)
    {
      component.ThrowNull ();

      if (component.IsCategory (TCategory.Test)) {
        ComponentControlModel.SelectModel (component);
      }
    }
    #endregion
  };
  //---------------------------//


}  // namespace
