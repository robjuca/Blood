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

    public bool Enabled
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
      Enabled = false;

      m_Image = null;
    }

    TSelectionInfo (string name, object tag, bool enabled)
      : this ()
    {
      Name = name;
      Tag = tag;
      Enabled = enabled;
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

    public void Select (string name, object tag, bool enabled)
    {
      Name = name;
      Tag = tag;
      Enabled = enabled;
    }

    public void CopyFrom (TSelectionInfo alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        Tag = alias.Tag;
        Enabled = alias.Enabled;

        SetImage (alias.GetImage ());
      }
    }
    #endregion

    #region Fields
    byte []                                 m_Image;
    #endregion

    #region Static
    public static TSelectionInfo Create (string name, object tag, bool enabled) => new TSelectionInfo (name, tag, enabled);
    public static TSelectionInfo CreateDefault => new TSelectionInfo ();
    #endregion
  };
  //---------------------------//

}  // namespace