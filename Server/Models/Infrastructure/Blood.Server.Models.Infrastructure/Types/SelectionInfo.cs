/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
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
    #endregion

    #region Constructor
    TSelectionInfo ()
    {
      Name = string.Empty;
      Tag = null;

      m_Image = null;
    }

    TSelectionInfo (string name, object tag)
      : this ()
    {
      Name = name;
      Tag = tag;
    }
    #endregion

    #region Members
    public byte [] GetImage ()
    {
      return (m_Image);
    }

    public void SetImage (byte [] image)
    {
      m_Image = image;
    }

    public void Select (string name, object tag)
    {
      Name = name;
      Tag = tag;
    }

    public void CopyFrom (TSelectionInfo alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        Tag = alias.Tag;

        SetImage (alias.GetImage ());
      }
    }
    #endregion

    #region Fields
    byte []                                 m_Image;
    #endregion

    #region Static
    public static TSelectionInfo Create (string name, object tag) => new TSelectionInfo (name, tag);
    public static TSelectionInfo CreateDefault => new TSelectionInfo ();
    #endregion
  };
  //---------------------------//

}  // namespace