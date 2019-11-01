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
  public class TFactoryListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> MaterialItemsSource
    {
      get;
    }

    public ObservableCollection<TItemInfo> TargetItemsSource
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

    public TItemInfo TargetCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TFactoryListModel ()
    {
      MaterialItemsSource = new ObservableCollection<TComponentModelItem> ();
      TargetItemsSource = new ObservableCollection<TItemInfo> ();

      TargetSelectedIndex = -1;

      TargetCurrent = TItemInfo.CreateDefault;

      Targets = new Collection<TComponentModelItem> ();
      ItemInfoCheckedCollection = new Collection<TItemInfo> ();
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
        foreach (var itemTarget in Targets) {
          if (itemTarget.GadgetTargetModel.MaterialId.Equals (MaterialSelected.GadgetMaterialModel.Id)) {
            var item = IsChecked (itemTarget.Id);

            if (item.IsEmpty) {
              TargetItemsSource.Add (TItemInfo.Create (itemTarget));
            }

            else {
              TargetItemsSource.Add (item);
            }
          }
        }

        if (TargetCount > 0) {
          TargetSelectedIndex = 0;
        }
      }
    }

    internal void ItemInfoChecked (TItemInfo itemInfo, bool isChecked)
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
    #endregion

    #region property
    Collection<TComponentModelItem> Targets
    {
      get;
    }

    Collection<TItemInfo> ItemInfoCheckedCollection
    {
      get;
    }
    #endregion

    #region Support
    TItemInfo IsChecked (TItemInfo itemInfo)
    {
      var item = TItemInfo.CreateDefault;

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

    TItemInfo IsChecked (Guid id)
    {
      var item = TItemInfo.CreateDefault;

      foreach (var itemInfoChecked in ItemInfoCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          item.CopyFrom (itemInfoChecked);
          break;
        }
      }

      return (item);
    }

    void AddChecked (TItemInfo itemInfo)
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

  //----- TItemInfo
  public class TItemInfo
  {
    #region Property
    public TComponentModelItem ModelItem
    {
      get;
    }

    public Guid Id
    {
      get
      {
        return (ModelItem.Id);
      }
    }

    public bool IsEmpty
    {
      get
      {
        return (Id.Equals (Guid.Empty));
      }
    }

    public bool IsChecked
    {
      get; 
      set;
    }
    #endregion

    #region Constructor
    TItemInfo ()
    {
      ModelItem = TComponentModelItem.CreateDefault;
    }

    TItemInfo (TComponentModelItem item)
      : this ()
    {
      ModelItem.CopyFrom (item);
    }
    #endregion

    #region Members]
    internal bool Contains (Guid id)
    {
      return (Id.Equals (id));
    }

    internal bool Contains (TItemInfo alias)
    {
      return (alias.NotNull () ? Id.Equals (alias.Id) : false);
    }

    internal void CopyFrom (TItemInfo alias)
    {
      if (alias.NotNull ()) {
        ModelItem.CopyFrom (alias.ModelItem);
        IsChecked = alias.IsChecked;
      }
    }
    #endregion

    #region Static
    public static TItemInfo Create (TComponentModelItem item) => new TItemInfo (item);
    public static TItemInfo CreateDefault => new TItemInfo ();
    #endregion
  };
  //---------------------------//

}  // namespace
