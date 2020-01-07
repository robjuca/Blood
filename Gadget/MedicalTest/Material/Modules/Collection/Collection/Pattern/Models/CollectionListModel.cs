/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Server.Models.Action;

using Shared.ViewModel;
using Shared.Gadget.Models.Action;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TGadgetMaterialModel> ItemsSource
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

      ItemsSource = new ObservableCollection<TGadgetMaterialModel> ();
      Current = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      TActionComponent.Select (ItemsSource, entityAction);

      if (ItemsSource.Count > 0) {
        SelectedIndex = 0;
      }
    }
    #endregion

    
  };
  //---------------------------//

}  // namespace
