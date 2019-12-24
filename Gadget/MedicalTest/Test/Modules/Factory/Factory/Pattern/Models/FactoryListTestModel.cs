/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListTestModel
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
    public TFactoryListTestModel ()
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
      GadgetItemsSource.Clear ();

      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        action.ModelAction.CopyFrom (modelAction.Value);

        var model = action.ModelAction.GadgetTestModel;
        model.CopyFrom (action); // set gadget model

        var gadgetItem = TComponentModelItem.Create (action);

        GadgetFullCollection.Add (gadgetItem);

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

    internal void GadgetItemChecked (TFactoryListItemInfo itemInfo, bool isChecked)
    {
      if (itemInfo.NotNull ()) {
        itemInfo.IsChecked = isChecked;

        var item = IsChecked (itemInfo);

        if (isChecked) {
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

    internal void MaterialItemChanged (string materialName)
    {
      GadgetItemsSource.Clear ();

      foreach (var gadgetItem in GadgetFullCollection) {
        if (gadgetItem.GadgetTestModel.Material.Equals (materialName)) {
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
      //  if (GadgetSelectionItemsSource [index].GadgetMaterialModel.Material.Equals (gadgetTest.Material)) {
      //    GadgetSelectionSelectedIndex = index;
      //    break;
      //  }
      //}

      var gadgetItem = GadgetById (gadgetTest.Id);

      // found
      if (gadgetItem.Id.NotEmpty ()) {
        if (gadgetItem.Category.Equals (Server.Models.Infrastructure.TCategory.Test)) {
          gadgetItem.GadgetTestModel.CopyFrom (action.ModelAction.GadgetTestModel);
          var gadget = gadgetItem.GadgetTestModel;

          if (gadget.RequestCategory ().Equals (Server.Models.Infrastructure.TCategory.Test)) {
            var contents = new Collection<Server.Models.Component.GadgetTest> ();
            gadget.RequestContent (contents);

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

        else {
          AddChecked (TFactoryListItemInfo.Create (gadgetItem, isChecked: true));
        }
      }

      // remove my self
      var itemSource = ItemSourceById (gadgetTest.Id);

      if (itemSource.Id.NotEmpty ()) {
        GadgetItemsSource.Remove (itemSource);
      }
    }

    internal void Cleanup ()
    {
      GadgetCheckedCollection.Clear ();
      GadgetItemsSource.Clear ();

      foreach (var gadgetItem in GadgetFullCollection) {
        if (gadgetItem.Enabled) {
          if (gadgetItem.Busy.IsFalse ()) {
            GadgetItemsSource.Add (TFactoryListItemInfo.Create (gadgetItem));
          }
        }
      }
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
      if (itemInfo.NotNull ()) {
        foreach (var itemInfoChecked in GadgetCheckedCollection) {
          if (itemInfoChecked.Contains (itemInfo)) {
            return (itemInfoChecked);
          }
        }
      }

      return (TFactoryListItemInfo.CreateDefault);
    }

    TFactoryListItemInfo IsChecked (Guid id)
    {
      foreach (var itemInfoChecked in GadgetCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          return (itemInfoChecked);
        }
      }

      return (TFactoryListItemInfo.CreateDefault);
    }

    bool ContainsChecked (Guid id)
    {
      foreach (var itemInfoChecked in GadgetCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          return (true);
        }
      }

      return (false);
    }

    void AddChecked (TFactoryListItemInfo itemInfo)
    {
      if (itemInfo.NotNull ()) {
        if (ContainsChecked (itemInfo.Id).IsFalse ()) {
          GadgetCheckedCollection.Add (itemInfo);
        }

        else {
          (IsChecked (itemInfo)).IsChecked = true;
        }

        var itemSource = ItemSourceById (itemInfo.Id);

        if (itemSource.Id.IsEmpty ().IsFalse ()) {
          itemSource.IsChecked = true; // for sure
        }
      }
    }

    void RemoveChecked (Guid id)
    {
      foreach (var itemInfoChecked in GadgetCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          GadgetCheckedCollection.Remove (itemInfoChecked);
          break;
        }
      }

      var itemSource = ItemSourceById (id);

      if (itemSource.Id.IsEmpty ().IsFalse ()) {
        itemSource.IsChecked = false; // for sure
      }
    }

    TComponentModelItem GadgetById (Guid id)
    {
      foreach (var item in GadgetFullCollection) {
        if (item.Id.Equals (id)) {
          return (item);
        }
      }

      return (TComponentModelItem.CreateDefault);
    }

    TFactoryListItemInfo ItemSourceById (Guid id)
    {
      foreach (var item in GadgetItemsSource) {
        if (item.Id.Equals (id)) {
          return (item);
        }
      }

      return (TFactoryListItemInfo.CreateDefault);
    }
    #endregion
  };
  //---------------------------//


}  // namespace
