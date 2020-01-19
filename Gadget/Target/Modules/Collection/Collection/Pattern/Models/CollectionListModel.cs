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

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<GadgetMaterial> MaterialItemsSource
    {
      get;
    }

    public ObservableCollection<GadgetTarget> TargetItemsSource
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

    public GadgetMaterial MaterialSelected
    {
      get
      {
        return (MaterialSelectionSelectedIndex.Equals (-1) ? GadgetMaterial.CreateDefault : MaterialItemsSource [MaterialSelectionSelectedIndex]);
      }
    }

    public int MaterialSelectionSelectedIndex
    {
      get;
      set;
    }

    public int TargetSelectedIndex
    {
      get;
      set;
    }

    public GadgetTarget TargetCurrent
    {
      get
      {
        return (TargetSelectedIndex.Equals (-1) ? GadgetTarget.CreateDefault : TargetItemsSource [TargetSelectedIndex]);
      }
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      MaterialItemsSource = new ObservableCollection<GadgetMaterial> ();
      TargetItemsSource = new ObservableCollection<GadgetTarget> ();

      MaterialSelectionSelectedIndex = -1;
      TargetSelectedIndex = -1;

      Targets = new Collection<GadgetTarget> ();
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      Targets.Clear ();

      var gadgets = new Collection<TActionComponent> ();
      
      TActionConverter.Collection (TCategory.Target, gadgets, entityAction);

      foreach (var component in gadgets) {
        Targets.Add (component.Models.GadgetTargetModel);
      }
    }

    internal void RefreshModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      MaterialItemsSource.Clear ();
      MaterialSelectionSelectedIndex = -1;

      // for gadget Material
      var gadgets = new Collection<TActionComponent> ();
      TActionConverter.Collection (TCategory.Material, gadgets, entityAction);

      foreach (var component in gadgets) {
        var gadgetMaterial = component.Models.GadgetMaterialModel;

        if (gadgetMaterial.Enabled) {
          MaterialItemsSource.Add (gadgetMaterial);

          foreach (var gadgetTarget in Targets) {
            // Node reverse here 
            if (gadgetTarget.MaterialId.Equals (gadgetMaterial.Id)) {
              gadgetTarget.Material = gadgetMaterial.GadgetName;
            }
          }
        }
      }

      if (MaterialItemsSource.Any ()) {
        MaterialSelected.CopyFrom (MaterialItemsSource [0]);
        MaterialSelectionSelectedIndex = 0;
      }
    }

    internal void MaterialChanged ()
    {
      TargetItemsSource.Clear ();

      TargetSelectedIndex = -1;

      foreach (var model in Targets) {
        if (model.MaterialId.Equals (MaterialSelected.Id)) {
          TargetItemsSource.Add (model);
        }
      }

      if (TargetItemsSource.Any ()) {
        TargetSelectedIndex = 0;
      }
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetTargetModel.CopyFrom (TargetCurrent);
      component.Models.GadgetMaterialModel.CopyFrom (MaterialSelected);
    }
    #endregion

    #region property
    Collection<GadgetTarget> Targets
    {
      get;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
