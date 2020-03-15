﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Globalization;

using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public static class TContentStyle
  {
    #region Data
    public enum Style
    {
      mini,
      small,
      large,
      big,
      None,
    };

    public enum Mode
    {
      Horizontal,
      Vertical,
      None,
    };
    #endregion

    #region Property
    public static TSize WindowSize
    {
      get
      {
        var size = TSize.CreateDefault;
        size.Width = RequestStyleSize (Mode.Horizontal, Style.big);
        size.Height = RequestStyleSize (Mode.Vertical, Style.big);

        return (size); 
      }
    }

    public static string WindowSizeString
    {
      get
      {
        return ($"{WindowSize.Width} x {WindowSize.Height}");
      }
    }

    public static string HorizontalStyleSizeString
    {
      get
      {
        return ($"-> style horizontal:{Environment.NewLine}mini: {RequestStyleSizeString (Mode.Horizontal, Style.mini)}{Environment.NewLine}big: {RequestStyleSizeString (Mode.Horizontal, Style.big)}");
      }
    }

    public static string VerticalStyleSizeString
    {
      get
      {
        return ($"-> style vertical:{Environment.NewLine}mini: {RequestStyleSizeString (Mode.Vertical, Style.mini)}{Environment.NewLine}big: {RequestStyleSizeString (Mode.Vertical, Style.big)}");
      }
    }

    public static string DashBoardSizeString
    {
      get
      {
        return ($"{HorizontalStyleSizeString}{Environment.NewLine}{VerticalStyleSizeString}{Environment.NewLine}-> dashboard size: {WindowSizeString}");
      }
    }
    #endregion

    #region Static Property
    public static string MINI => (Style.mini.ToString ());

    public static string SMALL => (Style.small.ToString ());

    public static string LARGE => (Style.large.ToString ());

    public static string BIG => (Style.big.ToString ());

    public static string NONE => (Style.None.ToString ());

    public static Array GetValues => (Enum.GetValues (typeof (Style)));

    public static readonly string [] Names = new string [] { MINI, SMALL, LARGE, BIG, NONE };

    public static int ColumnWidth
    {
      get
      {
        if (m_ColumnWidth.Equals (0)) {
          var supportSettings = TSupportSettings.CreateDefault;

          if (supportSettings.Validate ()) {
            var settingsValue = TSupportSettingsData.Request ("ColumnWidth");

            m_ColumnWidth = int.Parse (settingsValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
          }
        }

        return (m_ColumnWidth);
      }
    }
    #endregion

    #region Members
    public static void SelectColumnWidth (int columnWidth)
    {
      m_ColumnWidth = columnWidth;
    }

    public static void RequestSize (TSize size)
    {
      if (size.NotNull ()) {
        size.Width = (size.Columns * MiniSize.Width);
        size.Height = (size.Rows * MiniSize.Height);
      }
    }

    public static int RequestStyleSize (TContentStyle.Mode mode, TContentStyle.Style style)
    {
      int size = 0;

      switch (mode) {
        case Mode.Horizontal:
          size = m_HorizontalSizeStyles [style];
          break;

        case Mode.Vertical:
          size = m_VerticalSizeStyles [style];
          break;
      }

      return (size);
    }

    public static int RequestBoardStyleSize (TContentStyle.Style style)
    {
      return (m_BoardStyleSize.ContainsKey (style) ? m_BoardStyleSize [style] : 0);
    }

    public static string RequestStyleSizeString (TContentStyle.Mode mode, TContentStyle.Style style)
    {
      return (RequestStyleSize (mode, style).ToString (CultureInfo.InvariantCulture));
    }

    public static Style TryToParse (string style)
    {
      if (style.NotNull ()) {
        if (Enum.TryParse (style.Trim (), out Style someStyle)) {
          return (someStyle);
        }
      }

      return (Style.None);
    }
    #endregion

    #region Property
    static TSize MiniSize
    {
      get
      {
        if (m_MiniSize.IsNull ()) {
          var miniHeight = (int) (m_ColumnWidth * .5); // 50%

          m_MiniSize = TSize.CreateDefault;
          m_MiniSize.Width = m_ColumnWidth; // 1c
          m_MiniSize.Height = miniHeight; // 1r
        }

        return (m_MiniSize);
      }
    } 
    #endregion

    #region Fields
    static TSize                                                          m_MiniSize; 
    static int                                                            m_ColumnWidth = 0;

    static readonly Dictionary<TContentStyle.Style, int>                  m_HorizontalSizeStyles = new Dictionary<Style, int> ()
      {
        {TContentStyle.Style.mini, (MiniSize.Width * 1)},
        {TContentStyle.Style.small, (MiniSize.Width * 2)},
        {TContentStyle.Style.large, (MiniSize.Width * 3)},
        {TContentStyle.Style.big, (MiniSize.Width * 4)},
      };


    static readonly Dictionary<TContentStyle.Style, int>                  m_VerticalSizeStyles = new Dictionary<Style, int> ()
      {
        {TContentStyle.Style.mini, (MiniSize.Height * 1)},
        {TContentStyle.Style.small, (MiniSize.Height * 2)},
        {TContentStyle.Style.large, (MiniSize.Height * 3)},
        {TContentStyle.Style.big, (MiniSize.Height * 4)},
      };

    static readonly Dictionary<TContentStyle.Style, int>                  m_BoardStyleSize= new Dictionary<TContentStyle.Style, int>
      {
        { TContentStyle.Style.mini, 1 },
        { TContentStyle.Style.small, 2 },
        { TContentStyle.Style.large, 3 },
        { TContentStyle.Style.big, 4 },
      };
    #endregion
  };
  //---------------------------//

}  // namespace
