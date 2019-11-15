/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Target;
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
      if (action.ModelAction.GadgetMaterialModel.Id.IsEmpty ()) {
        // try selection info
        if (action.SupportAction.SelectionInfo.Tag is Guid id) {
          if (action.ModelAction.GadgetTargetModel.MaterialId.Equals (id)) {
            action.ModelAction.GadgetMaterialModel.Id = id;
            action.ModelAction.GadgetMaterialModel.Material = action.SupportAction.SelectionInfo.Name;
            action.ModelAction.GadgetMaterialModel.SetImage (action.SupportAction.SelectionInfo.GetImage ());
          }
        }
      }

      ComponentControlModel.SelectModel (action);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
