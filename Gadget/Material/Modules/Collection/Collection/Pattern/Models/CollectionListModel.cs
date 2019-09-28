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
    public ObservableCollection<TComponentModelItem> ItemsSource
    {
      get; 
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem Current
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      SelectedIndex = -1;

      ItemsSource = new ObservableCollection<TComponentModelItem> ();
      Current = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      action.CollectionAction.GadgetMaterialCollection.Clear ();

      ItemsSource.Clear ();

      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        action.ModelAction.CopyFrom (modelAction.Value);
        
        var model = action.ModelAction.GadgetMaterialModel;
        model.CopyFrom (action); // set gadget model

        ItemsSource.Add (TComponentModelItem.Create (action));

        // update list
        action.CollectionAction.GadgetMaterialCollection.Add (model);
      }

      if (ItemsSource.Count > 0) {
        SelectedIndex = 0;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
