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
    public ObservableCollection<TGadgetTestModel> ItemsSource
    {
      get; 
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public TGadgetTestModel Current
    {
      get;
    }

    public string TestCount
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

      ItemsSource = new ObservableCollection<TGadgetTestModel> ();
      Current = TGadgetTestModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      entityAction.ThrowNull ();

      ItemsSource.Clear ();

      TGadgetTestActionComponent.Select (ItemsSource, entityAction);

      if (ItemsSource.Any ()) {
        SelectedIndex = 0;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
