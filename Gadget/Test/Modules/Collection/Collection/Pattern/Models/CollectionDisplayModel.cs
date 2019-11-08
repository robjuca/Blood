/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Shared.ViewModel;

using Shared.Gadget.Test;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionDisplayModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      set;
    }

    public TComponentModelItem ComponentModelItem
    {
      get;
      private set;
    }

    public bool IsViewEnabled
    {
      get;
      set;
    }

    public bool IsEditCommandEnabled
    {
      get
      {
        return (ComponentModelItem.NotNull ());
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (ComponentModelItem.IsNull () ? false : ComponentModelItem.CanRemove);
      }
    }

    public Visibility DistortedVisibility
    {
      get;
      set;
    }

    public Visibility BusyVisibility
    {
      get;
      set;
    }

    public bool Distorted
    {
      get
      {
        return (false);
      }
    }

    public Guid Id
    {
      get
      {
        return (ComponentModelItem.IsNull () ? Guid.Empty : ComponentModelItem.Id);
      }
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
      DistortedVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TComponentModelItem item)
    {
      ComponentModelItem = item ?? throw new System.ArgumentNullException (nameof (item));

      BusyVisibility = ComponentModelItem.BusyVisibility;
      DistortedVisibility = Distorted ? Visibility.Visible : Visibility.Hidden;
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      var modelAction = ComponentModelItem.RequestModel ();
      action.ModelAction.CopyFrom (modelAction);

      action.Id = Id;
      action.CategoryType.Select (ComponentModelItem.Category);
    }

    internal void Cleanup ()
    {
      ComponentModelItem = null;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
      DistortedVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
