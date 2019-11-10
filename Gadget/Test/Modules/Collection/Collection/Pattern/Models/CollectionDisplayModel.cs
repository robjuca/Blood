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
        return (ComponentModelItem.IsNull () ? false : ComponentModelItem.Busy.IsFalse ());
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (ComponentModelItem.IsNull () ? false : ComponentModelItem.CanRemove);
      }
    }

    public Visibility BusyVisibility
    {
      get;
      set;
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

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelItem = TComponentModelItem.Create (action);

      ComponentControlModel.SelectModel (action);

      BusyVisibility = ComponentModelItem.BusyVisibility;
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      action.Id = Id;
      action.CategoryType.Select (ComponentModelItem.Category);

      var modelAction = ComponentModelItem.RequestModel ();
      action.ModelAction.CopyFrom (modelAction);

      ComponentControlModel.RequestTargets (action);
    }

    internal void Cleanup ()
    {
      ComponentModelItem = null;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
