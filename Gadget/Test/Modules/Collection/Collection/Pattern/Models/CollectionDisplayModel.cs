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

    public GadgetMaterial CurrentGadgetMaterial
    {
      get;
    }

    public GadgetTest GadgetModel
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
        return (GadgetModel.Busy.IsFalse ());
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (GadgetModel.CanRemove);
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
        return (GadgetModel.Id);
      }
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      CurrentGadgetMaterial = GadgetMaterial.CreateDefault;
      GadgetModel = GadgetTest.CreateDefault;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TActionComponent component)
    {
      component.ThrowNull ();

      if (component.IsCategory (TCategory.Test)) {
        CurrentGadgetMaterial.CopyFrom (component.Models.GadgetMaterialModel);
        GadgetModel.CopyFrom (component.Models.GadgetTestModel);

        ComponentControlModel.SelectModel (component);

        BusyVisibility = GadgetModel.BusyVisibility;
      }
    }

    internal void Cleanup ()
    {
      GadgetModel.CopyFrom (GadgetTest.CreateDefault);
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetMaterialModel.CopyFrom (CurrentGadgetMaterial);
      component.Models.GadgetTestModel.CopyFrom (GadgetModel);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
