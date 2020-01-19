/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
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

    public GadgetTarget GadgetModel
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
        return (GadgetModel.ValidateId);
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (GadgetModel.ValidateId ? GadgetModel.Enabled.IsFalse () : false);
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
        return (GadgetModel.ValidateId ? GadgetModel.Id : Guid.Empty);
      }
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
      GadgetModel = GadgetTarget.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TActionComponent component)
    {
      component.ThrowNull ();

      GadgetModel.CopyFrom (component.Models.GadgetTargetModel);
      
      ComponentControlModel.SelectModel (component);

      BusyVisibility = GadgetModel.BusyVisibility;
    }

    internal void Cleanup ()
    {
      GadgetModel.CopyFrom (GadgetTarget.CreateDefault);
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetTargetModel.CopyFrom (GadgetModel);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
