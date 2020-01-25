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

using Shared.Gadget.Registration;
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

    public GadgetRegistration GadgetModel
    {
      get;
    }

    public string Name
    {
      get
      {
        return (GadgetModel.GadgetInfo);
      }
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
      ComponentControlModel = TComponentControlModel.CreateDefault;
      GadgetModel = GadgetRegistration.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TActionComponent component)
    {
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Registration)) {
          GadgetModel.CopyFrom (component.Models.GadgetRegistrationModel);

          ComponentControlModel.SelectModel (component);

          BusyVisibility = component.Models.GadgetRegistrationModel.BusyVisibility;
        }
      }
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetRegistrationModel.CopyFrom (GadgetModel);
    }

    internal void Cleanup ()
    {
      GadgetModel.CopyFrom (GadgetRegistration.CreateDefault);
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
