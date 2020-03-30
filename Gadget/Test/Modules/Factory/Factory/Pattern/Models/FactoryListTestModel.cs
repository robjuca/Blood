/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Action;
using Server.Models.Component;
using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListTestModel
  {
    #region Property
    public ObservableCollection<GadgetTest> GadgetItemsSource
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
        return (GadgetCheckedCollection.Any ());
      }
    }

    public bool IsEditMode
    {
      get
      {
        return (m_CurrentEditGadget.ValidateId);
      }
    }
    #endregion

    #region Constructor
    public TFactoryListTestModel ()
    {
      GadgetItemsSource = new ObservableCollection<GadgetTest> ();

      GadgetFullCollection = new Collection<GadgetTest> ();
      GadgetCheckedCollection = new Collection<GadgetTest> ();

      m_CurrentMaterialGadget = GadgetMaterial.CreateDefault;
      m_CurrentEditGadget = GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Collection<TActionComponent> gadgets)
    {
      if (gadgets.NotNull ()) {
        Cleanup ();

        foreach (var gadget in gadgets) {
          GadgetFullCollection.Add (gadget.Models.GadgetTestModel);
        }

        MaterialChanged ();
      }
    }

    internal void MaterialItemChanged (TActionComponent component)
    {
      if (component.IsCategory(TCategory.Material)) {
        var gadget = component.Models.GadgetMaterialModel;

        if (gadget.ValidateId) {
          if (m_CurrentMaterialGadget.Contains (gadget.Id).IsFalse ()) {
            m_CurrentMaterialGadget.CopyFrom (gadget);

            MaterialChanged ();
          }
        }
      }
    }

    internal void GadgetItemChecked (GadgetTest gadget, bool isChecked)
    {
      if (gadget.NotNull ()) {
        gadget.IsChecked = isChecked; // for sure

        var gadgetChecked = IsChecked (gadget);

        if (isChecked) {
          if (gadgetChecked.ValidateId.IsFalse ()) {
            AddChecked (gadget);
          }
        }

        else {
          if (gadgetChecked.ValidateId) {
            RemoveChecked (gadget.Id);
          }
        }
      }
    }
    
    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      foreach (var item in GadgetCheckedCollection) {
        // ensure ChildId diferent from ParentId
        if (item.Id.NotEquals (action.Id)) {
          var componentRelation = ComponentRelation.CreateDefault;
          componentRelation.ChildId = item.Id;
          componentRelation.ChildCategory = TCategoryType.ToValue (TCategory.Test);
          componentRelation.ParentId = action.Id;
          componentRelation.ParentCategory = TCategoryType.ToValue (action.CategoryType.Category);

          // Extension 
          if (string.IsNullOrEmpty (action.ModelAction.ExtensionTextModel.Extension)) {
            action.ModelAction.ExtensionTextModel.Extension = m_CurrentMaterialGadget.GadgetName;
          }

          action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
        }
      }

      // update rule
      action.SupportAction.Rule.Pump ("gadget");
    }

    internal void Edit (TActionComponent component)
    {
      component.ThrowNull ();

      Cleanup ();

      m_CurrentEditGadget.CopyFrom (component.Models.GadgetTestModel);

      // found
      if (m_CurrentEditGadget.ValidateId) {
        if (m_CurrentEditGadget.Material.Equals (m_CurrentMaterialGadget.Material, StringComparison.InvariantCulture)) {
          MaterialChanged ();
        }
      }
    }

    internal void Cleanup ()
    {
      GadgetItemsSource.Clear ();
      GadgetCheckedCollection.Clear ();

      m_CurrentEditGadget.CopyFrom (GadgetTest.CreateDefault);
    }
    #endregion

    #region property
    Collection<GadgetTest> GadgetFullCollection
    {
      get;
    }

    Collection<GadgetTest> GadgetCheckedCollection
    {
      get;
    }
    #endregion

    #region Fields
    readonly GadgetMaterial                                               m_CurrentMaterialGadget;
    readonly GadgetTest                                                   m_CurrentEditGadget;
    #endregion

    #region Support
    GadgetTest IsChecked (GadgetTest gadget)
    {
      if (gadget.NotNull ()) {
        foreach (var gadgetChecked in GadgetCheckedCollection) {
          if (gadgetChecked.Contains (gadget.Id)) {
            return (gadgetChecked);
          }
        }
      }

      return (GadgetTest.CreateDefault);
    }

    GadgetTest IsChecked (Guid id)
    {
      foreach (var componentChecked in GadgetCheckedCollection) {
        if (componentChecked.Contains (id)) {
          return (componentChecked);
        }
      }

      return (GadgetTest.CreateDefault);
    }

    bool ContainsChecked (Guid id)
    {
      foreach (var componentChecked in GadgetCheckedCollection) {
        if (componentChecked.Contains (id)) {
          return (true);
        }
      }

      return (false);
    }

    bool AddGadget (GadgetTest gadget)
    {
      var res = false;

      if (gadget.NotNull ()) {
        if (ContainsGadget (gadget.Id).IsFalse ()) {
          GadgetItemsSource.Add (gadget);
          res = true;
        }
      }

      return (res);
    }

    bool ContainsGadget (Guid id)
    {
      foreach (var gadget in GadgetItemsSource) {
        if (gadget.Contains (id)) {
          return (true);
        }
      }

      return (false);
    }

    void AddChecked (GadgetTest gadget)
    {
      if (gadget.NotNull ()) {
        if (ContainsChecked (gadget.Id).IsFalse ()) {
          GadgetCheckedCollection.Add (gadget);
        }

        else {
          IsChecked (gadget).IsChecked = true;
        }

        var itemSource = ItemSourceById (gadget.Id);

        if (itemSource.ValidateId) {
          itemSource.IsChecked = true; // for sure
        }
      }
    }

    void RemoveChecked (Guid id)
    {
      foreach (var componentChecked in GadgetCheckedCollection) {
        if (componentChecked.Contains (id)) {
          GadgetCheckedCollection.Remove (componentChecked);
          break;
        }
      }

      var itemSource = ItemSourceById (id);

      if (itemSource.ValidateId) {
        itemSource.IsChecked = false; // for sure
      }
    }

    GadgetTest GadgetById (Guid id)
    {
      foreach (var item in GadgetFullCollection) {
        if (item.Contains (id)) {
          return (item);
        }
      }

      return (GadgetTest.CreateDefault);
    }

    GadgetTest ItemSourceById (Guid id)
    {
      foreach (var item in GadgetItemsSource) {
        if (item.Contains (id)) {
          return (item);
        }
      }

      return (GadgetTest.CreateDefault);
    }

    void SortItemsSourceCollection ()
    {
      var list = GadgetItemsSource
        .OrderBy (p => p.GadgetInfo)
        .ToList ()
      ;

      GadgetItemsSource.Clear ();

      foreach (var item in list) {
        GadgetItemsSource.Add (item);
      }
    }

    void UpdateCurrentEditGadget ()
    {
      if (m_CurrentEditGadget.ValidateId) {
        // remove my self
        var itemSource = ItemSourceById (m_CurrentEditGadget.Id);

        if (itemSource.ValidateId) {
          GadgetItemsSource.Remove (itemSource);
        }

        if (m_CurrentEditGadget.HasContentTest) {
          var contents = new Collection<GadgetTest> ();
          m_CurrentEditGadget.RequestContent (contents);

          foreach (var gadget in contents) {
            var gadgetTest = GadgetById (gadget.Id);

            if (gadgetTest.ValidateId) {
              gadgetTest.Material = m_CurrentMaterialGadget.Material;

              if (AddGadget (gadgetTest)) {
                AddChecked (gadgetTest);
              }
            }
          }
        }

        else {
          //AddChecked (TFactoryListItemInfo.Create (gadgetItem, isChecked: true));
        }
      }
    }

    void MaterialChanged ()
    {
      if (m_CurrentMaterialGadget.ValidateId) {
        GadgetItemsSource.Clear ();

        foreach (var gadgetTest in GadgetFullCollection) {
          if (gadgetTest.Material.Equals (m_CurrentMaterialGadget.Material, StringComparison.InvariantCulture)) {
            var checkedItem = IsChecked (gadgetTest.Id);

            if (checkedItem.ValidateId.IsFalse ()) {
              if (gadgetTest.Enabled) {
                if (gadgetTest.Busy.IsFalse ()) {
                  // only Target content
                  if (gadgetTest.HasContentTarget) {
                    gadgetTest.Material = m_CurrentMaterialGadget.Material;

                    AddGadget (gadgetTest);
                  }
                }
              }
            }

            else {
              AddGadget (checkedItem);
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
