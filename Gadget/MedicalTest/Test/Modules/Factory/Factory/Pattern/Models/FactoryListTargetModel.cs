/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListTargetModel
  {
    #region Property
    public ObservableCollection<TFactoryListItemInfo> GadgetItemsSource
    {
      get;
    }

    public string GadgetCount
    {
      get
      {
        return ($"[{GadgetItemsSource.Count}] [{GadgetCheckedCollection.Count}]");
      }
    }
    #endregion

    #region Constructor
    public TFactoryListTargetModel ()
    {
      GadgetItemsSource = new ObservableCollection<TFactoryListItemInfo> ();

      GadgetFullCollection = new Collection<TComponentModelItem> ();
      GadgetCheckedCollection = new Collection<TFactoryListItemInfo> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      GadgetFullCollection.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetTargetCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetTargetModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        GadgetFullCollection.Add (TComponentModelItem.Create (action));
      }
    }

    internal void MaterialItemChanged (Guid materialId)
    {
      GadgetItemsSource.Clear ();

      foreach (var gadgetItem in GadgetFullCollection) {
        if (gadgetItem.GadgetTargetModel.MaterialId.Equals (materialId)) {
          var checkedItem = IsChecked (gadgetItem.Id);

          if (checkedItem.IsEmpty) {
            if (gadgetItem.Enabled) {
              if (gadgetItem.Busy.IsFalse ()) {
                GadgetItemsSource.Add (TFactoryListItemInfo.Create (gadgetItem));
              }
            }
          }

          else {
            GadgetItemsSource.Add (checkedItem);
          }
        }
      }
    }

    //internal void RefreshModel (Server.Models.Component.TEntityAction action)
    //{
    //  // for gadget Material
    //  GadgetSelectionItemsSource.Clear ();

    //  var list = action.CollectionAction.GadgetMaterialCollection
    //    .OrderBy (p => p.Material)
    //    .ToList ()
    //  ;

    //  foreach (var gadget in list) {
    //    if (gadget.Enabled) {
    //      var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
    //      modelAction.GadgetMaterialModel.CopyFrom (gadget);

    //      action.ModelAction.CopyFrom (modelAction);

    //      GadgetSelectionItemsSource.Add (TComponentModelItem.Create (action));

    //      foreach (var item in GadgetFullCollection) {
    //        // Node reverse here
    //        if (item.NodeModel.ParentId.Equals (gadget.Id)) {
    //          item.GadgetMaterialModel.CopyFrom (gadget);
    //        }
    //      }
    //    }
    //  }

    //  if (GadgetSelectionItemsSource.Count > 0) {
    //    GadgetSelectionCurrent = GadgetSelectionItemsSource [0];
    //  }

    //  GadgetSelectionEnabled = GadgetCheckedCollection.Count.Equals (0);
    //}

    internal void GadgetItemChecked (TFactoryListItemInfo itemInfo)
    {
      if (itemInfo.NotNull ()) {
        var item = IsChecked (itemInfo);

        if (itemInfo.IsChecked) {
          if (item.IsEmpty) {
            AddChecked (itemInfo);
          }
        }

        else {
          if (item.IsEmpty.IsFalse ()) {
            RemoveChecked (itemInfo.Id);
          }
        }
      }
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      foreach (var item in GadgetCheckedCollection) {
        var componentRelation = Server.Models.Component.ComponentRelation.CreateDefault;
        componentRelation.ChildId = item.Id;
        componentRelation.ChildCategory = item.CategoryValue;
        componentRelation.ParentId = action.Id;
        componentRelation.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

        action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
      }

      // Extension 
      if (string.IsNullOrEmpty (action.ModelAction.ExtensionTextModel.Extension)) {
        //action.ModelAction.ExtensionTextModel.Extension = GadgetSelectionCurrent.GadgetMaterialModel.Material;
      }

      // update rule
      action.SupportAction.Rule.Pump ("gadget");
    }

    internal void Edit (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      var gadgetTest = action.ModelAction.GadgetTestModel;

      // update gadget selection (material)
      //for (int index = 0; index < GadgetSelectionItemsSource.Count; index++) {
      //  if (GadgetSelectionItemsSource[index].GadgetMaterialModel.Material.Equals (gadgetTest.Material)) {
      //    GadgetSelectionSelectedIndex = index;
      //    break;
      //  }
      //}

      // found
      if (gadgetTest.Id.NotEmpty ()) {
        if (gadgetTest.RequestCategory ().Equals (Server.Models.Infrastructure.TCategory.Target)) {
          var contents = new Collection<Server.Models.Component.GadgetTarget> ();
          gadgetTest.RequestContent (contents);

          foreach (var gadgetContent in contents) {
            var item = GadgetById (gadgetContent.Id);

            if (item.Id.NotEmpty ()) {
              var itemInfo = TFactoryListItemInfo.Create (item, isChecked: true);

              GadgetItemsSource.Add (itemInfo);
              AddChecked (itemInfo);
            }
          }
        }
      }

      //GadgetSelectionEnabled = GadgetCheckedCollection.Count.Equals (0);
    }

    internal void Cleanup ()
    {
      GadgetCheckedCollection.Clear ();
    }
    #endregion

    #region property
    Collection<TComponentModelItem> GadgetFullCollection
    {
      get;
    }

    Collection<TFactoryListItemInfo> GadgetCheckedCollection
    {
      get;
    }
    #endregion

    #region Support
    TFactoryListItemInfo IsChecked (TFactoryListItemInfo itemInfo)
    {
      var item = TFactoryListItemInfo.CreateDefault;

      if (itemInfo.NotNull ()) {
        foreach (var itemInfoChecked in GadgetCheckedCollection) {
          if (itemInfoChecked.Contains (itemInfo)) {
            item.CopyFrom (itemInfo);
            break;
          }
        }
      }

      return (item);
    }

    TFactoryListItemInfo IsChecked (Guid id)
    {
      var item = TFactoryListItemInfo.CreateDefault;

      foreach (var itemInfoChecked in GadgetCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          item.CopyFrom (itemInfoChecked);
          break;
        }
      }

      return (item);
    }

    void AddChecked (TFactoryListItemInfo itemInfo)
    {
      GadgetCheckedCollection.Add (itemInfo);

      //GadgetSelectionEnabled = GadgetCheckedCollection.Count.Equals (0);
    }

    void RemoveChecked (Guid id)
    {
      foreach (var itemInfoChecked in GadgetCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          GadgetCheckedCollection.Remove (itemInfoChecked);
          break;
        }
      }

      //GadgetSelectionEnabled = GadgetCheckedCollection.Count.Equals (0);
    }

    TComponentModelItem GadgetById (Guid id)
    {
      var itemModel = TComponentModelItem.CreateDefault;

      foreach (var item in GadgetFullCollection) {
        if (item.Id.Equals (id)) {
          itemModel.CopyFrom (item);
          break;
        }
      }

      return (itemModel);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
