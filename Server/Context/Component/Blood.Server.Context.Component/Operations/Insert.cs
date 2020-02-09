/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Linq;

using rr.Library.Helper;

using Server.Models.Infrastructure;
using Server.Models.Component;
using Server.Models.Action;
//---------------------------//

namespace Server.Context.Component
{
  public sealed class TOperationInsert : IOperation
  {
    #region Interface
    public void Invoke (IModelContext modelContext, IEntityAction entityAction, Server.Models.Infrastructure.TExtension extension)
    {
      var context = TModelContext.CastTo (modelContext);

      var relationList = context.CategoryRelation
        .ToList ()
      ;

      var action = TEntityAction.Request (entityAction);
      action.CollectionAction.SetCollection (relationList);

      if (action.Operation.HasExtension) {
        Server.Models.Infrastructure.THelper.FormatExtensionNotImplementedException (action);
      }

      else {
        Insert (context, action);
      }
    }
    #endregion

    #region Support
    void Insert (TModelContext context, TEntityAction action)
    {
      /*
       DATA IN:
       - action.Operation.CategoryType.Category
       - action.CollectionAction.CategoryRelationCollection
       - action.ModelAction 
       - action.CollectionAction.ExtensionNodeCollection
       */

      try {
        // Validate Name
        if (ValidateString (action)) {
          //Id
          var id = Guid.NewGuid ();
          var categoryValue = TCategoryType.ToValue (action.Operation.CategoryType.Category);

          // Descriptor
          action.ModelAction.ComponentDescriptorModel.Id = id;
          action.ModelAction.ComponentDescriptorModel.Category = categoryValue;

          var compDescriptor = ComponentDescriptor.CreateDefault;
          compDescriptor.CopyFrom (action.ModelAction.ComponentDescriptorModel);

          context.ComponentDescriptor.Add (compDescriptor);

          // Info
          action.ModelAction.ComponentInfoModel.Id = id;

          var compInfo = ComponentInfo.CreateDefault;
          compInfo.CopyFrom (action.ModelAction.ComponentInfoModel);

          context.ComponentInfo.Add (compInfo);

          // Status
          action.ModelAction.ComponentStatusModel.Id = id;

          var compStatus = ComponentStatus.CreateDefault;
          compStatus.CopyFrom (action.ModelAction.ComponentStatusModel);

          context.ComponentStatus.Add (compStatus);

          // status collection
          foreach (var item in action.CollectionAction.ComponentStatusCollection) {
            var list = context.ComponentStatus
              .Where (p => p.Id.Equals (item.Id))
              .ToList ()
            ;

            // already exist (update)
            if (list.Count.Equals (1)) {
              var model = list [0];
              model.Change (item);

              context.ComponentStatus.Update (model);
            }

            // new (add)
            else {
              context.ComponentStatus.Add (item);
            }
          }

          // Component Relation Collection
          foreach (var item in action.CollectionAction.ComponentRelationCollection) {
            // change child status busy to true
            var childList = context.ComponentStatus
              .Where (p => p.Id.Equals (item.ChildId))
              .ToList ()
            ;

            // found
            if (childList.Count.Equals (1)) {
              var child = childList [0];
              child.Busy = true;

              context.ComponentStatus.Update (child); // update
            }

            item.ParentId = id;
            context.ComponentRelation.Add (item); // insert new
          }

          // extensions

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

            foreach (var extensionName in extension.ExtensionList) {
              switch (extensionName) {
                case TComponentExtensionName.Document: {
                    //action.ModelAction.ExtensionDocumentModel.Id = id;

                    //var extDocument = ExtensionDocument.CreateDefault;
                    //extDocument.CopyFrom (action.ModelAction.ExtensionDocumentModel);

                    //context.ExtensionDocument.Add (extDocument);
                  }
                  break;

                case TComponentExtensionName.Geometry: {
                    action.ModelAction.ExtensionGeometryModel.Id = id;

                    var extGeometry = ExtensionGeometry.CreateDefault;
                    extGeometry.CopyFrom (action.ModelAction.ExtensionGeometryModel);

                    context.ExtensionGeometry.Add (extGeometry);
                  }
                  break;

                case TComponentExtensionName.Image: {
                    action.ModelAction.ExtensionImageModel.Id = id;

                    var extImage = ExtensionImage.CreateDefault;
                    extImage.CopyFrom (action.ModelAction.ExtensionImageModel);

                    context.ExtensionImage.Add (extImage);
                  }
                  break;

                case TComponentExtensionName.Layout: {
                    action.ModelAction.ExtensionLayoutModel.Id = id;

                    var extLayout = ExtensionLayout.CreateDefault;
                    extLayout.CopyFrom (action.ModelAction.ExtensionLayoutModel);

                    context.ExtensionLayout.Add (extLayout);
                  }
                  break;

                case TComponentExtensionName.Node: {
                    // Node reverse
                    if (compStatus.NodeReverse) {
                      // use Node from ModelAction (only)
                      if (compStatus.UseNodeModel) {
                        action.ModelAction.ExtensionNodeModel.ChildId = id; // update
                        context.ExtensionNode.Add (action.ModelAction.ExtensionNodeModel);

                        // update status
                        var parentId = action.ModelAction.ExtensionNodeModel.ParentId;

                        var statusList = context.ComponentStatus
                          .Where (p => p.Id.Equals (parentId))
                          .ToList ()
                        ;

                        // found
                        if (statusList.Count.Equals (1)) {
                          var model = statusList [0];
                          model.Busy = true;

                          context.ComponentStatus.Update (model);
                        }

                        context.SaveChanges (); // update all
                      }
                    }

                    else {
                      // use Node from ModelAction 
                      if (compStatus.UseNodeModel) {
                        action.ModelAction.ExtensionNodeModel.ParentId = id; // update
                        context.ExtensionNode.Add (action.ModelAction.ExtensionNodeModel);
                      }

                      // Use Node Collection
                      if (compStatus.UseNodeCollection) {
                        foreach (var nodeModel in action.CollectionAction.ExtensionNodeCollection) {
                          nodeModel.ParentId = id; // for sure
                          context.ExtensionNode.Add (nodeModel);
                        }
                      }

                      context.SaveChanges (); // update all

                      // update status
                      var nodeList = context.ExtensionNode
                        .Where (p => p.ParentId.Equals (id))
                        .ToList ()
                      ;

                      foreach (var node in nodeList) {
                        var statusList = context.ComponentStatus
                          .Where (p => p.Id.Equals (node.ChildId))
                          .ToList ()
                        ;

                        // found
                        if (statusList.Count.Equals (1)) {
                          var model = statusList [0];
                          model.Busy = true;

                          context.ComponentStatus.Update (model);
                        }
                      }

                      context.SaveChanges (); // update all
                    }
                  }
                  break;

                case TComponentExtensionName.Text: {
                    action.ModelAction.ExtensionTextModel.Id = id;

                    var extText = ExtensionText.CreateDefault;
                    extText.CopyFrom (action.ModelAction.ExtensionTextModel);

                    context.ExtensionText.Add (extText);
                  }
                  break;

                case TComponentExtensionName.Content: {
                    action.ModelAction.ExtensionContentModel.Id = id;

                    var extContent = ExtensionContent.CreateDefault;
                    extContent.CopyFrom (action.ModelAction.ExtensionContentModel);

                    context.ExtensionContent.Add (extContent);
                  }
                  break;
              }
            }
          }

          context.SaveChanges ();

          action.Result = TValidationResult.Success;
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Insert", exception, action);
      }
    }

    bool ValidateString (TEntityAction action)
    {
      if (string.IsNullOrEmpty (action.ModelAction.ComponentInfoModel.Name.Trim ())) {
        action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Insert] Name can NOT be NULL or EMPTY!");
        return (false);
      }

      return (true);
    }
    #endregion
  };
  //---------------------------//

}  // namespace