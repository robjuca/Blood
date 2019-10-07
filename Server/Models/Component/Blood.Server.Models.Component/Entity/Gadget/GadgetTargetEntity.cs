/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetTarget
  {
    #region Constructor
    public GadgetTarget ()
    {
      Id = Guid.Empty;
      MaterialId = Guid.Empty;

      Target = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      Enabled = false;
    }

    public GadgetTarget (GadgetTarget alias)
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
        MaterialId = action.ModelAction.ExtensionNodeModel.ChildId;

        Target = action.ModelAction.ExtensionTextModel.Text;
        Description = action.ModelAction.ExtensionTextModel.Description;
        ExternalLink = action.ModelAction.ExtensionTextModel.ExternalLink;
        Enabled = action.ModelAction.ComponentInfoModel.Enabled;
      }
    }

    public void CopyFrom (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        MaterialId = alias.MaterialId;

        Target = alias.Target;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        Target = alias.Target;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;
      }
    }

    public GadgetTarget Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }
    #endregion

    #region Static
    public static GadgetTarget CreateDefault => (new GadgetTarget ());
    #endregion
  };
  //---------------------------//

}  // namespace