/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using rr.Library.Types;
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

    public bool Locked
    {
      get;
      set;
    }

    public bool IsChecked
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

    public bool IsEditEnabled
    {
      get
      {
        return (ValidateId && Locked.IsFalse ());
      }
    }

    public bool IsRemoveEnabled
    {
      get
      {
        return (ValidateId ? (Enabled.IsFalse () && Locked.IsFalse ()) : false);
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

    public Collection<byte> Image
    {
      get;
      private set;
    }

    public bool HasImage
    {
      get
      {
        return (Image.Any ());
      }
    }

    public DateTime Date
    {
      get;
      private set;
    }

    public TObservableCommand ObservableCommand
    {
      get; 
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
      IsChecked = false;
      Image = new Collection<byte> ();
      Date = DateTime.Now;

      ObservableCommand = new TObservableCommand (new DelegateCommand<object> (ObservableCommandHandler));
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
        IsChecked = alias.IsChecked;
        SetImage (alias.Image);
        Date = alias.Date;
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
        IsChecked = alias.IsChecked;
        SetImage (alias.Image);
        Date = alias.Date;
      }
    }

    public byte [] GetImage ()
    {
      var image = new byte [Image.Count];
      Image.CopyTo (image, 0);

      return (image);
    }

    public void SetImage (Collection<byte> image)
    {
      if (image.NotNull ()) {
        Image = new Collection<byte> (image);
      }
    }

    public void SetImage (byte [] image)
    {
      if (image.NotNull ()) {
        Image = new Collection<byte> (image);
      }
    }

    public void SetDate (DateTime date)
    {
      if (date.NotNull ()) {
        Date = date;
      }
    }
    #endregion

    #region Event
    public event EventHandler<TObservableCommandEventArgs> ObservableCommandEvent;

    void ObservableCommandHandler (object context)
    {
      ObservableCommandEvent?.Invoke (this, new TObservableCommandEventArgs (context));
    }
    #endregion

    #region class
    // TObservableCommandEventArgs
    public class TObservableCommandEventArgs : EventArgs
    {
      #region Property
      public object Context
      {
        get;
      }
      #endregion

      #region Constructor
      public TObservableCommandEventArgs (object context)
      {
        Context = context;
      }
      #endregion
    } 
    #endregion
  };
  //---------------------------//

}  // namespace