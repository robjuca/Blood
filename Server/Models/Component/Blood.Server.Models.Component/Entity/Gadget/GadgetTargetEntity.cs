﻿/*----------------------------------------------------------------
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
      // Has only one child node (GadgetMaterial)

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
        CopyFrom (action.ModelAction);
        CopyNode (action);
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

    public void RefreshModel (TEntityAction action)
    {
      // TODO: review
      if (action.NotNull ()) {
        if (action.CategoryType.IsCategory (Infrastructure.TCategory.Target)) {
          // update model action
          CopyFrom (action.ModelAction); // my self
          CopyNode (action);
          action.ModelAction.GadgetTargetModel.CopyFrom (this);

          action.CollectionAction.GadgetTargetCollection.Clear ();

          switch (action.Operation.Operation) {
            // Collection
            case Infrastructure.TOperation.Collection:
              OperationCollection (action);
              break;

            // Collection
            case Infrastructure.TOperation.Select:
              OperationSelect (action);
              break;
          }
        }
      }
    }
    #endregion

    #region Static
    public static GadgetTarget CreateDefault => (new GadgetTarget ());
    #endregion

    #region Support
    void CopyFrom (TModelAction modelAction)
    {
      Id = modelAction.ComponentInfoModel.Id;
      MaterialId = modelAction.ExtensionNodeModel.ChildId;

      Target = modelAction.ExtensionTextModel.Text;
      Description = modelAction.ExtensionTextModel.Description;
      ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
      Enabled = modelAction.ComponentInfoModel.Enabled;
    }

    void CopyNode (TEntityAction action)
    {
      // Has only one child node (GadgetMaterial)
      foreach (var node in action.CollectionAction.ExtensionNodeCollection) {
        if (node.ParentId.Equals (Id)) {
          action.ModelAction.ExtensionNodeModel.ChildId = node.ChildId;
          action.ModelAction.ExtensionNodeModel.ChildCategory = node.ChildCategory;
          action.ModelAction.ExtensionNodeModel.ParentId = node.ParentId;
          action.ModelAction.ExtensionNodeModel.ParentCategory = node.ParentCategory;

          MaterialId = node.ChildId;

          break;
        }
      }
    }

    void OperationCollection (TEntityAction action)
    {
      var gadgetId = action.ModelAction.GadgetTargetModel.Id;

      // update model collection
      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        // check if gadget exist
        if (gadgetId.IsEmpty ()) {
          var entityAction = TEntityAction.CreateDefault;
          entityAction.ModelAction.CopyFrom (modelAction.Value);

          // child node
          foreach (var item in action.CollectionAction.ExtensionNodeCollection) {
            entityAction.CollectionAction.ExtensionNodeCollection.Add (item);
          }

          var gadget = GadgetTarget.CreateDefault;
          gadget.CopyFrom (entityAction);

          action.CollectionAction.GadgetTargetCollection.Add (gadget);
        }
      }
    }

    void OperationSelect (TEntityAction action)
    {
      var gadgetId = action.ModelAction.GadgetTargetModel.Id;

      if (action.ModelAction.ExtensionNodeModel.ParentId.Equals (gadgetId)) {
        // only GadgetMaterial as node
        var childGadgetId = action.ModelAction.ExtensionNodeModel.ChildId;

        if (action.CollectionAction.ModelCollection.ContainsKey (childGadgetId)) {
          var childModelAction = action.CollectionAction.ModelCollection [childGadgetId];
          var entityAction = TEntityAction.CreateDefault;
          entityAction.ModelAction.CopyFrom (childModelAction);

          var gadget = GadgetMaterial.CreateDefault;
          gadget.CopyFrom (entityAction);

          action.CollectionAction.GadgetMaterialCollection.Add (gadget);
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace