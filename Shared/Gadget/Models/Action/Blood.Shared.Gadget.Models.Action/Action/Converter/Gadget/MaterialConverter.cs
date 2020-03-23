/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public static class TGadgetMaterialConverter
  {
    #region Static Members
    public static void Collection (Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull ()) {
        gadgets.Clear ();

        if (entityAction.NotNull()) {
          if (entityAction.CategoryType.IsCategory (TCategory.Material)) {
            var gadgetCollection = new Collection<GadgetMaterial> ();

            foreach (var item in entityAction.CollectionAction.ModelCollection) {
              var modelAction = item.Value;
              var gadget = GadgetMaterial.CreateDefault;

              gadget.Id = modelAction.ComponentInfoModel.Id;
              gadget.GadgetName = modelAction.ExtensionTextModel.Text;
              gadget.Description = modelAction.ExtensionTextModel.Description;
              gadget.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
              gadget.SetImage (modelAction.ExtensionImageModel.Image);
              gadget.Enabled = modelAction.ComponentInfoModel.Enabled;

              gadget.GadgetInfo = modelAction.ComponentInfoModel.Name;
              gadget.Busy = modelAction.ComponentStatusModel.Busy;
              gadget.Material = modelAction.ExtensionTextModel.Text;

              gadgetCollection.Add (gadget);
            }

            // sort
            var list = gadgetCollection
              .OrderBy (p => p.GadgetInfo)
              .ToList ()
            ;

            foreach (var model in list) {
              var component = TActionComponent.Create (TCategory.Material);
              component.Models.GadgetMaterialModel.CopyFrom (model);

              gadgets.Add (component);
            }
          }
        }
      }
    }

    public static void Select (TActionComponent component, TEntityAction entityAction)
    {
      if (entityAction.NotNull()) {
        if (entityAction.CategoryType.IsCategory (TCategory.Material)) {
          if (component.NotNull()) {
            component.Select (TCategory.Material);

            component.Models.GadgetMaterialModel.Id = entityAction.ModelAction.ComponentInfoModel.Id;
            component.Models.GadgetMaterialModel.GadgetName = entityAction.ModelAction.ExtensionTextModel.Text;
            component.Models.GadgetMaterialModel.Description = entityAction.ModelAction.ExtensionTextModel.Description;
            component.Models.GadgetMaterialModel.ExternalLink = entityAction.ModelAction.ExtensionTextModel.ExternalLink;
            component.Models.GadgetMaterialModel.SetImage (entityAction.ModelAction.ExtensionImageModel.Image);
            component.Models.GadgetMaterialModel.Enabled = entityAction.ModelAction.ComponentInfoModel.Enabled;

            component.Models.GadgetMaterialModel.GadgetInfo = entityAction.ModelAction.ComponentInfoModel.Name;
            component.Models.GadgetMaterialModel.Busy = entityAction.ModelAction.ComponentStatusModel.Busy;
            component.Models.GadgetMaterialModel.Material = entityAction.ModelAction.ExtensionTextModel.Text;
          }
        }
      }
    }

    public static void Request (TActionComponent component, TEntityAction entityAction)
    {
      if (component.NotNull()) {
        if (component.IsCategory (TCategory.Material)) {
          if (entityAction != null) {
            entityAction.Id = component.Models.GadgetMaterialModel.Id;
            entityAction.CategoryType.Select (TCategory.Material);

            entityAction.ModelAction.ComponentInfoModel.Name = component.Models.GadgetMaterialModel.GadgetInfo;
            entityAction.ModelAction.ComponentStatusModel.Busy = component.Models.GadgetMaterialModel.Busy;

            entityAction.ModelAction.ComponentInfoModel.Id = component.Models.GadgetMaterialModel.Id;
            entityAction.ModelAction.ExtensionTextModel.Id = component.Models.GadgetMaterialModel.Id;
            entityAction.ModelAction.ExtensionTextModel.Text = component.Models.GadgetMaterialModel.GadgetName;
            entityAction.ModelAction.ExtensionTextModel.Description = component.Models.GadgetMaterialModel.Description;
            entityAction.ModelAction.ExtensionTextModel.ExternalLink = component.Models.GadgetMaterialModel.ExternalLink;
            entityAction.ModelAction.ComponentInfoModel.Enabled = component.Models.GadgetMaterialModel.Enabled;
            entityAction.ModelAction.ExtensionImageModel.Image = component.Models.GadgetMaterialModel.GetImage ();
          }
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
