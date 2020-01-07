/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Action;
using Server.Models.Infrastructure;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public static class TGadgetMaterialActionComponent
  {
    #region Static Members
    public static void Select (Collection<TGadgetMaterialModel> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull () && entityAction.NotNull ()) {
        gadgets.Clear ();

        if (entityAction.CategoryType.IsCategory (TCategory.Material)) {
          foreach (var item in entityAction.CollectionAction.ModelCollection) {
            var modelAction = item.Value;
            var gadget = TGadgetMaterialModel.Create (modelAction.ComponentInfoModel.Name, modelAction.ComponentStatusModel.Busy);

            gadget.Model.Id = modelAction.ComponentInfoModel.Id;
            gadget.Model.Material = modelAction.ExtensionTextModel.Text;
            gadget.Model.Description = modelAction.ExtensionTextModel.Description;
            gadget.Model.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
            gadget.Model.SetImage (modelAction.ExtensionImageModel.Image);
            gadget.Model.Enabled = modelAction.ComponentInfoModel.Enabled;

            gadgets.Add (gadget);
          }

          var list = new Collection<TGadgetMaterialModel> (gadgets.OrderBy (p => p.Name).ToList ());

          gadgets.Clear ();

          foreach (var item in list) {
            gadgets.Add (item);
          }
        }
      }
    }

    public static void Select (TGadgetMaterialModel gadget, TEntityAction entityAction)
    {
      if (gadget.NotNull () && entityAction.NotNull ()) {
        if (entityAction.CategoryType.IsCategory (TCategory.Material)) {
          var someGadget = TGadgetMaterialModel.Create (entityAction.ModelAction.ComponentInfoModel.Name, entityAction.ModelAction.ComponentStatusModel.Busy);

          someGadget.Model.Id = entityAction.ModelAction.ComponentInfoModel.Id;
          someGadget.Model.Material = entityAction.ModelAction.ExtensionTextModel.Text;
          someGadget.Model.Description = entityAction.ModelAction.ExtensionTextModel.Description;
          someGadget.Model.ExternalLink = entityAction.ModelAction.ExtensionTextModel.ExternalLink;
          someGadget.Model.SetImage (entityAction.ModelAction.ExtensionImageModel.Image);
          someGadget.Model.Enabled = entityAction.ModelAction.ComponentInfoModel.Enabled;

          gadget.CopyFrom (someGadget);
        }
      }
    }

    public static void Request (TGadgetMaterialModel gadget, TEntityAction entityAction)
    {
      if (gadget.NotNull () && entityAction.NotNull ()) {
        entityAction.Id = gadget.Model.Id;
        entityAction.CategoryType.Select (TCategory.Material);

        entityAction.ModelAction.ComponentInfoModel.Name = gadget.Name;
        entityAction.ModelAction.ComponentStatusModel.Busy = gadget.Busy;

        entityAction.ModelAction.ComponentInfoModel.Id = gadget.Model.Id;
        entityAction.ModelAction.ExtensionTextModel.Text = gadget.Model.Material;
        entityAction.ModelAction.ExtensionTextModel.Description = gadget.Model.Description;
        entityAction.ModelAction.ExtensionTextModel.ExternalLink = gadget.Model.ExternalLink;
        entityAction.ModelAction.ComponentInfoModel.Enabled = gadget.Model.Enabled;
        entityAction.ModelAction.ExtensionImageModel.Image = gadget.Model.GetImage ();
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace