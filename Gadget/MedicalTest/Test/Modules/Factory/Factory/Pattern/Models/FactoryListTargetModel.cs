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
        return ($"[{GadgetItemsSource.Count}] [{GadgetCheckedCount}]");
      }
    }

    public int GadgetCheckedCount
    {
      get
      {
        return (GadgetCheckedCollection.Count);
      }
    }

    public bool HasGadgetChecked
    {
      get
      {
        return (GadgetCheckedCount.Equals (0).IsFalse ());
      }
    }
    #endregion

    #region Constructor
    public TFactoryListTargetModel ()
    {
      GadgetItemsSource = new ObservableCollection<TFactoryListItemInfo> ();

      GadgetFullCollection = new Collection<TComponentModelItem> ();
      GadgetCheckedCollection = new Collection<TFactoryListItemInfo> ();

      m_CurrentMaterialId = Guid.Empty;
      m_CurrentMaterialName = string.Empty;
      m_CurrentEditGadget = Server.Models.Component.GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection (Target collection)

      action.ThrowNull ();

      Cleanup ();
      GadgetFullCollection.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetTargetCollection) {
        if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetTargetModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          GadgetFullCollection.Add (TComponentModelItem.Create (action));
        }
      }

      MaterialItemChanged (m_CurrentMaterialId, m_CurrentMaterialName);
    }

    internal void MaterialItemChanged (Guid materialId, string materialName)
    {
      if (materialId.IsEmpty ().IsFalse ()) {
        if (materialId.Equals (m_CurrentMaterialId).IsFalse ()) {
          m_CurrentMaterialId = materialId;
          m_CurrentMaterialName = materialName;

          MaterialChanged ();
        }
      }
    }

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

      Cleanup ();

      m_CurrentEditGadget.CopyFrom (action.ModelAction.GadgetTestModel);

      // found
      if (m_CurrentEditGadget.Id.NotEmpty ()) {
        if (m_CurrentEditGadget.Material.Equals (m_CurrentMaterialName)) {
          MaterialChanged ();
        }
      }
    }

    internal void Cleanup ()
    {
      GadgetItemsSource.Clear ();
      GadgetCheckedCollection.Clear ();

      m_CurrentEditGadget = Server.Models.Component.GadgetTest.CreateDefault;
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

    #region Fields
    Guid                                              m_CurrentMaterialId;
    string                                            m_CurrentMaterialName;
    Server.Models.Component.GadgetTest                m_CurrentEditGadget;
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

    void SortItemsSourceCollection ()
    {
      var list = GadgetItemsSource
        .OrderBy (p => p.Name)
        .ToList ()
      ;

      GadgetItemsSource.Clear ();

      foreach (var item in list) {
        GadgetItemsSource.Add (item);
      }
    }

    void UpdateCurrentEditGadget ()
    {
      if (m_CurrentEditGadget.Id.NotEmpty ()) {
        if (m_CurrentEditGadget.RequestCategory ().Equals (Server.Models.Infrastructure.TCategory.Target)) {
          var contents = new Collection<Server.Models.Component.GadgetTarget> ();
          m_CurrentEditGadget.RequestContent (contents);

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
    }

    void MaterialChanged ()
    {
      if (m_CurrentMaterialId.NotEmpty ()) {
        GadgetItemsSource.Clear ();

        foreach (var gadgetItem in GadgetFullCollection) {
          if (gadgetItem.GadgetTargetModel.MaterialId.Equals (m_CurrentMaterialId)) {
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

        UpdateCurrentEditGadget ();
        SortItemsSourceCollection ();
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
