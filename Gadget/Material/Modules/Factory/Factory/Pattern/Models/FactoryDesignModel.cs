/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Gadget.Models.Action;

using Shared.Gadget.Material;
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
    internal void SelectModel (TActionComponent component)
    {
      ComponentControlModel.SelectModel (component);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
