/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

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
    public ObservableCollection<GadgetReport> ItemsSource
    {
      get;
    }

    public string ReportCount
    {
      get
      {
        return ($"[ {ItemsSource.Count} ]");
      }
    }

    public int SelectedIndex
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      ItemsSource = new ObservableCollection<GadgetReport> ();

      SelectedIndex = -1;

      Reports = new Collection<GadgetReport> ();
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      entityAction.ThrowNull ();

      Reports.Clear ();

      var gadgets = new Collection<TActionComponent> ();
      TActionConverter.Collection (TCategory.Report, gadgets, entityAction);

      foreach (var model in gadgets) {
        Reports.Add (model.Models.GadgetReportModel);
      }
    }

    internal bool SelectionChanged (TActionComponent component)
    {
      component.ThrowNull ();

      bool res = false;

      if (SelectedIndex.Equals (-1).IsFalse ()) {
        var gadget = ItemsSource [SelectedIndex];
        component.Models.GadgetReportModel.CopyFrom (gadget);
        res = true;
      }

      return (res);
    }
    #endregion

    #region property
    Collection<GadgetReport> Reports
    {
      get;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
