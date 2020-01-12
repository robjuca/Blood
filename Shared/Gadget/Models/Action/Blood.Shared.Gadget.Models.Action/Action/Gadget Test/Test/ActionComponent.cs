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
  public static class TGadgetTestActionComponent
  {
    #region Static Members
    public static void Select (Collection<TGadgetTestModel> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull () && entityAction.NotNull ()) {
        gadgets.Clear ();

        if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
          foreach (var item in entityAction.CollectionAction.ModelCollection) {
            var modelAction = item.Value;
            var gadget = TGadgetTestModel.Create (modelAction.ComponentInfoModel.Name, modelAction.ComponentStatusModel.Busy);

            gadget.Model.Id = modelAction.ComponentInfoModel.Id;
            gadget.Model.Test = modelAction.ExtensionTextModel.Text;
            gadget.Model.Description = modelAction.ExtensionTextModel.Description;
            gadget.Model.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
            gadget.Model.Material = modelAction.ExtensionTextModel.Extension;
            gadget.Model.Enabled = modelAction.ComponentInfoModel.Enabled;

            // update
            if (entityAction.CollectionAction.ComponentRelationCollection.Count.Equals (0)) {
              foreach (var componentRelation in entityAction.CollectionAction.ComponentOperation.ParentCategoryCollection) {
                foreach (var relation in componentRelation.Value) {
                  entityAction.CollectionAction.ComponentRelationCollection.Add (relation);
                }
              }
            }

            foreach (var relation in entityAction.CollectionAction.ComponentRelationCollection) {
              if (relation.ParentId.IsEmpty ()) {
                gadget.Model.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
              }

              else {
                if (relation.ParentId.Equals (gadget.Id)) {
                  gadget.Model.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
                }
              }
            }
            
            gadgets.Add (gadget);
          }

          // sort
          var list = new Collection<TGadgetTestModel> (gadgets.OrderBy (p => p.Name).ToList ());

          gadgets.Clear ();

          foreach (var item in list) {
            gadgets.Add (item);
          }
        }
      }
    }

    public static void Select (TGadgetTestModel gadget, TEntityAction entityAction)
    {
      if (gadget.NotNull () && entityAction.NotNull ()) {
        if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
          var modelAction = entityAction.ModelAction;

          gadget.CopyFrom (TGadgetTestModel.Create (modelAction.ComponentInfoModel.Name, modelAction.ComponentStatusModel.Busy));

          gadget.Model.Id = modelAction.ComponentInfoModel.Id;
          gadget.Model.Test = modelAction.ExtensionTextModel.Text;
          gadget.Model.Description = modelAction.ExtensionTextModel.Description;
          gadget.Model.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
          gadget.Model.Material = modelAction.ExtensionTextModel.Extension;
          gadget.Model.Enabled = modelAction.ComponentInfoModel.Enabled;

          // update
          if (gadget.ValidateId) {
            // content list
            foreach (var item in entityAction.ComponentOperation.ParentIdCollection) {
              foreach (var relation in item.Value) {
                gadget.Model.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
              }
            }

            var idCollection = new Collection<Guid> ();
            gadget.Model.RequestContentId (idCollection);

            foreach (var id in idCollection) {
              if (entityAction.CollectionAction.EntityCollection.ContainsKey (id)) {
                var gadgetEntityAction = entityAction.CollectionAction.EntityCollection [id];

                // target
                if (gadgetEntityAction.CategoryType.IsCategory (TCategory.Target)) {
                  var gadgetTarget = TGadgetTargetModel.CreateDefault;
                  TGadgetTargetActionComponent.Select (gadgetTarget, gadgetEntityAction);

                  gadget.Model.AddContent (gadgetTarget.Model);
                }

                // test
                if (gadgetEntityAction.CategoryType.IsCategory (TCategory.Test)) {
                  var gadgetTest = TGadgetTestModel.CreateDefault;
                  TGadgetTestActionComponent.Select (gadgetTest, gadgetEntityAction);

                  gadget.Model.AddContent (gadgetTest.Model);
                }
              }
            }
          }
        }
      }
    }

    public static void Request (TGadgetTestModel gadget, TEntityAction entityAction)
    {
      if (gadget.NotNull () && entityAction.NotNull ()) {
        entityAction.Id = gadget.Model.Id;
        entityAction.CategoryType.Select (TCategory.Test);

        entityAction.ModelAction.ComponentInfoModel.Name = gadget.Name;
        entityAction.ModelAction.ComponentStatusModel.Busy = gadget.Busy;

        entityAction.ModelAction.ComponentInfoModel.Id = gadget.Model.Id;
        entityAction.ModelAction.ExtensionTextModel.Text = gadget.Model.Test;
        entityAction.ModelAction.ExtensionTextModel.Description = gadget.Model.Description;
        entityAction.ModelAction.ExtensionTextModel.ExternalLink = gadget.Model.ExternalLink;
        entityAction.ModelAction.ComponentInfoModel.Enabled = gadget.Model.Enabled;
      }
    }
    #endregion
  };
  //---------------------------//

  //public void RefreshModel (TEntityAction action)
  //{
  //  if (action.NotNull ()) {
  //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Test)) {
  //      // collection
  //      if (action.ModelAction.ComponentInfoModel.Id.IsEmpty ()) {
  //        // action.CollectionAction.ModelCollection [Id(GadgetTest)]
  //        foreach (var modelAction in action.CollectionAction.ModelCollection) {
  //          var gadget = GadgetTest.CreateDefault;
  //          gadget.CopyFrom (modelAction.Value);

  //          foreach (var item in action.ComponentOperation.ParentCategoryCollection) {
  //            var relationList = item.Value
  //              .Where (p => p.ParentId.Equals (gadget.Id))
  //              .ToList ()
  //            ;

  //            foreach (var relation in relationList) {
  //              gadget.AddContentId (relation.ChildId, Server.Models.Infrastructure.TCategoryType.FromValue (relation.ChildCategory));
  //            }
  //          }

  //          action.CollectionAction.GadgetTestCollection.Add (gadget);
  //        }
  //      }

  //      // just me
  //      else {
  //        // update model action
  //        CopyFrom (action.ModelAction); // my self

  //        // content list
  //        foreach (var item in action.ComponentOperation.ParentIdCollection) {
  //          foreach (var relation in item.Value) {
  //            Content.Add (relation.ChildId, Server.Models.Infrastructure.TCategoryType.FromValue (relation.ChildCategory));
  //          }
  //        }

  //        // update
  //        action.ModelAction.GadgetTestModel.CopyFrom (this);

  //        action.CollectionAction.GadgetTestCollection.Clear ();

  //        // update model collection
  //        foreach (var modelAction in action.CollectionAction.ModelCollection) {
  //          action.ModelAction.CopyFrom (modelAction.Value);

  //          var gadget = GadgetTest.CreateDefault;
  //          gadget.CopyFrom (action);

  //          modelAction.Value.GadgetTestModel.CopyFrom (gadget); // update colection

  //          action.CollectionAction.GadgetTestCollection.Add (gadget);
  //        }

  //        // update contents
  //        Content.Update (action);
  //      }
  //    }
  //  }
  //}

  //public void UpdateModel (TEntityAction action)
  //{
  //  if (action.NotNull ()) {
  //    if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Test)) {
  //      if (action.ModelAction.GadgetTestModel.Id.Equals (Id)) {
  //        Content.Update (action);
  //      }
  //    }
  //  }
  //}

  //public void UpdateContents (TEntityAction action)
  //{
  //  Content.Update (action);
  //}


}  // namespace