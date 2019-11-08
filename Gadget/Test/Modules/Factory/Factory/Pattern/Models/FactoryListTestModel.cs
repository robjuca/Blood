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
    public ObservableCollection<TFactoryListItemInfo> GadgetItemsSource
    {
      get;
    }

    public string GadgetItemsSourceCount
    {
      get
      {
        return ($"[ {GadgetItemsSource.Count} ]");
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

      // update rule
      action.SupportAction.Rule.Pump ("gadget");
    }

    internal void Edit (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      foreach (var gadgetTargetId in action.ModelAction.GadgetTestModel.Targets) {
        var gadgetItem = GadgetById (gadgetTargetId);

        // found
        if (gadgetItem.Id.NotEmpty ()) {
          var itemInfo = TFactoryListItemInfo.Create (gadgetItem);
          itemInfo.IsChecked = true;

          AddChecked (itemInfo);
          GadgetItemsSource.Add (itemInfo);
        }
      }

      // remove my self
      var id = action.ModelAction.GadgetTestModel.Id;

      foreach (var item in GadgetItemsSource) {
        if (item.Id.Equals (id)) {
          GadgetItemsSource.Remove (item);
          break;
        }
      }
    }

    internal void Cleanup ()
    {
      GadgetCheckedCollection.Clear ();
      GadgetItemsSource.Clear ();

      foreach (var item in GadgetFullCollection) {
        GadgetItemsSource.Add (TFactoryListItemInfo.Create (item));
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
    }

    void RemoveChecked (Guid id)
    {
      foreach (var itemInfoChecked in GadgetCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          GadgetCheckedCollection.Remove (itemInfoChecked);
          break;
        }
      }
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
