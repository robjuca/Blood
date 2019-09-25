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
        return (m_SelectedIndex.Equals (-1) ? TSelectionItem.CreateDefault : ItemsSource [m_SelectedIndex]);
      }
    }
    #endregion

    #region Constructor
    TSelectionPropertyInfo ()
    {
      m_SelectedIndex = -1;
    }
    #endregion

    #region Members
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
    #endregion

    #region Overrides
    public override string ToString () => (Selection.ValueString);
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