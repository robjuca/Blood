/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Models.Gadget
{
  public class GadgetTarget
  {
    #region Property
    public Guid Id
    {
      get; 
      set;
    }

    public Guid MaterialId
    {
      get; 
      set;
    }

    public string Target
    {
      get; 
      set;
    }

    public string Description
    {
      get; 
      set;
    }

    public string Reference
    {
      get; 
      set;
    }

    public string Value
    {
      get; 
      set;
    }

    public string ExternalLink
    {
      get; 
      set;
    }

    public bool Enabled
    {
      get; 
      set;
    } 
    #endregion

    #region Constructor
    public GadgetTarget ()
    {
      // Has only one child node (GadgetMaterial)

      Id = Guid.Empty;
      MaterialId = Guid.Empty;

      Target = string.Empty;
      Description = string.Empty;
      Reference = string.Empty;
      Value = string.Empty;
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
    //public void CopyFrom (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    CopyFrom (action.ModelAction);
    //    CopyNode (action);
    //  }
    //}

    public void CopyFrom (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        MaterialId = alias.MaterialId;

        Target = alias.Target;
        Description = alias.Description;
        Reference = alias.Reference;
        Value = alias.Value;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetTarget alias)
    {
      if (alias.NotNull ()) {
        Target = alias.Target;
        Description = alias.Description;
        Reference = alias.Reference;
        Value = alias.Value;
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

    //public void RefreshModel (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Target)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      CopyNode (action);
    //      action.ModelAction.GadgetTargetModel.CopyFrom (this);

    //      action.CollectionAction.GadgetTargetCollection.Clear (); // gadget
    //      action.CollectionAction.GadgetMaterialCollection.Clear ();  // child node

    //      switch (action.Operation.Operation) {
    //        // Collection
    //        case Infrastructure.TOperation.Collection:
    //          OperationCollection (action);
    //          break;

    //        // Collection
    //        case Infrastructure.TOperation.Select:
    //          OperationSelect (action);
    //          break;
    //      }
    //    }
    //  }
    //}

    //public void UpdateModel (TComponentModel model, TEntityAction action)
    //{
    //  foreach (var item in action.CollectionAction.GadgetMaterialCollection) {
    //    if (item.Id.Equals (MaterialId)) {
    //      model.GadgetMaterialModel.CopyFrom (item);
    //      break;
    //    }
    //  }
    //}
    #endregion

    #region Static
    public static GadgetTarget CreateDefault => (new GadgetTarget ());
    #endregion

    #region Support
    //void CopyFrom (TModelAction modelAction)
    //{
    //  Id = modelAction.ComponentInfoModel.Id;
    //  MaterialId = modelAction.ExtensionNodeModel.ParentId;

    //  Target = modelAction.ExtensionTextModel.Text;
    //  Description = modelAction.ExtensionTextModel.Description;
    //  Reference = modelAction.ExtensionTextModel.Reference;
    //  Value = modelAction.ExtensionTextModel.Value;
    //  ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
    //  Enabled = modelAction.ComponentInfoModel.Enabled;
    //}

    //void CopyNode (TEntityAction action)
    //{
    //  // Has only one child node (GadgetMaterial)
    //  foreach (var node in action.CollectionAction.ExtensionNodeCollection) {
    //    // gadget Target must be child here
    //    if (node.ChildId.Equals (Id)) {
    //      action.ModelAction.ExtensionNodeModel.ChildId = node.ChildId;
    //      action.ModelAction.ExtensionNodeModel.ChildCategory = node.ChildCategory;
    //      action.ModelAction.ExtensionNodeModel.ParentId = node.ParentId;
    //      action.ModelAction.ExtensionNodeModel.ParentCategory = node.ParentCategory;

    //      MaterialId = MaterialId.IsEmpty () ? node.ParentId : MaterialId;  // must be child

    //      break;
    //    }
    //  }
    //}

    //void OperationCollection (TEntityAction action)
    //{
    //  var gadgetId = action.ModelAction.GadgetTargetModel.Id;

    //  // update model collection
    //  foreach (var modelAction in action.CollectionAction.ModelCollection) {
    //    // check if gadget exist
    //    if (gadgetId.IsEmpty ()) {
    //      var entityAction = TEntityAction.CreateDefault;
    //      entityAction.ModelAction.CopyFrom (modelAction.Value);

    //      // child node
    //      foreach (var item in action.CollectionAction.ExtensionNodeCollection) {
    //        entityAction.CollectionAction.ExtensionNodeCollection.Add (item);
    //      }

    //      var gadget = GadgetTarget.CreateDefault;
    //      gadget.CopyFrom (entityAction);

    //      action.CollectionAction.GadgetTargetCollection.Add (gadget);
    //    }
    //  }
    //}

    //void OperationSelect (TEntityAction action)
    //{
    //  var gadgetId = action.ModelAction.GadgetTargetModel.Id;

    //  if (gadgetId.NotEmpty ()) {
    //    if (action.ModelAction.ExtensionNodeModel.ParentId.Equals (gadgetId)) {
    //      // only GadgetMaterial as node
    //      var childGadgetId = action.ModelAction.ExtensionNodeModel.ChildId;

    //      if (action.CollectionAction.ModelCollection.ContainsKey (childGadgetId)) {
    //        var childModelAction = action.CollectionAction.ModelCollection [childGadgetId];
    //        var entityAction = TEntityAction.CreateDefault;
    //        entityAction.ModelAction.CopyFrom (childModelAction);

    //        var gadget = GadgetMaterial.CreateDefault;
    //        gadget.CopyFrom (entityAction);

    //        action.CollectionAction.GadgetMaterialCollection.Add (gadget); // child node
    //      }
    //    }
    //  }
    //}
    #endregion
  };
  //---------------------------//

}  // namespace