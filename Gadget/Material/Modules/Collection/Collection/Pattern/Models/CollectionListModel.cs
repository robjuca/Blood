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

using Shared.ViewModel;
using Shared.Gadget.Models.Component;
using Shared.Gadget.Models.Action;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<GadgetMaterial> ItemsSource
    {
      get; 
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public string MaterialCount
    {
      get
      {
        return ($"[ {ItemsSource.Count} ]");
      }
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      SelectedIndex = -1;

      ItemsSource = new ObservableCollection<GadgetMaterial> ();
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      ItemsSource.Clear ();

      var gadgets = new Collection<TActionComponent> ();

      TActionConverter.Collection (TCategory.Material, gadgets, entityAction);

      foreach (var model in gadgets) {
        ItemsSource.Add (model.Models.GadgetMaterialModel);
      }

      if (ItemsSource.Any ()) {
        SelectedIndex = 0;
      }
    }

    internal TActionComponent RequestCurrent ()
    {
      if (SelectedIndex.Equals (-1)) {
        return (null);
      }

      var model = ItemsSource [SelectedIndex];
      
      var component = TActionComponent.Create (TCategory.Material);
      component.Models.GadgetMaterialModel.CopyFrom (model);

      return (component);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
