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
  public class TFactoryListTargetModel
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
    public TFactoryListTargetModel ()
    {
      GadgetItemsSource = new ObservableCollection<TGadgetTestComponent> ();

      GadgetFullCollection = new Collection<TGadgetTargetModel> ();
      GadgetCheckedCollection = new Collection<TGadgetTestComponent> ();

      m_CurrentMaterialGadget = GadgetMaterial.CreateDefault;
      m_CurrentEditGadget = GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection (Target collection)

      entityAction.ThrowNull ();

      Cleanup ();

      TGadgetTargetActionComponent.Select (GadgetFullCollection, entityAction);
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

    internal void GadgetItemChecked (TGadgetTestComponent gadgetComponent)
    {
      if (gadgetComponent.NotNull ()) {
        var component = IsChecked (gadgetComponent);

        if (gadgetComponent.IsChecked) {
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
        //var componentRelation = ComponentRelation.CreateDefault;
        //componentRelation.ChildId = item.Id;
        //componentRelation.ChildCategory = item.CategoryValue;
        //componentRelation.ParentId = action.Id;
        //componentRelation.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

        //action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
      }

      // Extension 
      if (string.IsNullOrEmpty (action.ModelAction.ExtensionTextModel.Extension)) {
        //action.ModelAction.ExtensionTextModel.Extension = m_CurrentMaterialName;
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
    Collection<TGadgetTargetModel> GadgetFullCollection
    {
      get;
    }

    Collection<TGadgetTestComponent> GadgetCheckedCollection
    {
      get;
    }
    #endregion

    #region Fields
    readonly GadgetMaterial                                     m_CurrentMaterialGadget;
    readonly GadgetTest                                         m_CurrentEditGadget;
    #endregion

    #region Support
    TGadgetTestComponent IsChecked (TGadgetTestComponent gadgetComponent)
    {
      var component = TGadgetTestComponent.CreateDefault;

      if (gadgetComponent.NotNull ()) {
        foreach (var itemInfoChecked in GadgetCheckedCollection) {
          if (itemInfoChecked.Contains (gadgetComponent)) {
            component.CopyFrom (gadgetComponent);
            break;
          }
        }
      }

      return (component);
    }

    TGadgetTestComponent IsChecked (Guid id)
    {
      var item = TGadgetTestComponent.CreateDefault;

      foreach (var itemInfoChecked in GadgetCheckedCollection) {
        if (itemInfoChecked.Contains (id)) {
          item.CopyFrom (itemInfoChecked);
          break;
        }
      }

      return (item);
    }

    void AddChecked (TGadgetTestComponent itemInfo)
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

    TGadgetTargetModel GadgetById (Guid id)
    {
      var itemModel = TGadgetTargetModel.CreateDefault;

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
      if (m_CurrentEditGadget.ValidateId) {
        if (m_CurrentEditGadget.RequestCategory ().Equals (TCategory.Target)) {
          var contents = new Collection<GadgetTarget> ();
          m_CurrentEditGadget.RequestContent (contents);

          foreach (var gadgetContent in contents) {
            var gadgetTarget = GadgetById (gadgetContent.Id);

            if (gadgetTarget.ValidateId) {
              var gadgetComponent = TGadgetTestComponent.CreateDefault;
              gadgetComponent.Select (m_CurrentMaterialGadget);
              gadgetComponent.Select (gadgetTarget.Model);
              gadgetComponent.Select (TCategory.Target);
              gadgetComponent.Check (true);

              GadgetItemsSource.Add (gadgetComponent);
              AddChecked (gadgetComponent);
            }
          }
        }
      }
    }

    void MaterialChanged ()
    {
      if (m_CurrentMaterialGadget.ValidateId) {
        GadgetItemsSource.Clear ();

        foreach (var gadgetTargetModel in GadgetFullCollection) {
          if (m_CurrentMaterialGadget.Contains (gadgetTargetModel.Model.MaterialId)) {
            var checkedItem = IsChecked (gadgetTargetModel.Id);

            if (checkedItem.IsEmpty) {
              if (gadgetTargetModel.Enabled) {
                if (gadgetTargetModel.Busy.IsFalse ()) {
                  var gadgetComponent = TGadgetTestComponent.CreateDefault;
                  gadgetComponent.Select (m_CurrentMaterialGadget);
                  gadgetComponent.Select (gadgetTargetModel.Model);
                  gadgetComponent.Select (TCategory.Target);
                  gadgetComponent.Check (true);

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
