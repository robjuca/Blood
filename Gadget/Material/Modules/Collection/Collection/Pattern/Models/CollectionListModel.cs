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

      ItemsSource.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetMaterialCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        action.ModelAction.CopyFrom (modelAction);

        var item = TComponentModelItem.Create (action);
        item.GadgetMaterialModel.Refresh (action);

        ItemsSource.Add (item);
      }

      if (ItemsSource.Count > 0) {
        SelectedIndex = 0;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
