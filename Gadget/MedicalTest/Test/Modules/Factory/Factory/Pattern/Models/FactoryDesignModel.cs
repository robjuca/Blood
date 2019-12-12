/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.ViewModel;

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
    internal void SelectModel (Server.Models.Component.TEntityAction action)
    {
      ComponentControlModel.SelectModel (action);
    }

    internal void AddModel (TComponentModelItem item) 
    {
      item.ThrowNull ();

      ComponentControlModel.AddComponent (item);
    }

    internal void RemoveModel (TComponentModelItem item)
    {
      item.ThrowNull ();

      ComponentControlModel.RemoveComponent (item);
    }

    internal void Cleanup ()
    {
      ComponentControlModel.Cleanup ();
    }
    #endregion
  };
  //---------------------------//


}  // namespace
