/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Windows;

using Server.Models.Infrastructure;

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
        return (Current.CanEdit);
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (Current.CanRemove);
      }
    }

    public bool IsModifyCommandEnabled
    {
      get
      {
        return (Current.Enabled && Current.Locked.IsFalse () && Current.HasContent);
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
    internal void Select (TActionComponent component, Dictionary<Guid, GadgetMaterial> materialDictionary)
    {
      component.ThrowNull ();
      materialDictionary.ThrowNull ();

      if (component.IsCategory (TCategory.Result)) {
        Current.CopyFrom (component.Models.GadgetResultModel);

        ComponentControlModel.Select (component, materialDictionary);

        BusyVisibility = Current.BusyVisibility;
      }
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetResultModel.CopyFrom (Current);
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
