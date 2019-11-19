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
    public ObservableCollection<TComponentModelItem> TestsItemsSource
    {
      get;
    }

    public string TestsCount
    {
      get
      {
        return ($"[ {TestsItemsSource.Count} ]");
      }
    }

    public int TestsSelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem TestsCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      TestsItemsSource = new ObservableCollection<TComponentModelItem> ();

      TestsSelectedIndex = -1;

      TestsCurrent = TComponentModelItem.CreateDefault;

      Testss = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      Testss.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetTestsCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetTestsModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        Testss.Add (TComponentModelItem.Create (action));
      }
    }
    #endregion

    #region property
    Collection<TComponentModelItem> Testss
    {
      get;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
