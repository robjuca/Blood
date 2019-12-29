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
  public class TFactoryListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> MaterialSelectionItemsSource
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

    public TComponentModelItem MaterialSelectionCurrent
    {
      get;
      set;
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
      MaterialSelectionItemsSource = new ObservableCollection<TComponentModelItem> ();
      
      MaterialSelectionSelectedIndex = -1;
      MaterialSelectionEnabled = true;

      SelectorTargetChecked = true;
      SelectorTestChecked = false;

      SlideIndex = 0;

      IsEnabledSelector = true;
    }
    #endregion

    #region Members
    internal void MaterialRefreshModel (Server.Models.Component.TEntityAction action)
    {
      // for gadget Material
      MaterialSelectionItemsSource.Clear ();

      var list = action.CollectionAction.GadgetMaterialCollection
        .OrderBy (p => p.Material)
        .ToList ()
      ;

      foreach (var gadget in list) {
        if (gadget.Enabled) {
          if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
            var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
            modelAction.GadgetMaterialModel.CopyFrom (gadget);

            action.ModelAction.CopyFrom (modelAction);

            MaterialSelectionItemsSource.Add (TComponentModelItem.Create (action));
          }
        }
      }

      if (MaterialSelectionItemsSource.Count > 0) {
        MaterialSelectionCurrent = MaterialSelectionItemsSource [0];
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

    internal void EditEnter (Server.Models.Component.TEntityAction action)
    {
      var gadget = action.ModelAction.GadgetTestModel;
      var relationCategory = gadget.RequestCategory ();

      MaterialSelectionItemChanged (gadget.Material);

      IsEnabledSelector = gadget.ContentCount.Equals (0);
      MaterialSelectionEnabled = gadget.ContentCount.Equals (0);

      switch (relationCategory) {
        case Server.Models.Infrastructure.TCategory.Target: {
            SlideIndex = 0;
            SelectorTargetChecked = true;
          }
          break;

        case Server.Models.Infrastructure.TCategory.Test: {
            SlideIndex = 1;
            SelectorTestChecked = true;
          }
          break;
      }
    }

    internal void MaterialSelectionItemChanged (string materialName)
    {
      for (int index = 0; index < MaterialSelectionItemsSource.Count; index++) {
        if (MaterialSelectionItemsSource [index].GadgetMaterialModel.Material.Equals (materialName)) {
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
