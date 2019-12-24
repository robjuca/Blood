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

      //GadgetFullCollection = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    //internal void Select (Server.Models.Component.TEntityAction action)
    //{
    //  // DATA IN:
    //  // action.CollectionAction.ModelCollection

    //  action.ThrowNull ();

    //  GadgetFullCollection.Clear ();

    //  foreach (var gadget in action.CollectionAction.GadgetTargetCollection) {
    //    var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
    //    modelAction.GadgetTargetModel.CopyFrom (gadget);

    //    action.ModelAction.CopyFrom (modelAction);

    //    GadgetFullCollection.Add (TComponentModelItem.Create (action));
    //  }
    //}

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
          

          //foreach (var item in GadgetFullCollection) {
          //  // Node reverse here
          //  if (item.NodeModel.ParentId.Equals (gadget.Id)) {
          //    item.GadgetMaterialModel.CopyFrom (gadget);
          //  }
          //}
        }
      }

      if (MaterialSelectionItemsSource.Count > 0) {
        MaterialSelectionCurrent = MaterialSelectionItemsSource [0];
      }

      //GadgetSelectionEnabled = GadgetCheckedCollection.Count.Equals (0);
    }

    //internal void MaterialSelectionItemChanged (int selectdIndex)
    //{
    //  //GadgetItemsSource.Clear ();

    //  //if (selectdIndex.Equals (-1)) {
    //  //  // do nothing
    //  //}

    //  else {
    //    //foreach (var gadgetItem in GadgetFullCollection) {
    //      //if (gadgetItem.GadgetTargetModel.MaterialId.Equals (GadgetSelectionCurrent.GadgetMaterialModel.Id)) {
    //      //  var checkedItem = IsChecked (gadgetItem.Id);

    //      //  if (checkedItem.IsEmpty) {
    //      //    if (gadgetItem.Enabled) {
    //      //      if (gadgetItem.Busy.IsFalse ()) {
    //      //        GadgetItemsSource.Add (TFactoryListItemInfo.Create (gadgetItem));
    //      //      }
    //      //    }
    //      //  }

    //      //  else {
    //      //    GadgetItemsSource.Add (checkedItem);
    //      //  }
    //      //}
    //    //}
    //  }
    //}
    #endregion

    #region property
    //Collection<TComponentModelItem> GadgetFullCollection
    //{
    //  get;
    //}
    #endregion
  };
  //---------------------------//

}  // namespace
