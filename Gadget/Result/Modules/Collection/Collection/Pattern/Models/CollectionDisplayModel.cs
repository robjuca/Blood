/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;

using Shared.Gadget.Result;
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

    public GadgetResult Current
    {
      get;
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
        return (Current.ValidateId);
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (Current.ValidateId ? Current.Enabled.IsFalse () : false);
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
        return (Current.Id);
      }
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
      Current = GadgetResult.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TActionComponent component)
    {
      component.ThrowNull ();

      if (component.IsCategory (TCategory.Result)) {
        Current.CopyFrom (component.Models.GadgetResultModel);

        ComponentControlModel.SelectModel (component);

        BusyVisibility = Current.BusyVisibility;
      }
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      //var modelAction = ComponentModelItem.RequestModel ();
      //action.ModelAction.CopyFrom (modelAction);

      //action.Id = Id;
      //action.CategoryType.Select (ComponentModelItem.Category);
    }

    internal void Cleanup ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
      Current.CopyFrom (GadgetResult.CreateDefault);

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
