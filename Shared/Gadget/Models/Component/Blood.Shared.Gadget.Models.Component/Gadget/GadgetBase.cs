/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class TGadgetBase
  {
    #region Property
    public Guid Id
    {
      get; 
      set;
    }

    public string GadgetInfo
    {
      get; 
      set;
    }

    public string GadgetName
    {
      get;
      set;
    }

    public string Material
    {
      get;
      set;
    }

    public string Description
    {
      get; 
      set;
    }

    public string ExternalLink
    {
      get; 
      set;
    }

    public bool Enabled
    {
      get; 
      set;
    }

    public bool Busy
    {
      get;
      set;
    }

    public bool ValidateId
    {
      get
      {
        return (Id.NotEmpty ());
      }
    }

    public Visibility DisableVisibility
    {
      get
      {
        return (Enabled ? Visibility.Collapsed : Visibility.Visible);
      }
    }

    public Visibility BusyVisibility
    {
      get
      {
        return (Busy ? Visibility.Visible : Visibility.Collapsed);
      }
    }
    #endregion

    #region Constructor
    protected TGadgetBase ()
    {
      Id = Guid.Empty;

      GadgetInfo = string.Empty;
      GadgetName = string.Empty;
      Material = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      Enabled = false;
      Busy = false;
    }

    protected TGadgetBase (TGadgetBase alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public bool Contains (Guid id)
    {
      return (Id.Equals (id));
    }
    #endregion

    #region Protected
    protected void CopyFrom (TGadgetBase alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        GadgetInfo = alias.GadgetInfo;
        GadgetName = alias.GadgetName;
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;
        Busy = alias.Busy;
      }
    }

    protected void Change (TGadgetBase alias)
    {
      if (alias.NotNull ()) {
        GadgetInfo = alias.GadgetInfo;
        GadgetName = alias.GadgetName;
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;
        Busy = alias.Busy;
      }
    } 
    #endregion
  };
  //---------------------------//

}  // namespace