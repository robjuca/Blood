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

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public static class TGadgetTargetConverter
  {
    #region Static Members
    public static void Collection (Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull ()) {
        gadgets.Clear ();

        if (entityAction.NotNull ()) {
          if (entityAction.CategoryType.IsCategory (TCategory.Target)) {
            var gadgetCollection = new Collection<GadgetTarget> ();

            foreach (var item in entityAction.CollectionAction.ModelCollection) {
              var modelAction = item.Value;
              var gadget = GadgetTarget.CreateDefault;

              gadget.Id = modelAction.ComponentInfoModel.Id;
              gadget.MaterialId = modelAction.ExtensionNodeModel.ParentId;
              gadget.GadgetName = modelAction.ExtensionTextModel.Text;
              gadget.Description = modelAction.ExtensionTextModel.Description;
              gadget.Reference = modelAction.ExtensionTextModel.Reference;
              gadget.Value = modelAction.ExtensionTextModel.Value;
              gadget.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
              gadget.Enabled = modelAction.ComponentInfoModel.Enabled;

              gadget.GadgetInfo = modelAction.ComponentInfoModel.Name;
              gadget.Busy = modelAction.ComponentStatusModel.Busy;

              //  // Has only one child node (GadgetMaterial)
              foreach (var node in entityAction.CollectionAction.ExtensionNodeCollection) {
                // gadget Target must be child here
                if (gadget.Contains (node.ChildId)) {
                  modelAction.ExtensionNodeModel.ChildId = node.ChildId;
                  modelAction.ExtensionNodeModel.ChildCategory = node.ChildCategory;
                  modelAction.ExtensionNodeModel.ParentId = node.ParentId;
                  modelAction.ExtensionNodeModel.ParentCategory = node.ParentCategory;

                  gadget.MaterialId = gadget.MaterialId.IsEmpty () ? node.ParentId : gadget.MaterialId;  // must be child

                  break;
                }
              }

              gadgetCollection.Add (gadget);
            }

            // sort
            var list = gadgetCollection
              .OrderBy (p => p.GadgetInfo)
              .ToList ()
            ;

            foreach (var model in list) {
              var component = TActionComponent.Create (TCategory.Target);
              component.Models.GadgetTargetModel.CopyFrom (model);

              gadgets.Add (component);
            }
          }
        }
      }
    }

    public static void Select (TActionComponent component, TEntityAction entityAction)
    {
      if (entityAction.NotNull ()) {
        if (entityAction.CategoryType.IsCategory (TCategory.Target)) {
          if (component.NotNull ()) {
            component.Select (TCategory.Target);

            component.Models.GadgetTargetModel.Id = entityAction.ModelAction.ComponentInfoModel.Id;
            component.Models.GadgetTargetModel.MaterialId = entityAction.ModelAction.ExtensionNodeModel.ParentId;
            component.Models.GadgetTargetModel.GadgetName = entityAction.ModelAction.ExtensionTextModel.Text;
            component.Models.GadgetTargetModel.Description = entityAction.ModelAction.ExtensionTextModel.Description;
            component.Models.GadgetTargetModel.Reference = entityAction.ModelAction.ExtensionTextModel.Reference;
            component.Models.GadgetTargetModel.Value = entityAction.ModelAction.ExtensionTextModel.Value;
            component.Models.GadgetTargetModel.ExternalLink = entityAction.ModelAction.ExtensionTextModel.ExternalLink;
            component.Models.GadgetTargetModel.Enabled = entityAction.ModelAction.ComponentInfoModel.Enabled;

            component.Models.GadgetTargetModel.GadgetInfo = entityAction.ModelAction.ComponentInfoModel.Name;
            component.Models.GadgetTargetModel.Busy = entityAction.ModelAction.ComponentStatusModel.Busy;

            //  // Has only one child node (GadgetMaterial)
            foreach (var node in entityAction.CollectionAction.ExtensionNodeCollection) {
              // gadget Target must be child here
              if (component.Models.GadgetTargetModel.Contains (node.ChildId)) {
                entityAction.ModelAction.ExtensionNodeModel.ChildId = node.ChildId;
                entityAction.ModelAction.ExtensionNodeModel.ChildCategory = node.ChildCategory;
                entityAction.ModelAction.ExtensionNodeModel.ParentId = node.ParentId;
                entityAction.ModelAction.ExtensionNodeModel.ParentCategory = node.ParentCategory;

                component.Models.GadgetTargetModel.MaterialId = component.Models.GadgetTargetModel.MaterialId.IsEmpty () ? node.ParentId : component.Models.GadgetTargetModel.MaterialId;  // must be child

                break;
              }
            }

            // update Material
            var materialId = component.Models.GadgetTargetModel.MaterialId;

            if (entityAction.CollectionAction.ModelCollection.ContainsKey (materialId)) {
              var action = TEntityAction.Create (TCategory.Material);
              action.ModelAction.CopyFrom (entityAction.CollectionAction.ModelCollection [materialId]);

              var componentMaterial = TActionComponent.Create (TCategory.Material);
              TActionConverter.Select (TCategory.Material, componentMaterial, action);

              var gadgetMaterial = componentMaterial.Models.GadgetMaterialModel;

              component.Models.GadgetMaterialModel.CopyFrom (gadgetMaterial);
              component.Models.GadgetTargetModel.Material = gadgetMaterial.Material;
            }
          }
        }
      }
    }

    public static void Request (TActionComponent component, TEntityAction entityAction)
    {
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Target)) {
          if (entityAction.NotNull ()) {
            entityAction.Id = component.Models.GadgetTargetModel.Id;
            entityAction.CategoryType.Select (TCategory.Target);

            entityAction.ModelAction.ComponentInfoModel.Name = component.Models.GadgetTargetModel.GadgetInfo;
            entityAction.ModelAction.ComponentStatusModel.Busy = component.Models.GadgetTargetModel.Busy;

            entityAction.ModelAction.ComponentInfoModel.Id = component.Models.GadgetTargetModel.Id;
            entityAction.ModelAction.ExtensionNodeModel.ChildId = component.Models.GadgetTargetModel.Id;
            entityAction.ModelAction.ExtensionNodeModel.ChildCategory = TCategoryType.ToValue (TCategory.Target);
            entityAction.ModelAction.ExtensionNodeModel.ParentId = component.Models.GadgetTargetModel.MaterialId;
            entityAction.ModelAction.ExtensionNodeModel.ParentCategory = TCategoryType.ToValue (TCategory.Material);
            entityAction.ModelAction.ExtensionTextModel.Id = component.Models.GadgetTargetModel.Id;
            entityAction.ModelAction.ExtensionTextModel.Text = component.Models.GadgetTargetModel.GadgetName;
            entityAction.ModelAction.ExtensionTextModel.Description = component.Models.GadgetTargetModel.Description;
            entityAction.ModelAction.ExtensionTextModel.Reference = component.Models.GadgetTargetModel.Reference;
            entityAction.ModelAction.ExtensionTextModel.Value = component.Models.GadgetTargetModel.Value;
            entityAction.ModelAction.ExtensionTextModel.ExternalLink = component.Models.GadgetTargetModel.ExternalLink;
            entityAction.ModelAction.ComponentInfoModel.Enabled = component.Models.GadgetTargetModel.Enabled;
          }
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace