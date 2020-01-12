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
  public static class TGadgetTargetActionComponent
  {
    #region Static Members
    public static void Select (Collection<TGadgetTargetModel> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull () && entityAction.NotNull ()) {
        gadgets.Clear ();

        if (entityAction.CategoryType.IsCategory (TCategory.Target)) {
          foreach (var item in entityAction.CollectionAction.ModelCollection) {
            var modelAction = item.Value;
            var gadget = TGadgetTargetModel.Create (modelAction.ComponentInfoModel.Name, modelAction.ComponentStatusModel.Busy);

            gadget.Model.Id = modelAction.ComponentInfoModel.Id;
            gadget.Model.MaterialId = modelAction.ExtensionNodeModel.ParentId;
            gadget.Model.Target = modelAction.ExtensionTextModel.Text;
            gadget.Model.Description = modelAction.ExtensionTextModel.Description;
            gadget.Model.Reference = modelAction.ExtensionTextModel.Reference;
            gadget.Model.Value = modelAction.ExtensionTextModel.Value;
            gadget.Model.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
            gadget.Model.Enabled = modelAction.ComponentInfoModel.Enabled;

            //  // Has only one child node (GadgetMaterial)
            foreach (var node in entityAction.CollectionAction.ExtensionNodeCollection) {
              // gadget Target must be child here
              if (node.ChildId.Equals (gadget.Id)) {
                entityAction.ModelAction.ExtensionNodeModel.ChildId = node.ChildId;
                entityAction.ModelAction.ExtensionNodeModel.ChildCategory = node.ChildCategory;
                entityAction.ModelAction.ExtensionNodeModel.ParentId = node.ParentId;
                entityAction.ModelAction.ExtensionNodeModel.ParentCategory = node.ParentCategory;

                gadget.Model.MaterialId = gadget.Model.MaterialId.IsEmpty () ? node.ParentId : gadget.Model.MaterialId;  // must be child

                break;
              }
            }

            gadgets.Add (gadget);
          }

          var list = new Collection<TGadgetTargetModel> (gadgets.OrderBy (p => p.Name).ToList ());

          gadgets.Clear ();

          foreach (var item in list) {
            gadgets.Add (item);
          }
        }
      }
    }

    public static void Select (TGadgetTargetModel gadget, TEntityAction entityAction)
    {
      if (gadget.NotNull () && entityAction.NotNull ()) {
        if (entityAction.CategoryType.IsCategory (TCategory.Target)) {
          var modelAction = entityAction.ModelAction;

          gadget.CopyFrom (TGadgetTargetModel.Create (modelAction.ComponentInfoModel.Name, modelAction.ComponentStatusModel.Busy));

          gadget.Model.Id = modelAction.ComponentInfoModel.Id;
          gadget.Model.MaterialId = modelAction.ExtensionNodeModel.ParentId;
          gadget.Model.Target = modelAction.ExtensionTextModel.Text;
          gadget.Model.Description = modelAction.ExtensionTextModel.Description;
          gadget.Model.Reference = modelAction.ExtensionTextModel.Reference;
          gadget.Model.Value = modelAction.ExtensionTextModel.Value;
          gadget.Model.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
          gadget.Model.Enabled = modelAction.ComponentInfoModel.Enabled;

          //  // Has only one child node (GadgetMaterial)
          foreach (var node in entityAction.CollectionAction.ExtensionNodeCollection) {
            // gadget Target must be child here
            if (node.ChildId.Equals (gadget.Id)) {
              entityAction.ModelAction.ExtensionNodeModel.ChildId = node.ChildId;
              entityAction.ModelAction.ExtensionNodeModel.ChildCategory = node.ChildCategory;
              entityAction.ModelAction.ExtensionNodeModel.ParentId = node.ParentId;
              entityAction.ModelAction.ExtensionNodeModel.ParentCategory = node.ParentCategory;

              gadget.Model.MaterialId = gadget.Model.MaterialId.IsEmpty () ? node.ParentId : gadget.Model.MaterialId;  // must be child

              break;
            }
          }
        }
      }
    }

    public static void Request (TGadgetTargetModel gadget, TEntityAction entityAction)
    {
      if (gadget.NotNull () && entityAction.NotNull ()) {
        entityAction.Id = gadget.Model.Id;
        entityAction.CategoryType.Select (TCategory.Target);

        entityAction.ModelAction.ComponentInfoModel.Name = gadget.Name;
        entityAction.ModelAction.ComponentStatusModel.Busy = gadget.Busy;

        entityAction.ModelAction.ComponentInfoModel.Id = gadget.Model.Id;
        entityAction.ModelAction.ExtensionNodeModel.ChildId = gadget.Id;
        entityAction.ModelAction.ExtensionNodeModel.ChildCategory = TCategoryType.ToValue (TCategory.Target);
        entityAction.ModelAction.ExtensionNodeModel.ParentId = gadget.Model.MaterialId;
        entityAction.ModelAction.ExtensionNodeModel.ParentCategory = TCategoryType.ToValue (TCategory.Material);
        entityAction.ModelAction.ExtensionTextModel.Text = gadget.Model.Target;
        entityAction.ModelAction.ExtensionTextModel.Description = gadget.Model.Description;
        entityAction.ModelAction.ExtensionTextModel.Reference = gadget.Model.Reference;
        entityAction.ModelAction.ExtensionTextModel.Value = gadget.Model.Value;
        entityAction.ModelAction.ExtensionTextModel.ExternalLink = gadget.Model.ExternalLink;
        entityAction.ModelAction.ComponentInfoModel.Enabled = gadget.Model.Enabled;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace