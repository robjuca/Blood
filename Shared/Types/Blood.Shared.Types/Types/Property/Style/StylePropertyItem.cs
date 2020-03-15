/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Globalization;

using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public class TStylePropertyItem
  {
    #region Property
    public TStyleInfo StyleInfo
    {
      get; 
    }

    public TSize Size
    {
      get;
      private set;
    }

    public string StyleString
    {
      get
      {
        return (StyleInfo.StyleString);
      }
    }

    public string SizeString
    {
      get
      {
        return (StyleInfo.IsStyleModeHorizontal ? Size.Width.ToString (CultureInfo.InvariantCulture) : StyleInfo.IsStyleModeVertical ? Size.Height.ToString (CultureInfo.InvariantCulture) : string.Empty);
      }
    }
    #endregion

    #region Constructor
    TStylePropertyItem ()
    {
      StyleInfo = TStyleInfo.CreateDefault;

      Size = TSize.CreateDefault;
    }

    public TStylePropertyItem (TContentStyle.Mode styleMode, TContentStyle.Style style)
      : this ()
    {
      StyleInfo = TStyleInfo.Create (styleMode);
      StyleInfo.Select (style);

      var size = TContentStyle.RequestStyleSize (styleMode, style);

      switch (styleMode) {
        case TContentStyle.Mode.Horizontal:
          Size.Width = size;
          break;

        case TContentStyle.Mode.Vertical:
          Size.Height = size;
          break;
      }
    }
    #endregion

    #region Static
    public static TStylePropertyItem CreateDefault => new TStylePropertyItem (); 
    #endregion
  };
  //---------------------------//

}  // namespace