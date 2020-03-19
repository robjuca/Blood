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
  public class TFactoryListModel
  {
    #region Property
    public ObservableCollection<GadgetMaterial> MaterialSelectionItemsSource
    {
      get;
    }

    public int MaterialSelectionItemsSourceCount
    {
      get
      {
        return (MaterialSelectionItemsSource.Count);
      }
    }

    public GadgetMaterial MaterialSelectionCurrent
    {
      get
      {
        return (MaterialSelectionSelectedIndex.Equals (-1) ? GadgetMaterial.CreateDefault : MaterialSelectionItemsSource [MaterialSelectionSelectedIndex]);
      }
    }

    public int MaterialSelectionSelectedIndex
    {
      get;
      set;
    }

    public bool MaterialSelectionEnabled
    {
      get;
      set;
    }

    public bool SelectorTargetChecked
    {
      get;
      set;
    }

    public bool SelectorTestChecked
    {
      get;
      set;
    }

    public bool IsEnabledSelector
    {
      get; 
      set;
    }

    public int SlideIndex
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TFactoryListModel ()
    {
      MaterialSelectionItemsSource = new ObservableCollection<GadgetMaterial> ();
      
      MaterialSelectionSelectedIndex = -1;
      MaterialSelectionEnabled = true;

      SelectorTargetChecked = true;
      SelectorTestChecked = false;

      SlideIndex = 0;

      IsEnabledSelector = true;
    }
    #endregion

    #region Members
    internal void MaterialRefreshModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      // for gadget Material
      var gadgets = new Collection<TActionComponent> ();
      TActionConverter.Collection (TCategory.Material, gadgets, entityAction);

      MaterialSelectionItemsSource.Clear ();

      foreach (var gadget in gadgets) {
        MaterialSelectionItemsSource.Add (gadget.Models.GadgetMaterialModel);
      }

      if (MaterialSelectionItemsSource.Any ()) {
        MaterialSelectionSelectedIndex = 0;
      }
    }

    internal void PropertyChanged (string propertyName, bool locked)
    {
      switch (propertyName) {
        case "GadgetAdd":
          IsEnabledSelector = false;
          MaterialSelectionEnabled = false;
          break;

        case "GadgetRemove":
          if (locked.IsFalse ()) {
            IsEnabledSelector = true;  
            MaterialSelectionEnabled = true;
          }
          break;
      }
    }

    internal void Request (TActionComponent component)
    {
      if (component.NotNull ()) {
        component.Models.GadgetMaterialModel.CopyFrom (MaterialSelectionCurrent);
      }
    }

    internal void EditEnter (TActionComponent component)
    {
      component.ThrowNull ();

      if (component.IsCategory (TCategory.Test)) {
        var gadget = component.Models.GadgetTestModel;

        MaterialSelectionItemChanged (gadget.Material);

        IsEnabledSelector = gadget.IsContentEmpty;
        MaterialSelectionEnabled = gadget.IsContentEmpty;

        if (gadget.IsContentTarget) {
          SlideIndex = 0;
          SelectorTargetChecked = true;
        }

        if (gadget.IsContentTest) {
          SlideIndex = 1;
          SelectorTestChecked = true;
        }
      }
    }

    internal void MaterialSelectionItemChanged (string materialName)
    {
      for (int index = 0; index < MaterialSelectionItemsSource.Count; index++) {
        if (MaterialSelectionItemsSource [index].GadgetName.Equals (materialName, StringComparison.InvariantCulture)) {
          MaterialSelectionSelectedIndex = index;
          break;
        }
      }
    }

    internal void Cleanup ()
    {
      MaterialSelectionEnabled = true;
      IsEnabledSelector = true;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
