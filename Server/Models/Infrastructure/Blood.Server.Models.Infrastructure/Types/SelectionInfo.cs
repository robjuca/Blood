/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TSelectionInfo
  {
    #region Property
    public string Name
    {
      get;
      private set;
    }

    public object Tag
    {
      get;
      private set;
    }

    public byte [] Image
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TSelectionInfo ()
    {
      Name = string.Empty;
      Tag = null;
      Image = null;
    }

    TSelectionInfo (string name, object tag)
      : this ()
    {
      Name = name;
      Tag = tag;
    }

    TSelectionInfo (string name, object tag, byte [] image)
    {
      Name = name;
      Tag = tag;
      Image = image;
    }
    #endregion

    #region Members
    public void Select (string name, object tag)
    {
      Name = name;
      Tag = tag;
    }

    public void Select (string name, object tag, byte [] image)
    {
      Name = name;
      Tag = tag;
      Image = image;
    }

    public void CopyFrom (TSelectionInfo alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        Tag = alias.Tag;
        Image = alias.Image;
      }
    }
    #endregion

    #region Static
    public static TSelectionInfo Create (string name, object tag) => new TSelectionInfo (name, tag);
    public static TSelectionInfo Create (string name, object tag, byte [] image) => new TSelectionInfo (name, tag, image);
    public static TSelectionInfo CreateDefault => new TSelectionInfo ();
    #endregion
  };
  //---------------------------//

}  // namespace