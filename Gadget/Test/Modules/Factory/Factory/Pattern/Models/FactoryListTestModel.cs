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
  public class TFactoryListTestModel
  {
    #region Property
    public ObservableCollection<TFactoryListItemInfo> TestItemsSource
    {
      get;
    }

    public string TestCount
    {
      get
      {
        return ($"[ {TestItemsSource.Count} ]");
      }
    }
    #endregion

    #region Constructor
    public TFactoryListTestModel ()
    {
      TestItemsSource = new ObservableCollection<TFactoryListItemInfo> ();

      ItemInfoCheckedCollection = new Collection<TFactoryListItemInfo> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      TestItemsSource.Clear ();

      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        action.ModelAction.CopyFrom (modelAction.Value);

        var model = action.ModelAction.GadgetTestModel;
        model.CopyFrom (action); // set gadget model

        var gadgetItem = TComponentModelItem.Create (action);

        var checkedItem = IsChecked (gadgetItem.Id);

        if (checkedItem.IsEmpty) {
          if (gadgetItem.Enabled) {
            if (gadgetItem.Busy.IsFalse ()) {
              TestItemsSource.Add (TFactoryListItemInfo.Create (gadgetItem));
            }
          }
        }

        else {
          TestItemsSource.Add (checkedItem);
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
      TestItemsSource.Clear ();
    }
    #endregion

    #region property
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
