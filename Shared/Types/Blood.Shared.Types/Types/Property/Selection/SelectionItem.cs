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

    public byte [] Image
    {
      get;
    }
    #endregion

    #region Constructor
    TSelectionItem (string valueString, object tag, byte [] image)
    {
      ValueString = valueString;
      Tag = tag;
      Image = image;
    }

    TSelectionItem ()
    {
      ValueString = "empty";
      Tag = null;
      Image = null;
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
    #endregion

    #region Static
    public static TSelectionItem Create (string valueString, object tag, byte [] image) => new TSelectionItem (valueString, tag, image);

    public static TSelectionItem CreateDefault => new TSelectionItem (); 
    #endregion
  };
  //---------------------------//

}  // namespace