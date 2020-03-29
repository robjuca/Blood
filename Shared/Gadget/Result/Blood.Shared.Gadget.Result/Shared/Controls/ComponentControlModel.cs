/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using Shared.Gadget.Models.Component;
using Shared.Gadget.Models.Action;
//---------------------------//

namespace Shared.Gadget.Result
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetResult ControlModel
    {
      get;
    }

    public Dictionary<Guid, GadgetMaterial> MaterialDictionary
    {
      get; 
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetResult.CreateDefault;

      MaterialDictionary = new Dictionary<Guid, GadgetMaterial> ();
    }
    #endregion

    #region Members
    public void Select (TActionComponent component, Dictionary<Guid, GadgetMaterial> materialDictionary)
    {
      if (component.NotNull ()) {
        ControlModel.CopyFrom (component.Models.GadgetResultModel);
      }

      if (materialDictionary.NotNull ()) {
        MaterialDictionary.Clear ();

        foreach (var item in materialDictionary) {
          MaterialDictionary.Add (item.Key, item.Value);
        }
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace