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
    public ObservableCollection<TComponentModelItem> MaterialItemsSource
    {
      get;
    }

    public ObservableCollection<TFactoryListItemInfo> TargetItemsSource
    {
      get;
    }

    public int MaterialCount
    {
      get
      {
        return (MaterialItemsSource.Count);
      }
    }

    public int TargetCount
    {
      get
      {
        return (TargetItemsSource.Count);
      }
    }

    public TComponentModelItem MaterialSelected
    {
      get;
      set;
    }

    public int TargetSelectedIndex
    {
      get;
      set;
    }

    public TFactoryListItemInfo TargetCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TFactoryListTargetModel ()
    {
      MaterialItemsSource = new ObservableCollection<TComponentModelItem> ();
      TargetItemsSource = new ObservableCollection<TFactoryListItemInfo> ();

      TargetSelectedIndex = -1;

      TargetCurrent = TFactoryListItemInfo.CreateDefault;

      Targets = new Collection<TComponentModelItem> ();
      ItemInfoCheckedCollection = new Collection<TFactoryListItemInfo> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      Targets.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetTargetCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetTargetModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        Targets.Add (TComponentModelItem.Create (action));
      }
    }

    internal void RefreshModel (Server.Models.Component.TEntityAction action)
    {
      // for gadget Material
      MaterialItemsSource.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetMaterialCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetMaterialModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        MaterialItemsSource.Add (TComponentModelItem.Create (action));

        foreach (var item in Targets) {
          // Node reverse here
          if (item.NodeModel.ParentId.Equals (gadget.Id)) {
            item.GadgetMaterialModel.CopyFrom (gadget);
          }
        }
      }

      if (MaterialItemsSource.Count > 0) {
        MaterialSelected = MaterialItemsSource [0];
      }
    }

    internal void MaterialChanged (int selectdIndex)
    {
      TargetItemsSource.Clear ();

      if (selectdIndex.Equals (-1)) {
        TargetSelectedIndex = -1;
      }

      else {
        foreach (var gadgetItem in Targets) {
          if (gadgetItem.GadgetTargetModel.MaterialId.Equals (MaterialSelected.GadgetMaterialModel.Id)) {
            var checkedItem = IsChecked (gadgetItem.Id);

            if (checkedItem.IsEmpty) {
              if (gadgetItem.Enabled) {
                if (gadgetItem.Busy.IsFalse ()) {
                  TargetItemsSource.Add (TFactoryListItemInfo.Create (gadgetItem));
                }
              }
            }

            else {
              TargetItemsSource.Add (checkedItem);
            }
          }
        }

        if (TargetCount > 0) {
          TargetSelectedIndex = 0;
        }
      }
    }

    internal void ItemInfoChecked (TFactoryListItemInfo itemInfo, bool isChecked)
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

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      foreach (var item in ItemInfoCheckedCollection) {
        var componentRelation = Server.Models.Component.ComponentRelation.CreateDefault;
        componentRelation.ChildId = item.Id;
        componentRelation.ChildCategory = item.CategoryValue;
        componentRelation.ParentId = action.Id;
        componentRelation.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

        action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
      }
    }

    internal void Cleanup ()
    {
      ItemInfoCheckedCollection.Clear ();
      TargetItemsSource.Clear ();

      MaterialChanged (-1);
    }
    #endregion

    #region property
    Collection<TComponentModelItem> Targets
    {
      get;
    }

    Collection<TFactoryListItemInfo> ItemInfoCheckedCollection
    {
      get;
    }
    #endregion

    #region Support
    TFactoryListItemInfo IsChecked (TFactoryListItemInfo itemInfo)
    {
      var item = TFactoryListItemInfo.CreateDefault;

      if (itemInfo.NotNull ()) {
        foreach (var itemInfoChecked in ItemInfoCheckedCollection) {
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

      foreach (var itemInfoChecked in ItemInfoCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          item.CopyFrom (itemInfoChecked);
          break;
        }
      }

      return (item);
    }

    void AddChecked (TFactoryListItemInfo itemInfo)
    {
      ItemInfoCheckedCollection.Add (itemInfo);
    }

    void RemoveChecked (Guid id)
    {
      foreach (var itemInfoChecked in ItemInfoCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          ItemInfoCheckedCollection.Remove (itemInfoChecked);
          break;
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
