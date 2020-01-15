/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Action;
using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListTestModel
  {
    #region Property
    public ObservableCollection<TGadgetTestComponent> GadgetItemsSource
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
      GadgetItemsSource = new ObservableCollection<TGadgetTestComponent> ();

      GadgetFullCollection = new Collection<TGadgetTestModel> ();
      GadgetCheckedCollection = new Collection<TGadgetTestComponent> ();

      m_CurrentMaterialGadget = GadgetMaterial.CreateDefault;
      m_CurrentEditGadget = GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection (Test collection)

      entityAction.ThrowNull ();

      Cleanup ();

      TGadgetTestActionComponent.Select (GadgetFullCollection, entityAction);

      MaterialChanged ();
    }

    internal void MaterialItemChanged (TGadgetMaterialModel gadget)
    {
      if (gadget.ValidateId) {
        if (gadget.Id.Equals (m_CurrentMaterialGadget.Id).IsFalse ()) {
          m_CurrentMaterialGadget.CopyFrom (gadget.Model);
         
          MaterialChanged ();
        }
      }
    }

    internal void GadgetItemChecked (TGadgetTestComponent gadgetComponent, bool isChecked)
    {
      if (gadgetComponent.NotNull ()) {
        gadgetComponent.IsChecked = isChecked;

        var component = IsChecked (gadgetComponent);

        if (isChecked) {
          if (component.IsEmpty) {
            AddChecked (gadgetComponent);
          }
        }

        else {
          if (component.IsEmpty.IsFalse ()) {
            RemoveChecked (gadgetComponent.Id);
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
          //var componentRelation = ComponentRelation.CreateDefault;
          //componentRelation.ChildId = item.Id;
          //componentRelation.ChildCategory = item.CategoryValue;
          //componentRelation.ParentId = action.Id;
          //componentRelation.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

          //// Extension 
          //if (string.IsNullOrEmpty (action.ModelAction.ExtensionTextModel.Extension)) {
          //  action.ModelAction.ExtensionTextModel.Extension = m_CurrentMaterialName;
          //}

          //action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
        }
      }

      // update rule
      action.SupportAction.Rule.Pump ("gadget");
    }

    internal void Edit (TGadgetTestModel gadget)
    {
      gadget.ThrowNull ();

      Cleanup ();

      m_CurrentEditGadget.CopyFrom (gadget.Model);

      // found
      if (m_CurrentEditGadget.ValidateId) {
        if (m_CurrentEditGadget.Material.Equals (m_CurrentMaterialGadget.Material)) {
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
    Collection<TGadgetTestModel> GadgetFullCollection
    {
      get;
    }

    Collection<TGadgetTestComponent> GadgetCheckedCollection
    {
      get;
    }
    #endregion

    #region Fields
    readonly GadgetMaterial                                               m_CurrentMaterialGadget;
    readonly GadgetTest                                                   m_CurrentEditGadget;
    #endregion

    #region Support
    TGadgetTestComponent IsChecked (TGadgetTestComponent gadgetComponent)
    {
      if (gadgetComponent.NotNull ()) {
        foreach (var componentChecked in GadgetCheckedCollection) {
          if (componentChecked.Contains (gadgetComponent)) {
            return (componentChecked);
          }
        }
      }

      return (TGadgetTestComponent.CreateDefault);
    }

    TGadgetTestComponent IsChecked (Guid id)
    {
      foreach (var componentChecked in GadgetCheckedCollection) {
        if (componentChecked.Contains (id)) {
          return (componentChecked);
        }
      }

      return (TGadgetTestComponent.CreateDefault);
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

    void AddChecked (TGadgetTestComponent gadgetComponent)
    {
      if (gadgetComponent.NotNull ()) {
        if (ContainsChecked (gadgetComponent.Id).IsFalse ()) {
          GadgetCheckedCollection.Add (gadgetComponent);
        }

        else {
          IsChecked (gadgetComponent).Check (true);
        }

        var itemSource = ItemSourceById (gadgetComponent.Id);

        if (itemSource.ValidateId) {
          itemSource.Check (true); // for sure
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
        itemSource.Check (false); // for sure
      }
    }

    TGadgetTestModel GadgetById (Guid id)
    {
      foreach (var item in GadgetFullCollection) {
        if (item.Contains (id)) {
          return (item);
        }
      }

      return (TGadgetTestModel.CreateDefault);
    }

    TGadgetTestComponent ItemSourceById (Guid id)
    {
      foreach (var item in GadgetItemsSource) {
        if (item.Contains (id)) {
          return (item);
        }
      }

      return (TGadgetTestComponent.CreateDefault);
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

        if (itemSource.ValidateId) {
          GadgetItemsSource.Remove (itemSource);
        }

        if (m_CurrentEditGadget.RequestCategory ().Equals (TCategory.Test)) {
          var contents = new Collection<GadgetTest> ();
          m_CurrentEditGadget.RequestContent (contents);

          foreach (var gadgetContent in contents) {
            var gadgetTest = GadgetById (gadgetContent.Id);

            if (gadgetTest.ValidateId) {
              var gadgetComponent = TGadgetTestComponent.CreateDefault;
              gadgetComponent.Select (m_CurrentMaterialGadget);
              gadgetComponent.Select (gadgetTest.Model);
              gadgetComponent.Select (TCategory.Test);

              GadgetItemsSource.Add (gadgetComponent);
              AddChecked (gadgetComponent);
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
          if (gadgetTest.Model.Material.Equals (m_CurrentMaterialGadget.Material)) {
            var checkedItem = IsChecked (gadgetTest.Id);

            if (checkedItem.IsEmpty) {
              if (gadgetTest.Enabled) {
                if (gadgetTest.Busy.IsFalse ()) {
                  var gadgetComponent = TGadgetTestComponent.CreateDefault;
                  gadgetComponent.Select (m_CurrentMaterialGadget);
                  gadgetComponent.Select (gadgetTest.Model);
                  gadgetComponent.Select (TCategory.Test);

                  GadgetItemsSource.Add (gadgetComponent);
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
