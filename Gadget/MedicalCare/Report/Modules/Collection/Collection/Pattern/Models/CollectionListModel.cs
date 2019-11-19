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
    public ObservableCollection<TComponentModelItem> ReportItemsSource
    {
      get;
    }

    public string ReportCount
    {
      get
      {
        return ($"[ {ReportItemsSource.Count} ]");
      }
    }

    public int ReportSelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem ReportCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      ReportItemsSource = new ObservableCollection<TComponentModelItem> ();

      ReportSelectedIndex = -1;

      ReportCurrent = TComponentModelItem.CreateDefault;

      Reports = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      Reports.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetReportCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetReportModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        Reports.Add (TComponentModelItem.Create (action));
      }
    }
    #endregion

    #region property
    Collection<TComponentModelItem> Reports
    {
      get;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
