/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public class TSelectionPropertyInfo : NotificationObject
  {
    #region Property
    public Collection<TSelectionItem> ItemsSource
    {
      get;
      private set;
    }

    public int SelectedIndex
    {
      get
      {
        return (m_SelectedIndex);
      }

      set
      {
        m_SelectedIndex = value;

        if (m_SelectedIndex > -1)  {
          RaisePropertyChanged ("SelectionProperty");
        }
      }
    }

    public TSelectionItem Selection
    {
      get
      {
        return (HasSelection ? ItemsSource [m_SelectedIndex] : TSelectionItem.CreateDefault);
      }
    }
    #endregion

    #region Constructor
    TSelectionPropertyInfo ()
    {
      m_SelectedIndex = -1;

      ItemsSource = new Collection<TSelectionItem> ();
    }
    #endregion

    #region Members
    public void Select (Server.Models.Component.TEntityAction action)
    {
      ItemsSource.Clear ();

      if (action.NotNull ()) {
        foreach (var item in action.SupportAction.SelectionCollection) {
          var selectionItem = TSelectionItem.Create (item.Name, item.Tag, item.GetImage ());

          ItemsSource.Add (selectionItem);
        }
      }

      if (ItemsSource.Count > 0) {
        SelectedIndex = 0;
      }
    }

    public void Select (TSelectionItem item)
    {
      for (int index = 0; index < ItemsSource.Count; index++) {
        var selectionItem = ItemsSource [index];

        if (selectionItem.Contains (item)) {
          SelectedIndex = index;
          break;
        }
      }
    }

    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        var name = action.SupportAction.SelectionInfo.Name;
        var tag = action.SupportAction.SelectionInfo.Tag;
        var image = action.SupportAction.SelectionInfo.GetImage ();

        var selection = TSelectionItem.Create (name, tag, image);

        Select (selection);
      }
    }

    public void Request (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        if (HasSelection) {
          action.SupportAction.SelectionInfo.Select (Selection.ValueString, Selection.Tag, Selection.Image);
        }
      }
    }
    #endregion

    #region Overrides
    public override string ToString () => (Selection.ValueString);
    #endregion

    #region Property
    bool HasSelection
    {
      get
      {
        return (SelectedIndex > -1);
      }
    } 
    #endregion

    #region Fields
    int                                     m_SelectedIndex;
    #endregion

    #region Static
    public static TSelectionPropertyInfo CreateDefault => new TSelectionPropertyInfo ();
    #endregion
  }
  //---------------------------//

}  // namespace