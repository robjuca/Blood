﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Linq;

using Server.Models.Infrastructure;
using Server.Models.Action;
//---------------------------//

namespace Server.Context.Component
{
  public class TOperationSupport
  {
    #region Property
    public Guid Id
    {
      get;
    }

    public int CategoryValue
    {
      get;
    }
    #endregion

    #region Constructor
    public TOperationSupport (TModelContext context, TEntityAction action)
      : this ()
    {
      /*
      DATA IN
      - action.Id 
      - action.CollectionAction.CategoryRelationCollection

      DATA OUT
      - action.ModelAction (model)
      - action.CollectionAction.ModeCollection {id, model} (for each node)
      */

      if (action.NotNull ()) {
        Id = action.Id;

        if (context.NotNull ()) {
          var descriptors = context.ComponentDescriptor.AsQueryable ()
          .Where (p => p.Id.Equals (Id))
          .ToList ()
        ;

          // found (request Category)
          if (descriptors.Any ()) {
            CategoryValue = descriptors [0].Category;
          }
        }
      }
    }

    TOperationSupport ()
    {
      Id = Guid.Empty;
      CategoryValue = TCategoryType.ToValue (TCategory.None);
    }
    #endregion

    #region Members
    internal void RequestComponent (TModelContext context, TEntityAction action)
    {
      if (context.NotNull () && action.NotNull ()) {
        RequestComponent (Id, context, action, action.ModelAction);
      }
    }

    public void RequestExtension (TModelContext context, TEntityAction action)
    {
      if (context.NotNull () && action.NotNull ()) {
        RequestExtension (CategoryValue, Id, context, action, action.ModelAction);
      }
    }

    internal static void RequestNode (TModelContext context, TEntityAction action)
    {
      /*
       DATA IN
      - action.Id {used as ParentId}

      DATA OUT
       - action.CollectionAction.ExtensionNodeCollection
       - action.CollectionAction.ModelCollection {id, modelAction}    // node model
      */

      if (context.NotNull () && action.NotNull ()) {
        var nodesCollection = context.ExtensionNode.AsQueryable ()
          .Where (p => p.ParentId.Equals (action.Id))
          .ToList ()
        ;

        var nodeReverse = action.ModelAction.ComponentStatusModel.NodeReverse;

        if (nodeReverse) {
          nodesCollection = context.ExtensionNode.AsQueryable ()
            .Where (p => p.ChildId.Equals (action.Id))
            .ToList ()
          ;
        }

        try {
          // node (child)
          foreach (var node in nodesCollection) {
            action.CollectionAction.ExtensionNodeCollection.Add (node);

            var id = nodeReverse ? node.ParentId : node.ChildId;
            var categoryValue = nodeReverse ? node.ParentCategory : node.ChildCategory;

            var modelAction = TModelAction.CreateDefault;

            if (RequestComponent (id, context, action, modelAction)) {
              if (RequestExtension (categoryValue, id, context, action, modelAction)) {
                action.CollectionAction.ModelCollection.Add (id, modelAction);    // add node model
              }
            }
          }
        }

        catch (Exception exception) {
          THelper.FormatException ("RequestNode - TOperationSupport", exception, action);
        }
      }
    }

    internal static void RequestRelation (TModelContext context, TEntityAction action)
    {
      /*
       DATA IN
      - action.ComponentOperation 

      DATA OUT
      - action.ComponentOperation
      */

      if (context.NotNull () && action.NotNull ()) {
        // Component Relation
        var componentRelationFullList = context.ComponentRelation
          .ToList ()
        ;

        // by Category
        if (action.ComponentOperation.IsComponentOperation (TComponentOperation.TInternalOperation.Category)) {
          foreach (var categoryValue in action.ComponentOperation.CategoryCollection) {
            // parent 
            var parentList = componentRelationFullList
              .Where (p => p.ParentCategory.Equals (categoryValue))
              .ToList ()
            ;

            if (parentList.Any ()) {
              action.CollectionAction.ComponentOperation.SelectParent (categoryValue, parentList);
            }

            // child
            var childList = componentRelationFullList
              .Where (p => p.ChildCategory.Equals (categoryValue))
              .ToList ()
            ;

            if (childList.Any ()) {
              action.CollectionAction.ComponentOperation.SelectChild (categoryValue, childList);
            }
          }
        }

        // by Id
        if (action.ComponentOperation.IsComponentOperation (TComponentOperation.TInternalOperation.Id)) {
          foreach (var id in action.ComponentOperation.IdCollection) {
            // parent 
            var parentList = componentRelationFullList
              .Where (p => p.ParentId.Equals (id))
              .ToList ()
            ;

            if (parentList.Any ()) {
              action.CollectionAction.ComponentOperation.SelectParent (id, parentList);
            }

            // child
            var childList = componentRelationFullList
              .Where (p => p.ChildId.Equals (id))
              .ToList ()
            ;

            if (childList.Any ()) {
              action.CollectionAction.ComponentOperation.SelectChild (id, childList);
            }
          }
        }
      }
    }
    #endregion

    #region Support
    static bool RequestComponent (Guid id, TModelContext context, TEntityAction action, TModelAction modelAction)
    {
      /*
      DATA OUT
      - action.ModelAction (model)
      */

      bool res = false;

      try {
        // info
        var infoList = context.ComponentInfo.AsQueryable()
          .Where (p => p.Id.Equals (id))
          .ToList ()
        ;

        // info found
        if (infoList.Count.Equals (1)) {
          var model = infoList [0];
          modelAction.ComponentInfoModel.CopyFrom (model);

          // update category
          if (action.CategoryType.IsCategory (TCategory.None)) {
            var descList = context.ComponentDescriptor.AsQueryable()
              .Where (p => p.Id.Equals (action.Id))
              .ToList ()
            ;

            // found
            if (descList.Count.Equals (1)) {
              var desc = descList [0];
              action.SelectCategoryType (TCategoryType.Create (TCategoryType.FromValue (desc.Category)));
            }
          }
        }

        // status
        var statusList = context.ComponentStatus.AsQueryable()
          .Where (p => p.Id.Equals (id))
          .ToList ()
        ;

        // status found
        if (statusList.Count.Equals (1)) {
          var model = statusList [0];
          modelAction.ComponentStatusModel.CopyFrom (model);
        }

        res = true;
      }

      catch (Exception exception) {
        THelper.FormatException ("RequestComponent - TOperationSupport", exception, action);
      }

      return (res);
    }

    static bool RequestExtension (int categoryValue, Guid id, TModelContext context, TEntityAction action, TModelAction modelAction)
    {
      /*
      DATA OUT
      - action.ModelAction (model)
      - action.CollectionAction.ExtensionNodeCollection
      - action.ComponentModel.NodeModelCollection
      */

      var res = false;

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

        try {
          foreach (var extensionName in extension.ExtensionList) {
            switch (extensionName) {
              //case TComponentExtensionNames.Document: {
              //    var list = context.ExtensionDocument
              //      .Where (p => p.Id.Equals (id))
              //      .ToList ()
              //    ;

              //    if (list.Count.Equals (1)) {
              //      modelAction.ExtensionDocumentModel.CopyFrom (list [0]);
              //    }
              //  }
              //  break;

              case TComponentExtensionNames.Geometry: {
                  var list = context.ExtensionGeometry.AsQueryable()
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionGeometryModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionNames.Image: {
                  var list = context.ExtensionImage.AsQueryable()
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionImageModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionNames.Layout: {
                  var list = context.ExtensionLayout.AsQueryable()
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionLayoutModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionNames.Node: {
                  // child first
                  var childList = context.ExtensionNode.AsQueryable()
                    .Where (p => p.ChildId.Equals (id))
                    .ToList ()
                  ;

                  if (childList.Count.Equals (1)) {
                    var node = childList [0];
                    modelAction.ExtensionNodeModel.CopyFrom (node);

                    //  check duplicated
                    //var list = action.CollectionAction.ExtensionNodeCollection
                    //  .Where (p => p.ChildId.Equals (id))
                    //  .ToList ()
                    //;

                    //if (list.Count.Equals (0)) {
                    //  action.CollectionAction.ExtensionNodeCollection.Add (node);
                    //  action.ComponentModel.NodeModelCollection.Add (node);
                    //}
                  }

                  //else {
                  //  // parent next
                  //  //var parentList = context.ExtensionNode.AsQueryable()
                  //  //  .Where (p => p.ParentId.Equals (id))
                  //  //  .ToList ()
                  //  //;

                  //  //foreach (var node in parentList) {
                  //  //  //  check duplicated
                  //  //  var list = action.CollectionAction.ExtensionNodeCollection
                  //  //    .Where (p => p.ChildId.Equals (node.ChildId))
                  //  //    .ToList ()
                  //  //  ;

                  //  //  if (list.Count.Equals (0)) {
                  //  //    action.CollectionAction.ExtensionNodeCollection.Add (node);
                  //  //    action.ComponentModel.NodeModelCollection.Add (node);
                  //  //  }
                  //  //}
                  //}
                }
                break;

              case TComponentExtensionNames.Text: {
                  var list = context.ExtensionText.AsQueryable()
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionTextModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionNames.Content: {
                  var list = context.ExtensionContent.AsQueryable()
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionContentModel.CopyFrom (list [0]);
                  }
                }
                break;
            }
          }

          res = true;
        }

        catch (Exception exception) {
          THelper.FormatException ("RequestExtension - TOperationSupport", exception, action);
        }
      }

      return (res);
    }
    #endregion
  };
  //---------------------------//

}  // namespace