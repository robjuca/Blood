/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
using Shared.Gadget.Material;
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

    public GadgetMaterial GadgetModel
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
        return (GadgetModel.Enabled.IsFalse ());
      }
    }

    public Visibility BusyVisibility
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
      GadgetModel = GadgetMaterial.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TActionComponent component)
    {
      component.ThrowNull ();

      if (component.IsCategory (TCategory.Material)) {
        GadgetModel.CopyFrom (component.Models.GadgetMaterialModel);

        ComponentControlModel.SelectModel (component);

        BusyVisibility = component.Models.GadgetMaterialModel.BusyVisibility;
      }
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetMaterialModel.CopyFrom (GadgetModel);
    }

    internal void Cleanup ()
    {
      GadgetModel.CopyFrom (GadgetMaterial.CreateDefault);
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
