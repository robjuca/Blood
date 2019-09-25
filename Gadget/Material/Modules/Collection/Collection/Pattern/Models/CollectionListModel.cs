/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<Server.Models.Component.GadgetMaterial> ItemsSource
    {
      get; 
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public Server.Models.Component.GadgetMaterial Current
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      SelectedIndex = -1;

      ItemsSource = new ObservableCollection<Server.Models.Component.GadgetMaterial> ();
      Current = Server.Models.Component.GadgetMaterial.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        action.ModelAction.CopyFrom (modelAction.Value);
        
        var model = action.ModelAction.GadgetMaterialModel;
        model.CopyFrom (action); // set gadget model

        ItemsSource.Add (model);
      }

      if (ItemsSource.Count > 0) {
        SelectedIndex = 0;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
