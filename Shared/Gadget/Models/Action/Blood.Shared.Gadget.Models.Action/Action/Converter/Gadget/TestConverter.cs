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
  public static class TGadgetTestConverter
  {
    #region Static Members
    public static void Collection (Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      gadgets.Clear ();

      if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
        foreach (var item in entityAction.CollectionAction.ModelCollection) {
          var modelAction = item.Value;
          var component = TActionComponent.Create (TCategory.Test);

          component.Models.GadgetTestModel.Id = modelAction.ComponentInfoModel.Id;
          component.Models.GadgetTestModel.GadgetName= modelAction.ExtensionTextModel.Text;
          component.Models.GadgetTestModel.Description = modelAction.ExtensionTextModel.Description;
          component.Models.GadgetTestModel.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
          component.Models.GadgetTestModel.Material = modelAction.ExtensionTextModel.Extension;
          component.Models.GadgetTestModel.Enabled = modelAction.ComponentInfoModel.Enabled;
                   
          component.Models.GadgetTestModel.GadgetInfo = entityAction.ModelAction.ComponentInfoModel.Name;
          component.Models.GadgetTestModel.Busy = entityAction.ModelAction.ComponentStatusModel.Busy;

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
              component.Models.GadgetTestModel.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
            }

            else {
              if (component.Models.GadgetTestModel.Contains (relation.ParentId)) {
                component.Models.GadgetTestModel.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
              }
            }
          }

          gadgets.Add (component);
        }
      }
    }

    public static void Select (TActionComponent component, TEntityAction entityAction)
    {
      if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
        component.Select (TCategory.Test);

        component.Models.GadgetTestModel.Id = entityAction.ModelAction.ComponentInfoModel.Id;
        component.Models.GadgetTestModel.GadgetName = entityAction.ModelAction.ExtensionTextModel.Text;
        component.Models.GadgetTestModel.Description = entityAction.ModelAction.ExtensionTextModel.Description;
        component.Models.GadgetTestModel.ExternalLink = entityAction.ModelAction.ExtensionTextModel.ExternalLink;
        component.Models.GadgetTestModel.Material = entityAction.ModelAction.ExtensionTextModel.Extension;
        component.Models.GadgetTestModel.Enabled = entityAction.ModelAction.ComponentInfoModel.Enabled;

        component.Models.GadgetTestModel.GadgetInfo = entityAction.ModelAction.ComponentInfoModel.Name;
        component.Models.GadgetTestModel.Busy = entityAction.ModelAction.ComponentStatusModel.Busy;

        // update
        if (component.Models.GadgetTestModel.ValidateId) {
          // content list
          foreach (var item in entityAction.ComponentOperation.ParentIdCollection) {
            foreach (var relation in item.Value) {
              component.Models.GadgetTestModel.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
            }
          }

          var idCollection = new Collection<Guid> ();
          component.Models.GadgetTestModel.RequestContentId (idCollection);

          foreach (var id in idCollection) {
            if (entityAction.CollectionAction.EntityCollection.ContainsKey (id)) {
              var gadgetEntityAction = entityAction.CollectionAction.EntityCollection [id];

              // target
              if (gadgetEntityAction.CategoryType.IsCategory (TCategory.Target)) {
                var modelTarget = TActionComponent.Create (TCategory.Target);
                TGadgetTargetConverter.Select (modelTarget, gadgetEntityAction);

                component.Models.GadgetTestModel.AddContent (modelTarget.Models.GadgetTargetModel);
              }

              // test
              if (gadgetEntityAction.CategoryType.IsCategory (TCategory.Test)) {
                var modelTest = TActionComponent.Create (TCategory.Test);
                TGadgetTestConverter.Select (modelTest, gadgetEntityAction);

                component.Models.GadgetTestModel.AddContent (modelTest.Models.GadgetTestModel);
              }
            }
          }
        }
      }
    }

    public static void Request (TActionComponent component, TEntityAction entityAction)
    {
      if (component.IsCategory (TCategory.Test)) {
        entityAction.Id = component.Models.GadgetTestModel.Id;
        entityAction.CategoryType.Select (TCategory.Test);

        entityAction.ModelAction.ComponentInfoModel.Name = component.Models.GadgetTestModel.GadgetInfo;
        entityAction.ModelAction.ComponentStatusModel.Busy = component.Models.GadgetTestModel.Busy;

        entityAction.ModelAction.ComponentInfoModel.Id = component.Models.GadgetTestModel.Id;
        entityAction.ModelAction.ExtensionTextModel.Text = component.Models.GadgetTestModel.GadgetName;
        entityAction.ModelAction.ExtensionTextModel.Description = component.Models.GadgetTestModel.Description;
        entityAction.ModelAction.ExtensionTextModel.ExternalLink = component.Models.GadgetTestModel.ExternalLink;
        entityAction.ModelAction.ComponentInfoModel.Enabled = component.Models.GadgetTestModel.Enabled;
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