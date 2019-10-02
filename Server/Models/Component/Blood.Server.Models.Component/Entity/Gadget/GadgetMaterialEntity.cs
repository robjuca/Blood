/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetMaterial
  {
    #region Constructor
    public GadgetMaterial ()
    {
      Id = Guid.Empty;

      Material = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      Image = null;
      Enabled = false;
    }

    public GadgetMaterial (GadgetMaterial alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (TEntityAction action)
    {
      if (action.NotNull ()) {
        Id = action.ModelAction.ComponentInfoModel.Id;
        Material = action.ModelAction.ExtensionTextModel.Text;
        Description = action.ModelAction.ExtensionTextModel.Description;
        ExternalLink = action.ModelAction.ExtensionTextModel.ExternalLink;
        Image = action.ModelAction.ExtensionImageModel.Image;
        Enabled = action.ModelAction.ComponentInfoModel.Enabled;
      }
    }

    public void CopyFrom (GadgetMaterial alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Image = alias.Image;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetMaterial alias)
    {
      if (alias.NotNull ()) {
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Image = alias.Image;
        Enabled = alias.Enabled;
      }
    }

    public GadgetMaterial Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }
    #endregion

    #region Static
    public static GadgetMaterial CreateDefault => (new GadgetMaterial ());
    #endregion
  };
  //---------------------------//

}  // namespace