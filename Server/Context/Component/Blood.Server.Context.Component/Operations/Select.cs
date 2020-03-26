/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using rr.Library.Helper;

using Server.Models.Infrastructure;
using Server.Models.Action;
//---------------------------//

namespace Server.Context.Component
{
  public sealed class TOperationSelect : IOperation
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
              case TExtension.Settings: {
                  SelectSettings (context, action);
                }
                break;

              case TExtension.ById: {
                  SelectById (context, action);
                }
                break;

              case TExtension.Relation: {
                  SelectRelation (context, action);
                }
                break;

              case TExtension.Many: {
                  SelectMany (context, action);
                }
                break;

              case TExtension.Node: {
                  SelectNode (context, action);
                }
                break;

              case TExtension.Zap: {
                  SelectZap (context, action);
                }
                break;

              case TExtension.Summary: {
                  SelectSummary (context, action);
                }
                break;

              case TExtension.Idle: {
                  //SelectIdle (context, action);
                }
                break;

              case TExtension.Full: {
                  Server.Models.Infrastructure.THelper.FormatExtensionNotImplementedException (action);
                }
                break;
            }
          }

          else {
            Server.Models.Infrastructure.THelper.FormatExtensionMustExistException (action);
          }
        }
      }
    }
    #endregion

    #region Support
    void SelectSettings (TModelContext context, TEntityAction action)
    {
      try {
        var modelList = context.Settings
          .ToList ()
        ;

        if (modelList.Count.Equals (1)) {
          var model = modelList [0];

          if (model.MyName.Equals ("robjuca", StringComparison.InvariantCulture)) {
            action.ModelAction.SettingsModel.CopyFrom (model);

            action.Result = TValidationResult.Success;
          }

          // bad name
          else {
            action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Settings] My Name can NOT be NULL or EMPTY or VALIDATE!");
          }
        }

        // wrong record count
        else {
          action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Settings] Wrong record count!");
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select Settings", exception, action);
      }
    }

    void SelectById (TModelContext context, TEntityAction action)
    {
      /*
      DATA IN
      - action.Id 
      - action.CollectionAction.CategoryRelationCollection

      DATA OUT
      - action.ModelAction (model)
      - action.CollectionAction.ModeCollection {id, model} (for each node)
      - action.CollectionAction.EntityCollection {id, model} (for each relation)
      */

      try {
        // Id must exist
        if (action.Id.IsEmpty ()) {
          action.Result = new TValidationResult ("[Select ById] Id can NOT be NULL or EMPTY!");
        }

        else {
          action.Result = TValidationResult.Success; // desired result DO NOT MOVE FROM HERE

          // relation by id (use parent)
          action.CollectionAction.SelectComponentOperation (TComponentOperation.TInternalOperation.Id);
          action.ComponentOperation.SelectById (action.Id);

          var operationSupport = new TOperationSupport (context, action);
          operationSupport.RequestComponent (context, action);
          operationSupport.RequestExtension (context, action);
          TOperationSupport.RequestNode (context, action);
          TOperationSupport.RequestRelation (context, action);

          // use Parent relation
          if (action.ComponentOperation.ParentIdCollection.ContainsKey (action.Id)) {
            var componentRelationList = action.ComponentOperation.ParentIdCollection [action.Id];

            foreach (var relation in componentRelationList) {
              var entityAction = TEntityAction.Create (TCategoryType.FromValue (relation.ChildCategory));
              entityAction.CollectionAction.SetCollection (action.CollectionAction.CategoryRelationCollection);
              entityAction.Id = relation.ChildId;

              SelectById (context, entityAction); // my self (tree navigation)

              action.CollectionAction.EntityCollection.Add (relation.ChildId, entityAction);
            }
          }
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select ById", exception, action);
      }
    }

    void SelectNode (TModelContext context, TEntityAction action)
    {
      /*
      DATA IN
      - action.CollectionAction.CategoryRelationCollection
      - action.ComponentModel (NodeModelCollection)

      DATA OUT
      - action.CollectionAction.ModelCollection {id, model}

      */

      try {
        TOperationSupport.RequestNode (context, action);

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select Node", exception, action);
      }
    }

    void SelectRelation (TModelContext context, TEntityAction action)
    {
      /*
       DATA IN
      - action.ComponentOperation 

      DATA OUT
      - action.ComponentOperation
      */

      try {
        var operationSupport = new TOperationSupport (context, action);
        TOperationSupport.RequestRelation (context, action);

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select Relation", exception, action);
      }
    }

    void SelectZap (TModelContext context, TEntityAction action)
    {
      /* 
       DATA IN:
        action.IdCollection {id to zap}
        action.CategoryType  

       DATA OUT:
        action.CollectionAction.ModelCollection [id] {Model}
      */

      try {
        action.CollectionAction.ModelCollection.Clear ();

        var categoryValue = TCategoryType.ToValue (action.CategoryType.Category);

        // search Id by category
        var descriptors = context.ComponentDescriptor.AsQueryable()
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        // zap
        foreach (var idToZap in action.IdCollection) {
          var zapList = descriptors
            .Where (p => p.Id.Equals (idToZap))
            .ToList ()
          ;

          // found
          if (zapList.Count.Equals (1)) {
            // update descriptors
            descriptors.Remove (zapList [0]);
          }
        }

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
                //case TComponentExtensionNames.Document: {
                //    var list = context.ExtensionDocument
                //      .Where (p => p.Id.Equals (id))
                //      .ToList ()
                //    ;

                //    if (list.Count.Equals (1)) {
                //      action.CollectionAction.ExtensionDocumentCollection.Add (list [0]);
                //    }
                //  }
                //  break;

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
                    var list = context.ExtensionImage.AsQueryable().Where (p => p.Id.Equals (id)).ToList ();

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
                    // child first
                    var childList = context.ExtensionNode.AsQueryable()
                      .Where (p => p.ChildId.Equals (id))
                      .ToList ()
                    ;

                    if (childList.Count.Equals (1)) {
                      action.ModelAction.ExtensionNodeModel.CopyFrom (childList [0]);
                      action.CollectionAction.ExtensionNodeCollection.Add (childList [0]);
                    }

                    else {
                      // parent next
                      var parentList = context.ExtensionNode.AsQueryable()
                        .Where (p => p.ParentId.Equals (id))
                        .ToList ()
                      ;

                      foreach (var model in parentList) {
                        action.CollectionAction.ExtensionNodeCollection.Add (model);
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
                  // child
                  var list = action.CollectionAction.ExtensionNodeCollection
                    .Where (p => p.ChildId.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    models.ExtensionNodeModel.CopyFrom (list [0]);
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

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select Zap", exception, action);
      }
    }

    void SelectMany (TModelContext context, TEntityAction action)
    {
      /*
       DATA IN
      - action.IdCollection (id) or action.IdDictionaryCollection
      - action.CollectionAction.CategoryRelationCollection

      DATA OUT
      - action.CollectionAction.EntityCollection {id, entityAction} or action.CollectionAction.EntityDictionaryCollection {id, Collection<TEntityAction>} 
      */

      try {
        action.CollectionAction.ModelCollection.Clear ();
        action.CollectionAction.EntityCollection.Clear ();
        action.CollectionAction.EntityDictionary.Clear ();

        // IdCollection
        if (action.IdCollection.Any ()) {
          foreach (var id in action.IdCollection) {
            var entityAction = TEntityAction.CreateDefault;
            entityAction.Id = id;
            entityAction.CollectionAction.SetCollection (action.CollectionAction.CategoryRelationCollection);

            SelectById (context, entityAction);

            action.CollectionAction.EntityCollection.Add (id, entityAction);
          }
        }

        else {
          // IdDictionary
          if (action.IdDictionary.Any ()) {
            foreach (var item in action.IdDictionary) {
              var entityCollection = new Dictionary<Guid, TEntityAction> ();

              foreach (var id in item.Value) {
                var entityAction = TEntityAction.CreateDefault;
                entityAction.Id = id;
                entityAction.CollectionAction.SetCollection (action.CollectionAction.CategoryRelationCollection);

                SelectById (context, entityAction);

                entityCollection.Add (id, entityAction);
              }
              
              action.CollectionAction.EntityDictionary.Add (item.Key, entityCollection);
            }
          }
        }

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select Many", exception, action);
      }
    }

    void SelectSummary (TModelContext context, TEntityAction action)
    {
      /*
       DATA IN
      - action.Summary

      DATA OUT
      - action.Summary
      - action.CollectionAction.ComponentInfoCollection
      - action.CollectionAction.ComponentStatusCollection
      */

      try {
        action.CollectionAction.ComponentInfoCollection.Clear ();
        action.CollectionAction.ComponentStatusCollection.Clear ();

        var categoryValue = TCategoryType.ToValue (action.SupportAction.SummaryInfo.Category);

        // search Id by category
        var descriptors = context.ComponentDescriptor.AsQueryable()
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        foreach (var descriptor in descriptors) {
          // Info
          var infoList = context.ComponentInfo.AsQueryable()
            .Where (p => p.Id.Equals (descriptor.Id))
            .ToList ()
          ;

          // info found
          if (infoList.Count.Equals (1)) {
            var infoModel = infoList [0];
            action.CollectionAction.ComponentInfoCollection.Add (infoModel);
          }

          // Status
          var statusList = context.ComponentStatus.AsQueryable()
            .Where (p => p.Id.Equals (descriptor.Id))
            .ToList ()
          ;

          // status found
          if (statusList.Count.Equals (1)) {
            var statusModel = statusList [0];
            action.CollectionAction.ComponentStatusCollection.Add (statusModel);
          }
        }

        // apply zap
        var enabledZap = new Collection<Guid> ();
        var busyZap = new Collection<Guid> ();

        // zap Enabled = false (disable)
        if (action.SupportAction.SummaryInfo.ZapDisable) {
          foreach (var itemInfo in action.CollectionAction.ComponentInfoCollection) {
            // only Enabled
            if (itemInfo.Enabled.IsFalse ()) {
              enabledZap.Add (itemInfo.Id);
            }
          }
        }

        // zap Busy = true
        if (action.SupportAction.SummaryInfo.ZapBusy) {
          foreach (var itemStatus in action.CollectionAction.ComponentStatusCollection) {
            // only not busy
            if (itemStatus.Busy) {
              busyZap.Add (itemStatus.Id);
            }
          }
        }

        foreach (var id in enabledZap) {
          var list = action.CollectionAction.ComponentInfoCollection
            .Where (p => p.Id.Equals (id))
            .ToList ()
          ;

          if (list.Count.Equals (1)) {
            action.CollectionAction.ComponentInfoCollection.Remove (list [0]);
          }
        }

        foreach (var id in busyZap) {
          var list = action.CollectionAction.ComponentInfoCollection
            .Where (p => p.Id.Equals (id))
            .ToList ()
          ;

          if (list.Count.Equals (1)) {
            action.CollectionAction.ComponentInfoCollection.Remove (list [0]);
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
            action.CollectionAction.ExtensionLayoutCollection.Clear ();

            // check for style only
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

            foreach (var layout in action.CollectionAction.ExtensionLayoutCollection) {
              action.SupportAction.SummaryInfo.Select (layout.StyleHorizontal, layout.StyleVertical);
            }
          }
        }

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select Summary", exception, action);
      }
    }
    #endregion

    #region Support OLD






    //void SelectIdle (TModelContext context, TEntityAction action)
    //{
    //  /* 
    //   DATA OUT:
    //    action.CollectionAction.ModelCollection [id] {info (enable=false), layout, image}
    //  */

    //  try {
    //    action.CollectionAction.ModelCollection.Clear ();

    //    // bag must have Enabled=false to be idle
    //    var bagInfoList = context.BagInfo
    //      .Where (p => p.Enabled.Equals (false))
    //      .OrderBy (p => p.Name)
    //    ;

    //    var bagLayoutList = context.BagLayout
    //      .ToList ()
    //    ;

    //    var bagImageList = context.BagImage
    //      .ToList ()
    //    ;

    //    // info
    //    foreach (var model in bagInfoList) {
    //      var modelAction = Models.Module.Bag.TModelAction.CreateDefault;
    //      modelAction.BagInfo.CopyFrom (model);

    //      action.CollectionAction.ModelCollection.Add (model.BagId, modelAction);
    //    }

    //    foreach (var item in action.CollectionAction.ModelCollection) {
    //      var id = item.Key;

    //      // layout
    //      var layoutList = bagLayoutList
    //        .Where (p => p.BagId.Equals (id))
    //        .ToList ()
    //      ;

    //      if (layoutList.Count.Equals (1)) {
    //        item.Value.BagLayout.CopyFrom (layoutList [0]);
    //      }

    //      // image
    //      var imageList = bagImageList
    //        .Where (p => p.BagId.Equals (id))
    //        .ToList ()
    //      ;

    //      if (imageList.Count.Equals (1)) {
    //        item.Value.BagImage.CopyFrom (imageList [0]);
    //      }
    //    }

    //    action.Result = TValidationResult.Success;
    //  }

    //  catch (Exception exception) {
    //    Server.Models.Infrastructure.THelper.FormatException ("Select Idle", exception, action);
    //  }
    //}
    #endregion
  };
  //---------------------------//

}  // namespace