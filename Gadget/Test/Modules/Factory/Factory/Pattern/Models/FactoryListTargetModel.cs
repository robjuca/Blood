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
  public class TFactoryListTargetModel
  {
    #region Property
    public ObservableCollection<GadgetTarget> GadgetItemsSource
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
    public TFactoryListTargetModel ()
    {
      GadgetItemsSource = new ObservableCollection<GadgetTarget> ();

      GadgetFullCollection = new Collection<GadgetTarget> ();
      GadgetCheckedCollection = new Collection<GadgetTarget> ();

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

      GadgetFullCollection.Clear ();

      var gadgets = new Collection<TActionComponent> ();
      TActionConverter.Collection (TCategory.Target, gadgets, entityAction);

      foreach (var gadget in gadgets) {
        GadgetFullCollection.Add (gadget.Models.GadgetTargetModel);
      }

      MaterialChanged ();
    }

    internal void MaterialItemChanged (TActionComponent component)
    {
      if (component.IsCategory (TCategory.Material)) {
        var gadget = component.Models.GadgetMaterialModel;

        if (gadget.ValidateId) {
          if (m_CurrentMaterialGadget.Contains (gadget.Id).IsFalse ()) {
            m_CurrentMaterialGadget.CopyFrom (gadget);

            MaterialChanged ();
          }
        }
      }
    }

    internal void GadgetItemChecked (GadgetTarget gadget)
    {
      if (gadget.NotNull ()) {
        if (gadget.IsChecked) {
          AddChecked (gadget);
        }

        else {
          RemoveChecked (gadget.Id);
        }
      }
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      foreach (var item in GadgetCheckedCollection) {
        var componentRelation = ComponentRelation.CreateDefault;
        componentRelation.ChildId = item.Id;
        componentRelation.ChildCategory = TCategoryType.ToValue (TCategory.Target);
        componentRelation.ParentId = action.Id;
        componentRelation.ParentCategory = TCategoryType.ToValue (action.CategoryType.Category);

        action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
      }

      // Extension 
      if (string.IsNullOrEmpty (action.ModelAction.ExtensionTextModel.Extension)) {
        action.ModelAction.ExtensionTextModel.Extension = m_CurrentMaterialGadget.GadgetName;
      }

      // update rule
      action.SupportAction.Rule.Pump ("gadget");
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetMaterialModel.CopyFrom (m_CurrentMaterialGadget);
      component.Models.GadgetTestModel.CopyFrom (m_CurrentEditGadget);
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
    Collection<GadgetTarget> GadgetFullCollection
    {
      get;
    }

    Collection<GadgetTarget> GadgetCheckedCollection
    {
      get;
    }
    #endregion

    #region Fields
    readonly GadgetMaterial                                     m_CurrentMaterialGadget;
    readonly GadgetTest                                         m_CurrentEditGadget;
    #endregion

    #region Support
    GadgetTarget IsChecked (GadgetTarget gadgetTarget)
    {
      var someGadgetTarget = GadgetTarget.CreateDefault;

      if (gadgetTarget.NotNull ()) {
        foreach (var gadgetTargetChecked in GadgetCheckedCollection) {
          if (gadgetTargetChecked.Contains (gadgetTarget.Id)) {
            someGadgetTarget.CopyFrom (gadgetTargetChecked);
            break;
          }
        }
      }

      return (someGadgetTarget);
    }

    GadgetTarget IsChecked (Guid id)
    {
      var someGadgetTarget = GadgetTarget.CreateDefault;

      foreach (var gadgetTargetChecked in GadgetCheckedCollection) {
        if (gadgetTargetChecked.Contains (id)) {
          someGadgetTarget.CopyFrom (gadgetTargetChecked);
          break;
        }
      }

      return (someGadgetTarget);
    }

    void AddChecked (GadgetTarget gadgetTarget)
    {
      GadgetCheckedCollection.Add (gadgetTarget);
    }

    void RemoveChecked (Guid id)
    {
      foreach (var gadgetTargetChecked in GadgetCheckedCollection) {
        if (gadgetTargetChecked.Contains (id)) {
          GadgetCheckedCollection.Remove (gadgetTargetChecked);
          break;
        }
      }
    }

    GadgetTarget GadgetById (Guid id)
    {
      var gadgetTarget = GadgetTarget.CreateDefault;

      foreach (var gadget in GadgetFullCollection) {
        if (gadget.Id.Equals (id)) {
          gadgetTarget.CopyFrom (gadget);
          break;
        }
      }

      return (gadgetTarget);
    }

    internal bool GadgetChanged ()
    {
      throw new NotImplementedException ();//TODO: what for??
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
        if (m_CurrentEditGadget.IsContentTarget) {
          var contents = new Collection<GadgetTarget> ();
          m_CurrentEditGadget.RequestContent (contents);

          foreach (var gadget in contents) {
            var gadgetTarget = GadgetById (gadget.Id);

            if (gadgetTarget.ValidateId) {
              gadgetTarget.MaterialId = m_CurrentMaterialGadget.Id;
              gadgetTarget.Material = m_CurrentMaterialGadget.Material;
              gadgetTarget.IsChecked = true;

              GadgetItemsSource.Add (gadgetTarget);
              AddChecked (gadgetTarget);
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
          if (m_CurrentMaterialGadget.Contains (gadgetTargetModel.MaterialId)) {
            var checkedItem = IsChecked (gadgetTargetModel.Id);

            if (checkedItem.ValidateId.IsFalse ()) {
              if (gadgetTargetModel.Enabled) {
                if (gadgetTargetModel.Busy.IsFalse ()) {
                  gadgetTargetModel.MaterialId = m_CurrentMaterialGadget.Id;
                  gadgetTargetModel.Material = m_CurrentMaterialGadget.Material;

                  GadgetItemsSource.Add (gadgetTargetModel);
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
