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

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TGadgetMaterialModel> MaterialItemsSource
    {
      get;
    }

    public ObservableCollection<TGadgetTargetModel> TargetItemsSource
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

    public TGadgetMaterialModel MaterialSelected
    {
      get;
      set;
    }

    public int TargetSelectedIndex
    {
      get;
      set;
    }

    public TGadgetTargetModel TargetCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      MaterialItemsSource = new ObservableCollection<TGadgetMaterialModel> ();
      TargetItemsSource = new ObservableCollection<TGadgetTargetModel> ();

      TargetSelectedIndex = -1;

      TargetCurrent = TGadgetTargetModel.CreateDefault;

      Targets = new Collection<TGadgetTargetModel> ();
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      TGadgetTargetActionComponent.Select (Targets, entityAction);
    }

    internal void RefreshModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      MaterialItemsSource.Clear ();
      
      // for gadget Material
      var list = new Collection<TGadgetMaterialModel> ();
      TGadgetMaterialActionComponent.Select (list, entityAction);

      foreach (var gadgetMaterial in list) {
        if (gadgetMaterial.Enabled) {
          MaterialItemsSource.Add (gadgetMaterial);

          foreach (var gadgetTarget in Targets) {
            // Node reverse here 
            if (gadgetTarget.Model.MaterialId.Equals (gadgetMaterial.Id)) {
              gadgetTarget.MaterialModel.CopyFrom (gadgetMaterial);
            }
          }
        }
      }

      if (MaterialItemsSource.Any ()) {
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
          if (item.Model.MaterialId.Equals (MaterialSelected.Id)) {
            TargetItemsSource.Add (item);
          }
        }

        if (TargetItemsSource.Any ()) {
          TargetSelectedIndex = 0;
        }
      }
    }
    #endregion

    #region property
    Collection<TGadgetTargetModel> Targets
    {
      get;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
