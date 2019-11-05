/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> MaterialItemsSource
    {
      get;
    }

    public ObservableCollection<TComponentModelItem> TargetItemsSource
    {
      get;
    }

    public string MaterialCount
    {
      get
      {
        return ($"[ {MaterialItemsSource.Count} ]");
      }
    }

    public string TargetCount
    {
      get
      {
        return ($"[ {TargetItemsSource.Count} ]");
      }
    }

    public TComponentModelItem MaterialSelected
    {
      get;
      set;
    }

    public int TargetSelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem TargetCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      MaterialItemsSource = new ObservableCollection<TComponentModelItem> ();
      TargetItemsSource = new ObservableCollection<TComponentModelItem> ();

      TargetSelectedIndex = -1;

      TargetCurrent = TComponentModelItem.CreateDefault;

      Targets = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      Targets.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetTargetCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetTargetModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        Targets.Add (TComponentModelItem.Create (action));
      }
    }

    internal void RefreshModel (Server.Models.Component.TEntityAction action)
    {
      // for gadget Material
      MaterialItemsSource.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetMaterialCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetMaterialModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        MaterialItemsSource.Add (TComponentModelItem.Create (action));

        foreach (var item in Targets) {
          // Node reverse here
          if (item.NodeModel.ParentId.Equals (gadget.Id)) {
            item.GadgetMaterialModel.CopyFrom (gadget);
          }
        }
      }

      if (MaterialItemsSource.Count > 0) {
        MaterialSelected = MaterialItemsSource [0];
      }
    }

    internal void MaterialChanged (int selectdIndex)
    {
      TargetItemsSource.Clear ();

      if (selectdIndex.Equals (-1)) {
        TargetSelectedIndex = -1;
      }

      else {
        foreach (var item in Targets) {
          if (item.GadgetTargetModel.MaterialId.Equals (MaterialSelected.GadgetMaterialModel.Id)) {
            TargetItemsSource.Add (item);
          }
        }

        if (TargetItemsSource.Count > 0) {
          TargetSelectedIndex = 0;
        }
      }
    }
    #endregion

    #region property
    Collection<TComponentModelItem> Targets
    {
      get;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
