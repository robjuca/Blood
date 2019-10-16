/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public class TSelectionItem
  {
    #region Property
    public string ValueString
    {
      get;
    }

    public object Tag
    {
      get;
    }

    public bool Enabled
    {
      get;
    }
    #endregion

    #region Constructor
    TSelectionItem (string valueString, object tag, byte [] image, bool enabled)
    {
      ValueString = valueString;
      Tag = tag;
      SetImage (image);
      Enabled = enabled;
    }

    TSelectionItem ()
    {
      ValueString = "empty";
      Tag = null;
      SetImage (null);
      Enabled = false;
    }
    #endregion

    #region Members
    public bool Contains (TSelectionItem alias)
    {
      if (alias.NotNull ()) {
        // Tag
        if (string.IsNullOrEmpty (alias.ValueString)) {
          return (Tag.Equals (alias.Tag));
        }

        // ValueString
        if (alias.Tag.IsNull ()) {
          return (ValueString.Equals (alias.ValueString));
        }

        // both
        return (ValueString.Equals (alias.ValueString) && Tag.Equals (alias.Tag));
      }

      return (false);
    }

    public bool Contains (string valueString)
    {
      return (ValueString.Equals (valueString));
    }

    public byte [] GetImage ()
    {
      return (m_Image);
    }

    public void SetImage (byte [] image)
    {
      m_Image = image;
    }
    #endregion

    #region Fields
    byte []                                 m_Image; 
    #endregion

    #region Static
    public static TSelectionItem Create (string valueString, object tag, byte [] image, bool enabled) => new TSelectionItem (valueString, tag, image, enabled);

    public static TSelectionItem CreateDefault => new TSelectionItem (); 
    #endregion
  };
  //---------------------------//

}  // namespace