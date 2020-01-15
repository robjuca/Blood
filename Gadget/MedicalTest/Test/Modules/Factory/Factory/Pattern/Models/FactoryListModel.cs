/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Action;

using Shared.Gadget.Models.Action;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListModel
  {
    #region Property
    public ObservableCollection<TGadgetMaterialModel> MaterialSelectionItemsSource
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

    public TGadgetMaterialModel MaterialSelectionCurrent
    {
      get
      {
        return (MaterialSelectionSelectedIndex.Equals (-1) ? TGadgetMaterialModel.CreateDefault : MaterialSelectionItemsSource [MaterialSelectionSelectedIndex]);
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
      MaterialSelectionItemsSource = new ObservableCollection<TGadgetMaterialModel> ();
      
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
      TGadgetMaterialActionComponent.Select (MaterialSelectionItemsSource, entityAction);

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

    internal void EditEnter (TGadgetTestModel gadget)
    {
      gadget.ThrowNull ();

      MaterialSelectionItemChanged (gadget.Model.Material);

      IsEnabledSelector = gadget.Model.IsContentEmpty;
      MaterialSelectionEnabled = gadget.Model.IsContentEmpty;

      if (gadget.Model.IsContentTarget) {
        SlideIndex = 0;
        SelectorTargetChecked = true;
      }

      if (gadget.Model.IsContentTest) {
        SlideIndex = 1;
        SelectorTestChecked = true;
      }
    }

    internal void MaterialSelectionItemChanged (string materialName)
    {
      for (int index = 0; index < MaterialSelectionItemsSource.Count; index++) {
        if (MaterialSelectionItemsSource [index].Model.Material.Equals (materialName)) {
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
