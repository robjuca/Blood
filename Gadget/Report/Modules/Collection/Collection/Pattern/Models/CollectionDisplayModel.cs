/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;

using Shared.Gadget.Report;
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

    public GadgetReport GadgetModel
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
      GadgetModel = GadgetReport.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TActionComponent component)
    {
      component.ThrowNull ();

      GadgetModel.CopyFrom (component.Models.GadgetReportModel);

      ComponentControlModel.SelectModel (component);

      BusyVisibility = GadgetModel.BusyVisibility;
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      component.Models.GadgetReportModel.CopyFrom (GadgetModel);
    }

    internal void Cleanup ()
    {
      GadgetModel.CopyFrom (GadgetReport.CreateDefault);
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
