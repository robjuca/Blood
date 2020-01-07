/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Server.Models.Action;

using Shared.ViewModel;

using Shared.Gadget.Target;
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
        return (ComponentModelItem.IsNull () ? false : (ComponentModelItem.InfoModel.Enabled.IsFalse ()));
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
    internal void Select (TComponentModelItem item)
    {
      ComponentModelItem = item ?? throw new System.ArgumentNullException (nameof (item));

      var action = TEntityAction.CreateDefault;
      //action.ModelAction.GadgetMaterialModel.CopyFrom (item.GadgetMaterialModel);
      //action.ModelAction.GadgetTargetModel.CopyFrom (item.GadgetTargetModel);

      //ComponentControlModel.SelectModel (action);

      BusyVisibility = ComponentModelItem.BusyVisibility;
    }

    internal void RequestModel (TEntityAction action)
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
    }
    #endregion
  };
  //---------------------------//

}  // namespace
