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
  public sealed class TOperationRemove : IOperation
  {
    #region Interface
    public void Invoke (IModelContext modelContext, IEntityAction entityAction, Server.Models.Infrastructure.TExtension extension)
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
            Server.Models.Infrastructure.THelper.FormatExtensionNotImplementedException (action);
          }

          else {
            Remove (context, action);
          }
        }
      }
    }
    #endregion

    #region Support
    void Remove (TModelContext context, TEntityAction action)
    {
      /*
      DATA IN
      - action.Id (Component Id to remove)
      */

      var id = action.Id;

      try {
        //Id must exist
        if (id.IsEmpty ()) {
          action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Remove] Component Id can NOT be NULL or EMPTY!");
        }

        else {
          // search Id
          var descriptors = context.ComponentDescriptor.AsQueryable()
            .Where (p => p.Id.Equals (id))
            .ToList ()
          ;

          // Descriptor found
          if (descriptors.Count.Equals (1)) {
            var descriptor = descriptors [0];
            var categoryValue = descriptor.Category;

            // remove from Info model
            var infoList = context.ComponentInfo.AsQueryable()
              .Where (p => p.Id.Equals (id))
              .ToList ()
            ;

            // Info found
            if (infoList.Count.Equals (1)) {
              var info = infoList [0];
              context.ComponentInfo.Remove (info);// remove from Info model

              // remove from Status model
              var statusList = context.ComponentStatus.AsQueryable()
                .Where (p => p.Id.Equals (id))
                .ToList ()
              ;

              var compStatus = ComponentStatus.CreateDefault;

              // Status found
              if (statusList.Count.Equals (1)) {
                var statusModel = statusList [0];
                context.ComponentStatus.Remove (statusModel);  // remove from Status model

                compStatus.CopyFrom (statusModel);
              }

              // status collection
              foreach (var item in action.CollectionAction.ComponentStatusCollection) {
                var list = context.ComponentStatus.AsQueryable()
                  .Where (p => p.Id.Equals (item.Id))
                  .ToList ()
                ;

                // Status found
                if (list.Count.Equals (1)) {
                  var statusModel = list [0];
                  context.ComponentStatus.Remove (statusModel);  // remove from Status model
                }
              }

              // extensions

              // remove extensions
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
                    case TComponentExtensionNames.Document: {
                        //var list = context.ExtensionDocument
                        //  .Where (p => p.Id.Equals (id))
                        //  .ToList ()
                        //;

                        //if (list.Count.Equals (1)) {
                        //  context.ExtensionDocument.Remove (list [0]);
                        //}
                      }
                      break;

                    case TComponentExtensionNames.Geometry: {
                        var list = context.ExtensionGeometry.AsQueryable()
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          context.ExtensionGeometry.Remove (list [0]);
                        }
                      }
                      break;

                    case TComponentExtensionNames.Image: {
                        var list = context.ExtensionImage.AsQueryable()
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          context.ExtensionImage.Remove (list [0]);
                        }
                      }
                      break;

                    case TComponentExtensionNames.Layout: {
                        var list = context.ExtensionLayout.AsQueryable()
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          context.ExtensionLayout.Remove (list [0]);
                        }
                      }
                      break;

                    case TComponentExtensionNames.Node: {
                        // Node reverse
                        if (compStatus.NodeReverse) {
                          // request for ChildId
                          var nodeList = context.ExtensionNode.AsQueryable()
                            .Where (p => p.ChildId.Equals (id))
                            .ToList ()
                          ;

                          // just one
                          if (nodeList.Count.Equals (1)) {
                            var node = nodeList [0];

                            // status
                            var list = context.ComponentStatus.AsQueryable()
                              .Where (p => p.Id.Equals (node.ParentId))
                              .ToList ()
                            ;

                            // found (just me)
                            if (list.Count.Equals (1)) {
                              var model = list [0];
                              model.Busy = false;
                              context.ComponentStatus.Update (model);
                            }

                            // remove
                            context.ExtensionNode.Remove (node);
                          }
                        }

                        else {
                          // request for ParentId
                          var nodeList = context.ExtensionNode.AsQueryable()
                            .Where (p => p.ParentId.Equals (id))
                            .ToList ()
                          ;

                          foreach (var node in nodeList) {
                            // status
                            var list = context.ComponentStatus.AsQueryable()
                              .Where (p => p.Id.Equals (node.ChildId))
                              .ToList ()
                            ;

                            // found
                            if (list.Count.Equals (1)) {
                              var model = list [0];
                              model.Busy = false;
                              context.ComponentStatus.Update (model);
                            }

                            // remove
                            context.ExtensionNode.Remove (node);
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
                          context.ExtensionText.Remove (list [0]);
                        }
                      }
                      break;

                    case TComponentExtensionNames.Content: {
                        var list = context.ExtensionContent.AsQueryable()
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          context.ExtensionContent.Remove (list [0]);
                        }
                      }
                      break;
                  }
                }
              }

              // remove from Descriptor model
              context.ComponentDescriptor.Remove (descriptor);

              context.SaveChanges (); // update

              action.Result = TValidationResult.Success;
            }
          }
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Remove", exception, action);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace