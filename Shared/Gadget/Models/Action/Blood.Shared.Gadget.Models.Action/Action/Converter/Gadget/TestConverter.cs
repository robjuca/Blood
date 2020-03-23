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
  public static class TGadgetTestConverter
  {
    #region Static Members
    public static void Collection (Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull ()) {
        gadgets.Clear ();

        if (entityAction.NotNull ()) {
          if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
            var gadgetCollection = new Collection<GadgetTest> ();

            foreach (var item in entityAction.CollectionAction.ModelCollection) {
              var modelAction = item.Value;
              var gadget = GadgetTest.CreateDefault;

              gadget.Id = modelAction.ComponentInfoModel.Id;
              gadget.GadgetName = modelAction.ExtensionTextModel.Text;
              gadget.Description = modelAction.ExtensionTextModel.Description;
              gadget.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
              gadget.Material = modelAction.ExtensionTextModel.Extension;
              gadget.Enabled = modelAction.ComponentInfoModel.Enabled;

              gadget.GadgetInfo = modelAction.ComponentInfoModel.Name;
              gadget.Busy = modelAction.ComponentStatusModel.Busy;

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
                  gadget.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
                }

                else {
                  if (gadget.Contains (relation.ParentId)) {
                    gadget.AddContentId (relation.ChildId, TCategoryType.FromValue (relation.ChildCategory));
                  }
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
              var component = TActionComponent.Create (TCategory.Test);
              component.Models.GadgetTestModel.CopyFrom (model);

              gadgets.Add (component);
            }
          }
        }
      }
    }

    public static void Select (TActionComponent component, TEntityAction entityAction)
    {
      if (entityAction.NotNull ()) {
        if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
          if (component.NotNull ()) {
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
                    var componentTarget = TActionComponent.Create (TCategory.Target);
                    TActionConverter.Select (TCategory.Target, componentTarget, gadgetEntityAction);

                    component.Models.GadgetTestModel.AddContent (componentTarget.Models.GadgetTargetModel);

                    // same material always
                    if (component.Models.GadgetMaterialModel.ValidateId.IsFalse ()) {
                      component.Models.GadgetMaterialModel.CopyFrom (componentTarget.Models.GadgetMaterialModel);
                    }
                  }

                  // test
                  if (gadgetEntityAction.CategoryType.IsCategory (TCategory.Test)) {
                    var componentTest = TActionComponent.Create (TCategory.Test);
                    TActionConverter.Select (TCategory.Test, componentTest, gadgetEntityAction);

                    component.Models.GadgetTestModel.AddContent (componentTest.Models.GadgetTestModel);

                    // update Material
                    if (component.Models.GadgetMaterialModel.ValidateId.IsFalse ()) {
                      component.Models.GadgetMaterialModel.CopyFrom (componentTest.Models.GadgetMaterialModel);
                    }
                  }

                  // update image
                  component.Models.GadgetTestModel.SetImage (component.Models.GadgetMaterialModel.GetImage ());
                }
              }
            }
          }
        }
      }
    }

    public static void SelectMany (Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      //entityAction.CollectionAction.EntityCollection

      if (entityAction.NotNull ()) {
        if (entityAction.CategoryType.IsCategory (TCategory.Test)) {
          if (gadgets.NotNull ()) {
            foreach (var component in gadgets) {
              var gadget = component.Models.GadgetTestModel;

              if (entityAction.CollectionAction.EntityCollection.ContainsKey (gadget.Id)) {
                var action = entityAction.CollectionAction.EntityCollection [gadget.Id];

                var idCollection = new Collection<Guid> ();
                gadget.RequestContentId (idCollection);

                // target
                if (gadget.IsContentTarget) {
                  foreach (var id in idCollection) {
                    if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
                      var gadgetEntityAction = action.CollectionAction.EntityCollection [id];
                      var someComponent = TActionComponent.Create (TCategory.Target);

                      TActionConverter.Select (TCategory.Target, someComponent, gadgetEntityAction);

                      gadget.AddContent (someComponent.Models.GadgetTargetModel);

                      // same material always
                      if (component.Models.GadgetMaterialModel.ValidateId.IsFalse ()) {
                        component.Models.GadgetMaterialModel.CopyFrom (someComponent.Models.GadgetMaterialModel);
                      }
                    }
                  }
                }

                // test
                if (gadget.IsContentTest) {
                  foreach (var id in idCollection) {
                    if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
                      var gadgetEntityAction = action.CollectionAction.EntityCollection [id];
                      var someComponent = TActionComponent.Create (TCategory.Test);

                      TActionConverter.Select (TCategory.Test, someComponent, gadgetEntityAction);

                      gadget.AddContent (someComponent.Models.GadgetTestModel);

                      // update Material
                      if (component.Models.GadgetMaterialModel.ValidateId.IsFalse ()) {
                        component.Models.GadgetMaterialModel.CopyFrom (someComponent.Models.GadgetMaterialModel);
                      }
                    }
                  }
                }

                // update image
                gadget.SetImage (component.Models.GadgetMaterialModel.GetImage ());
              }
            }
          }
        }
      }
    }

    public static void Request (TActionComponent component, TEntityAction entityAction)
    {
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Test)) {
          if (entityAction.NotNull ()) {
            entityAction.Id = component.Models.GadgetTestModel.Id;
            entityAction.CategoryType.Select (TCategory.Test);

            entityAction.ModelAction.ComponentInfoModel.Name = component.Models.GadgetTestModel.GadgetInfo;
            entityAction.ModelAction.ComponentStatusModel.Busy = component.Models.GadgetTestModel.Busy;

            entityAction.ModelAction.ComponentInfoModel.Id = component.Models.GadgetTestModel.Id;
            entityAction.ModelAction.ExtensionTextModel.Id = component.Models.GadgetTestModel.Id;
            entityAction.ModelAction.ExtensionTextModel.Text = component.Models.GadgetTestModel.GadgetName;
            entityAction.ModelAction.ExtensionTextModel.Description = component.Models.GadgetTestModel.Description;
            entityAction.ModelAction.ExtensionTextModel.ExternalLink = component.Models.GadgetTestModel.ExternalLink;
            entityAction.ModelAction.ComponentInfoModel.Enabled = component.Models.GadgetTestModel.Enabled;
          }
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace