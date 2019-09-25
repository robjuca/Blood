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
    #endregion

    #region Constructor
    TSelectionItem (string valueString, object tag)
    {
      ValueString = valueString;
      Tag = tag;
    }

    TSelectionItem ()
    {
      ValueString = "empty";
      Tag = null;
    }
    #endregion

    #region Members
    public bool Contains (TSelectionItem alias)
    {
      if (alias.NotNull ()) {
        return (ValueString.Equals (alias.ValueString));
      }

      return (false);
    }

    public bool Contains (string valueString)
    {
      return (ValueString.Equals (valueString));
    }
    #endregion

    #region Static
    public static TSelectionItem Create (string valueString, object tag) => new TSelectionItem (valueString, tag);

    public static TSelectionItem CreateDefault => new TSelectionItem (); 
    #endregion
  };
  //---------------------------//

}  // namespace