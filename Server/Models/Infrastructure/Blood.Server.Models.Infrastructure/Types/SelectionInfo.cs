/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
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

    TSelectionInfo (string name, object tag, byte [] image)
    {
      Name = name;
      Tag = tag;

      SetImage (image);
    }

    TSelectionInfo (string name, object tag, Collection<byte> image)
    {
      Name = name;
      Tag = tag;

      SetImage (image);
    }
    #endregion

    #region Members
    public byte [] GetImage ()
    {
      return (m_Image);
    }

    public void Select (string name, object tag)
    {
      Name = name;
      Tag = tag;
    }

    public void Select (string name, object tag, byte [] image)
    {
      Name = name;
      Tag = tag;

      SetImage (image);
    }

    public void Select (string name, object tag, Collection<byte> image)
    {
      Name = name;
      Tag = tag;

      SetImage (image);
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
    public static TSelectionInfo Create (string name, object tag, byte [] image) => new TSelectionInfo (name, tag, image);
    public static TSelectionInfo Create (string name, object tag, Collection<byte> image) => new TSelectionInfo (name, tag, image);
    public static TSelectionInfo CreateDefault => new TSelectionInfo ();
    #endregion

    #region Support
    void SetImage (byte [] image)
    {
      m_Image = image;
    }

    void SetImage (Collection<byte> image)
    {
      m_Image = new byte [image.Count];
      image.CopyTo (m_Image, 0);
    }
    #endregion
  };
  //---------------------------//

}  // namespace