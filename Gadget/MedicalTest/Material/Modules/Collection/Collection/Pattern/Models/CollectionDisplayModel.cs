/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Shared.Gadget.Models.Action;

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

    public TGadgetMaterialModel GadgetModel
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
        return (GadgetModel.NotNull ());
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (GadgetModel.IsNull () ? false : (GadgetModel.Model.Enabled.IsFalse ()));
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
        return (GadgetModel.IsNull () ? Guid.Empty : GadgetModel.Id);
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
    internal void Select (TGadgetMaterialModel model)
    {
      GadgetModel = model ?? throw new System.ArgumentNullException (nameof (model));

      ComponentControlModel.SelectModel (GadgetModel);

      BusyVisibility = GadgetModel.BusyVisibility;
    }

    internal void RequestModel (TGadgetMaterialModel model)
    {
      model.ThrowNull ();

      model.CopyFrom (GadgetModel);
    }

    internal void Cleanup ()
    {
      GadgetModel = TGadgetMaterialModel.CreateDefault;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
