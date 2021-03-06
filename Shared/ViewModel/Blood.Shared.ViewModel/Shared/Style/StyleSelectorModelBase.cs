﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using Server.Models.Action;

using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public abstract class TStyleSelectorModel<T>
  {
    #region Property
    public T StyleMini
    {
      get;
      private set;
    }

    public T StyleSmall
    {
      get;
      private set;
    }

    public T StyleLarge
    {
      get;
      private set;
    }

    public T StyleBig
    {
      get;
      private set;
    }

    public T StyleNone
    {
      get;
      private set;
    }

    public T Current
    {
      get
      {
        return (m_Styles [m_SelectedStyle]);
      }
    }
    #endregion

    #region Constructor
    public TStyleSelectorModel (T styleMini, T styleSmall, T styleLarge, T styleBig, T styleNone)
    {
      StyleMini = styleMini;
      StyleSmall = styleSmall;
      StyleLarge = styleLarge;
      StyleBig = styleBig;
      StyleNone = styleNone;

      m_Styles = new Dictionary<TContentStyle.Style, T>
      {
        { TContentStyle.Style.mini, StyleMini },
        { TContentStyle.Style.small, StyleSmall },
        { TContentStyle.Style.large, StyleLarge },
        { TContentStyle.Style.big, StyleBig },
        { TContentStyle.Style.None, StyleNone }
      };

      m_SelectedStyle = TContentStyle.Style.None;
    }
    #endregion
    
    #region Virtual Members
    public virtual void SelectItem (T styleItem, TEntityAction action)
    {
    }

    public virtual void SelectContent (T styleItem, TEntityAction action)
    {
    }
    #endregion

    #region Members
    public T Request (TContentStyle.Style style)
    {
      return (m_Styles [style]);
    }

    public bool Select (TContentStyle.Style selectedStyle)
    {
      bool res = false;

      if (m_SelectedStyle.Equals (selectedStyle).IsFalse ()) {
        m_SelectedStyle = selectedStyle;
        res = true;
      }

      return (res);
    }

    public void SelectItem (TEntityAction action)
    {
      if (action.NotNull ()) {
        foreach (var item in m_Styles) {
          SelectItem (item.Value, action);
        }
      }
    }

    public void SelectContent (TEntityAction action)
    {
      if (action.NotNull ()) {
        foreach (var item in m_Styles) {
          SelectContent (item.Value, action);
        }
      }
    }
    #endregion

    #region Fields
    readonly Dictionary<TContentStyle.Style, T>                           m_Styles;
    TContentStyle.Style                                                   m_SelectedStyle; 
    #endregion
  };
  //---------------------------//

}  // namespace