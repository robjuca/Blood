/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Registration;
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
    internal void SelectModel (string propertyName, Server.Models.Component.TEntityAction action)
    {
      //TODO: review
      ComponentControlModel.SelectModel (action);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
