/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using rr.Library.Helper;

using Server.Models.Infrastructure;
using Server.Models.Component;
using Server.Models.Action;
//---------------------------//

namespace Server.Context.Component
{
  public sealed class TOperationCollection : IOperation
  {
    #region Interface
    public void Invoke (IModelContext modelContext, IEntityAction entityAction, TExtension extension)
    {
      if (modelContext.NotNull ()) {
        var context = TModelContext.CastTo (modelContext);

        var relationList = context.CategoryRelation
          .ToList ()
        ;

        if (entityAction.NotNull ()) {
          var action = TEntityAction.Request (entityAction);
          action.CollectionAction.SetCollection (relationList);

          if (action.Operation.HasExtension) {
            switch (extension) {
              case TExtension.Full: {
                  CollectionFull (context, action);
                }
                break;

              case TExtension.Minimum: {
                  CollectionMinimum (context, action);
                }
                break;

              case TExtension.ById:
              case TExtension.Idle:
              case TExtension.Many:
              case TExtension.Zap: {
                  Models.Infrastructure.THelper.FormatExtensionNotImplementedException (action);
                }
                break;
            }
          }

          else {
            Models.Infrastructure.THelper.FormatExtensionMustExistException (action);
          }
        }
      }
    }
    #endregion

    #region Support
    void CollectionFull (TModelContext context, TEntityAction action)
    {
      /* 
        DATA IN:
          action.Operation.CategoryType.Category  
          action.CollectionAction.CategoryRelationCollection
      */

      try {
        // select Id by Category
        var categoryValue = TCategoryType.ToValue (action.Operation.CategoryType.Category);

        var descriptors = context.ComponentDescriptor.AsQueryable()
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        // found
        if (descriptors.Any ()) {
          // Component Info, Status
          action.CollectionAction.ComponentInfoCollection.Clear ();
          action.CollectionAction.ComponentStatusCollection.Clear ();

          foreach (var descriptor in descriptors) {
            // Info
            var infoList = context.ComponentInfo.AsQueryable()
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // Status
            var statusList = context.ComponentStatus.AsQueryable()
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // info found
            if (infoList.Count.Equals (1)) {
              var infoModel = infoList [0];
              action.CollectionAction.ComponentInfoCollection.Add (infoModel);
            }

            // status found
            if (statusList.Count.Equals (1)) {
              var statusModel = statusList [0];
              action.CollectionAction.ComponentStatusCollection.Add (statusModel);
            }
          }

          // Component Relation
          // by Category
          action.CollectionAction.ComponentOperation.Clear ();
          action.CollectionAction.SelectComponentOperation (TComponentOperation.TInternalOperation.Category);
          action.CollectionAction.ComponentOperation.SelectByCategory (categoryValue);

          var componentRelationFullList = context.ComponentRelation
            .ToList ()
          ;

          foreach (var itemCategory in action.CollectionAction.ComponentOperation.CategoryCollection) {
            // parent 
            var parentList = componentRelationFullList
              .Where (p => p.ParentCategory.Equals (itemCategory))
              .ToList ()
            ;

            action.CollectionAction.ComponentOperation.SelectParent (itemCategory, parentList);

            // child
            var childList = componentRelationFullList
              .Where (p => p.ChildCategory.Equals (itemCategory))
              .ToList ()
            ;

            action.CollectionAction.ComponentOperation.SelectChild (itemCategory, childList);
          }

          // Extension (CategoryRelationCollection)
          var categoryRelationList = action.CollectionAction.CategoryRelationCollection
            .Where (p => p.Category.Equals (categoryValue))
            .ToList ()
          ;

          // found
          if (categoryRelationList.Count.Equals (1)) {
            var categoryRelation = categoryRelationList [0]; // get extension using TComponentExtension

            var extension = TComponentExtension.Create (categoryRelation.Extension);
            extension.Request ();

            foreach (var item in action.CollectionAction.ComponentInfoCollection) {
              var id = item.Id;

              // Status
              var compStatus = Models.Component.ComponentStatus.CreateDefault;

              var statusList = action.CollectionAction.ComponentStatusCollection
                .Where (p => p.Id.Equals (id))
                .ToList ()
              ;

              if (statusList.Count.Equals (1)) {
                compStatus.CopyFrom (statusList [0]);
              }

              foreach (var extensionName in extension.ExtensionList) {
                switch (extensionName) {
                  case TComponentExtensionNames.Document: {
                      //var list = context.ExtensionDocument
                      //  .Where (p => p.Id.Equals (id))
                      //  .ToList ()
                      //;

                      //if (list.Count.Equals (1)) {
                      //  action.CollectionAction.ExtensionDocumentCollection.Add (list [0]);
                      //}
                    }
                    break;

                  case TComponentExtensionNames.Geometry: {
                      var list = context.ExtensionGeometry.AsQueryable()
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionGeometryCollection.Add (list [0]);
                      }
                    }
                    break;

                  case TComponentExtensionNames.Image: {
                      var list = context.ExtensionImage.AsQueryable()
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionImageCollection.Add (list [0]);
                      }
                    }
                    break;

                  case TComponentExtensionNames.Layout: {
                      var list = context.ExtensionLayout.AsQueryable()
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionLayoutCollection.Add (list [0]);
                      }
                    }
                    break;

                  case TComponentExtensionNames.Node: {
                      var nodeModeldList = context.ExtensionNode.AsQueryable()
                        .Where (p => p.ParentId.Equals (id))
                        .ToList ()
                      ;

                      // Node Reverse
                      if (compStatus.NodeReverse) {
                        nodeModeldList = context.ExtensionNode.AsQueryable()
                          .Where (p => p.ChildId.Equals (id))
                          .ToList ()
                        ;
                      }

                      // Node Model
                      if (compStatus.UseNodeModel) {
                        if (nodeModeldList.Count.Equals (1)) {
                          action.ModelAction.ExtensionNodeModel.CopyFrom (nodeModeldList [0]);
                          action.CollectionAction.ExtensionNodeCollection.Add (nodeModeldList [0]);
                        }
                      }

                      // Node Collection
                      if (compStatus.UseNodeCollection) {
                        foreach (var nodeModel in nodeModeldList) {
                          action.CollectionAction.ExtensionNodeCollection.Add (nodeModel);
                        }
                      }
                    }
                    break;

                  case TComponentExtensionNames.Text: {
                      var list = context.ExtensionText.AsQueryable()
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionTextCollection.Add (list [0]);
                      }
                    }
                    break;

                  case TComponentExtensionNames.Content: {
                      var list = context.ExtensionContent.AsQueryable()
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionContentCollection.Add (list [0]);
                      }
                    }
                    break;
                }
              }
            }
          }

          // populate ModelCollection
          action.CollectionAction.ModelCollection.Clear ();

          var componentExtension = TComponentExtension.CreateDefault;
          action.RequestExtension (componentExtension);

          foreach (var item in action.CollectionAction.ComponentInfoCollection) {
            // component
            // Info
            var id = item.Id;
            var models = TModelAction.CreateDefault;

            models.ComponentInfoModel.CopyFrom (item);

            // Status
            var compStatus = ComponentStatus.CreateDefault;

            var statusList = action.CollectionAction.ComponentStatusCollection
              .Where (p => p.Id.Equals (id))
              .ToList ()
            ;

            // found
            if (statusList.Count.Equals (1)) {
              models.ComponentStatusModel.CopyFrom (statusList [0]);
              compStatus.CopyFrom (statusList [0]);
            }

            // extension
            foreach (var extensionName in componentExtension.ExtensionList) {
              switch (extensionName) {
                case TComponentExtensionNames.Document: {
                    //var list = action.CollectionAction.ExtensionDocumentCollection
                    //  .Where (p => p.Id.Equals (id))
                    //  .ToList ()
                    //;

                    //if (list.Count.Equals (1)) {
                    //  models.ExtensionDocumentModel.CopyFrom (list [0]);
                    //}
                  }
                  break;

                case TComponentExtensionNames.Geometry: {
                    var list = action.CollectionAction.ExtensionGeometryCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionGeometryModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionNames.Image: {
                    var list = action.CollectionAction.ExtensionImageCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionImageModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionNames.Layout: {
                    var list = action.CollectionAction.ExtensionLayoutCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionLayoutModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionNames.Node: {
                    var nodeModellist = action.CollectionAction.ExtensionNodeCollection
                      .Where (p => p.ParentId.Equals (id))
                      .ToList ()
                    ;

                    if (compStatus.NodeReverse) {
                      nodeModellist = action.CollectionAction.ExtensionNodeCollection
                        .Where (p => p.ChildId.Equals (id))
                        .ToList ()
                      ;
                    }

                    if (compStatus.UseNodeModel) {
                      if (nodeModellist.Count.Equals (1)) {
                        models.ExtensionNodeModel.CopyFrom (nodeModellist [0]);
                      }
                    }
                  }
                  break;

                case TComponentExtensionNames.Text: {
                    var list = action.CollectionAction.ExtensionTextCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionTextModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionNames.Content: {
                    var list = action.CollectionAction.ExtensionContentCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionContentModel.CopyFrom (list [0]);
                    }
                  }
                  break;
              }
            }

            action.CollectionAction.ModelCollection.Add (id, models);
          }
        }

        // ComponentOperation - Operation.Category
        if (action.CollectionAction.ComponentOperation.IsComponentOperation (TComponentOperation.TInternalOperation.Category)) {
          action.IdCollection.Clear ();

          var list = new Collection<ComponentRelation> (action.CollectionAction.ComponentOperation.RequestParentCategoryCollection ());

          foreach (var relation in list) {
            if (relation.ChildId.NotEmpty ()) {
              action.IdCollection.Add (relation.ChildId);
            }
          }
        }

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Models.Infrastructure.THelper.FormatException ("Collection Full", exception, action);
      }
    }

    void CollectionMinimum (TModelContext context, TEntityAction action)
    {
      /* 
        DATA IN:
          action.Operation.CategoryType.Category  
          action.CollectionAction.CategoryRelationCollection
      */

      try {
        // select Id by Category
        var categoryValue = TCategoryType.ToValue (action.Operation.CategoryType.Category);

        var descriptors = context.ComponentDescriptor.AsQueryable()
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        // found
        if (descriptors.Any ()) {
          // Component Info, Status
          action.CollectionAction.ComponentInfoCollection.Clear ();
          action.CollectionAction.ComponentStatusCollection.Clear ();

          foreach (var descriptor in descriptors) {
            // Info
            var infoList = context.ComponentInfo.AsQueryable()
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // Status
            var statusList = context.ComponentStatus.AsQueryable()
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // info found
            if (infoList.Count.Equals (1)) {
              var infoModel = infoList [0];
              action.CollectionAction.ComponentInfoCollection.Add (infoModel);
            }

            // status found
            if (statusList.Count.Equals (1)) {
              var statusModel = statusList [0];
              action.CollectionAction.ComponentStatusCollection.Add (statusModel);
            }
          }

          // Extension (CategoryRelationCollection)
          var categoryRelationList = action.CollectionAction.CategoryRelationCollection
            .Where (p => p.Category.Equals (categoryValue))
            .ToList ()
          ;

          // found
          if (categoryRelationList.Count.Equals (1)) {
            var categoryRelation = categoryRelationList [0]; // get extension using TComponentExtension

            var extension = TComponentExtension.Create (categoryRelation.Extension);
            extension.Request ();

            foreach (var item in action.CollectionAction.ComponentInfoCollection) {
              var id = item.Id;

              foreach (var extensionName in extension.ExtensionList) {
                switch (extensionName) {
                  case TComponentExtensionNames.Layout: {
                      var list = context.ExtensionLayout.AsQueryable()
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;


                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionLayoutCollection.Add (list [0]);
                      }
                    }
                    break;
                }
              }
            }
          }

          // populate ModelCollection
          action.CollectionAction.ModelCollection.Clear ();

          var componentExtension = TComponentExtension.CreateDefault;
          action.RequestExtension (componentExtension);


          foreach (var item in action.CollectionAction.ComponentInfoCollection) {
            // component
            // Info
            var id = item.Id;
            var models = TModelAction.CreateDefault;

            models.ComponentInfoModel.CopyFrom (item);

            // Status
            var statusList = action.CollectionAction.ComponentStatusCollection
              .Where (p => p.Id.Equals (id))
              .ToList ()
            ;

            // found
            if (statusList.Count.Equals (1)) {
              models.ComponentStatusModel.CopyFrom (statusList [0]);
            }

            // extension
            foreach (var extensionName in componentExtension.ExtensionList) {
              switch (extensionName) {
                case TComponentExtensionNames.Layout: {
                    var list = action.CollectionAction.ExtensionLayoutCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionLayoutModel.CopyFrom (list [0]);
                    }
                  }
                  break;
              }
            }

            action.CollectionAction.ModelCollection.Add (id, models);
          }
        }

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Models.Infrastructure.THelper.FormatException ("Collection Full", exception, action);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace