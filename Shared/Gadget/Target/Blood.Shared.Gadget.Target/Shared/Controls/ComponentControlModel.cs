/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Models.Component;
using Shared.Gadget.Models.Action;
//---------------------------//

namespace Shared.Gadget.Target
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetTarget ControlModel
    {
      get;
    }

    public GadgetMaterial ChildControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetTarget.CreateDefault;
      ChildControlModel= GadgetMaterial.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TActionComponent component, string propertyName = "")
    {
      if (component.NotNull ()) {
        var gadget = component.Models.GadgetTargetModel;

        if (component.Models.GadgetTargetModel.ValidateId) {
          ControlModel.CopyFrom (gadget);
          ChildControlModel.CopyFrom (component.Models.GadgetMaterialModel);
        }

        // update
        else {
          if (string.IsNullOrEmpty (propertyName).IsFalse ()) {
            switch (propertyName) {
              case "DescriptionProperty" :
                ControlModel.Description = gadget.Description;
                break;

              case "ExternalLinkProperty":
                ControlModel.ExternalLink = gadget.ExternalLink;
                break;

              case "ReferenceProperty":
                ControlModel.Reference = gadget.Reference;
                break;

              case "TextProperty":
                ControlModel.GadgetName = gadget.GadgetName;
                break;

              case "NameProperty":
                ControlModel.GadgetInfo = gadget.GadgetInfo;
                break;

              case "EnabledProperty":
                ControlModel.Enabled = gadget.Enabled;
                break;
            }
          }
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