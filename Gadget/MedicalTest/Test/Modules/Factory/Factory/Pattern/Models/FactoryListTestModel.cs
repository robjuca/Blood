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
    public TFactoryListTestModel ()
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
      // action.CollectionAction.ModelCollection (Test collection)

      action.ThrowNull ();

      Cleanup ();
      GadgetFullCollection.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetTestCollection) {
        if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetTestModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          GadgetFullCollection.Add (TComponentModelItem.Create (action));
        }
      }

      MaterialChanged ();
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
        // ensure ChildId diferent from ParentId
        if (item.Id.NotEquals (action.Id)) {
          var componentRelation = Server.Models.Component.ComponentRelation.CreateDefault;
          componentRelation.ChildId = item.Id;
          componentRelation.ChildCategory = item.CategoryValue;
          componentRelation.ParentId = action.Id;
          componentRelation.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

          // Extension 
          if (string.IsNullOrEmpty (action.ModelAction.ExtensionTextModel.Extension)) {
            action.ModelAction.ExtensionTextModel.Extension = m_CurrentMaterialName;
          }

          action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
        }
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
    Guid                                                        m_CurrentMaterialId;
    string                                                      m_CurrentMaterialName;
    Server.Models.Component.GadgetTest                          m_CurrentEditGadget;
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
        // remove my self
        var itemSource = ItemSourceById (m_CurrentEditGadget.Id);

        if (itemSource.Id.NotEmpty ()) {
          GadgetItemsSource.Remove (itemSource);
        }

        if (m_CurrentEditGadget.RequestCategory ().Equals (Server.Models.Infrastructure.TCategory.Test)) {
          var contents = new Collection<Server.Models.Component.GadgetTest> ();
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

        //else {
        //  AddChecked (TFactoryListItemInfo.Create (gadgetItem, isChecked: true));
        //}
      }
    }

    void MaterialChanged ()
    {
      if (m_CurrentMaterialId.NotEmpty ()) {
        GadgetItemsSource.Clear ();

        foreach (var gadgetItem in GadgetFullCollection) {
          if (gadgetItem.GadgetTestModel.Material.Equals (m_CurrentMaterialName)) {
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
