/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Server.Models.Action;
using Server.Models.Infrastructure;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public static class TGadgetTargetConverter
  {
    #region Static Members
    public static void Collection (Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      gadgets.Clear ();

      if (entityAction.CategoryType.IsCategory (TCategory.Target)) {
        foreach (var item in entityAction.CollectionAction.ModelCollection) {
          var modelAction = item.Value;
          var actionModel = TActionComponent.Create (TCategory.Target);

          actionModel.Models.GadgetTargetModel.Id = modelAction.ComponentInfoModel.Id;
          actionModel.Models.GadgetTargetModel.MaterialId = modelAction.ExtensionNodeModel.ParentId;
          actionModel.Models.GadgetTargetModel.GadgetName = modelAction.ExtensionTextModel.Text;
          actionModel.Models.GadgetTargetModel.Description = modelAction.ExtensionTextModel.Description;
          actionModel.Models.GadgetTargetModel.Reference = modelAction.ExtensionTextModel.Reference;
          actionModel.Models.GadgetTargetModel.Value = modelAction.ExtensionTextModel.Value;
          actionModel.Models.GadgetTargetModel.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
          actionModel.Models.GadgetTargetModel.Enabled = modelAction.ComponentInfoModel.Enabled;
                     
          actionModel.Models.GadgetTargetModel.GadgetInfo = modelAction.ComponentInfoModel.Name;
          actionModel.Models.GadgetTargetModel.Busy = modelAction.ComponentStatusModel.Busy;

          //  // Has only one child node (GadgetMaterial)
          foreach (var node in entityAction.CollectionAction.ExtensionNodeCollection) {
            // gadget Target must be child here
            if (actionModel.Models.GadgetTargetModel.Contains (node.ChildId)) {
              entityAction.ModelAction.ExtensionNodeModel.ChildId = node.ChildId;
              entityAction.ModelAction.ExtensionNodeModel.ChildCategory = node.ChildCategory;
              entityAction.ModelAction.ExtensionNodeModel.ParentId = node.ParentId;
              entityAction.ModelAction.ExtensionNodeModel.ParentCategory = node.ParentCategory;

              actionModel.Models.GadgetTargetModel.MaterialId = actionModel.Models.GadgetTargetModel.MaterialId.IsEmpty () ? node.ParentId : actionModel.Models.GadgetTargetModel.MaterialId;  // must be child

              break;
            }
          }

          gadgets.Add (actionModel);
        }
      }
    }

    public static void Select (TActionComponent component, TEntityAction entityAction)
    {
      if (entityAction.CategoryType.IsCategory (TCategory.Target)) {
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
      }
    }

    public static void Request (TActionComponent component, TEntityAction entityAction)
    {
      if (component.IsCategory (TCategory.Target)) {
        entityAction.Id = component.Models.GadgetTargetModel.Id;
        entityAction.CategoryType.Select (TCategory.Target);

        entityAction.ModelAction.ComponentInfoModel.Name = component.Models.GadgetTargetModel.GadgetInfo;
        entityAction.ModelAction.ComponentStatusModel.Busy = component.Models.GadgetTargetModel.Busy;

        entityAction.ModelAction.ComponentInfoModel.Id = component.Models.GadgetTargetModel.Id;
        entityAction.ModelAction.ExtensionNodeModel.ChildId = component.Models.GadgetTargetModel.Id;
        entityAction.ModelAction.ExtensionNodeModel.ChildCategory = TCategoryType.ToValue (TCategory.Target);
        entityAction.ModelAction.ExtensionNodeModel.ParentId = component.Models.GadgetTargetModel.MaterialId;
        entityAction.ModelAction.ExtensionNodeModel.ParentCategory = TCategoryType.ToValue (TCategory.Material);
        entityAction.ModelAction.ExtensionTextModel.Text = component.Models.GadgetTargetModel.GadgetName;
        entityAction.ModelAction.ExtensionTextModel.Description = component.Models.GadgetTargetModel.Description;
        entityAction.ModelAction.ExtensionTextModel.Reference = component.Models.GadgetTargetModel.Reference;
        entityAction.ModelAction.ExtensionTextModel.Value = component.Models.GadgetTargetModel.Value;
        entityAction.ModelAction.ExtensionTextModel.ExternalLink = component.Models.GadgetTargetModel.ExternalLink;
        entityAction.ModelAction.ComponentInfoModel.Enabled = component.Models.GadgetTargetModel.Enabled;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace