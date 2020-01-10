/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using rr.Library.Types;

using Server.Models.Action;
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

      m_SelectionAllCollection = new Collection<TSelectionItem> ();
    }
    #endregion

    #region Members
    public void Select (TEntityAction action)
    {
      ItemsSource.Clear ();
      m_SelectionAllCollection.Clear ();

      if (action.NotNull ()) {
        foreach (var item in action.SupportAction.SelectionCollection) {
          if (item.Enabled) {
            var selectionItem = TSelectionItem.Create (item.Name, item.Tag, item.GetImage (), item.Enabled);

            ItemsSource.Add (selectionItem);

            m_SelectionAllCollection.Add (selectionItem);
          }
        }
      }

      if (ItemsSource.Any ()) {
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

    public void SelectModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        var name = action.SupportAction.SelectionInfo.Name;
        var tag = action.SupportAction.SelectionInfo.Tag;
        var image = action.SupportAction.SelectionInfo.GetImage ();
        var enabled = action.SupportAction.SelectionInfo.Enabled;

        if (enabled) {
          var selection = TSelectionItem.Create (name, tag, image, enabled);

          Select (selection);
        }
      }
    }

    public void Request (TEntityAction action)
    {
      if (action.NotNull ()) {
        if (HasSelection) {
          action.SupportAction.SelectionInfo.Select (Selection.ValueString, Selection.Tag, Selection.Enabled);
          action.SupportAction.SelectionInfo.SetImage (Selection.GetImage ());
        }
      }
    }

    public void Lock (bool lockCurrent)
    {
      var current = Selection;  // preserve

      ItemsSource.Clear ();

      if (lockCurrent) {
        ItemsSource.Add (current);
      }

      // restore
      else {
        foreach (var item in m_SelectionAllCollection) {
          ItemsSource.Add (item);
        }
      }

      Select (current);
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
    readonly Collection<TSelectionItem>               m_SelectionAllCollection;
    int                                               m_SelectedIndex;
    #endregion

    #region Static
    public static TSelectionPropertyInfo CreateDefault => new TSelectionPropertyInfo ();
    #endregion
  }
  //---------------------------//

}  // namespace