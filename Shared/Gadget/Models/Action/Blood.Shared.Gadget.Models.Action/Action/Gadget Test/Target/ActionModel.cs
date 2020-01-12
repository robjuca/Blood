/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Collections.ObjectModel;

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public sealed class TGadgetTargetModel : TActionModelBase<GadgetTarget>
  {
    #region Property
    public Guid Id
    {
      get
      {
        return (Model.Id);
      }
    }

    public bool Enabled
    {
      get
      {
        return (Model.Enabled);
      }
    }

    public bool CanRemove
    {
      get
      {
        return (Busy.IsFalse () && Model.Enabled.IsFalse () && ValidateId);
      }
    }

    public bool ValidateId
    {
      get
      {
        return (Id.NotEmpty ());
      }
    }

    public Visibility DisableVisibility
    {
      get
      {
        return (Model.Enabled ? Visibility.Collapsed : Visibility.Visible);
      }
    }

    public Collection<byte> MaterialImage 
    {
      get
      {
        return (MaterialModel.Model.Image);
      }
    }

    public TGadgetMaterialModel MaterialModel
    {
      get; 
    }
    #endregion

    #region Constructor
    TGadgetTargetModel (string name, bool busy)
      : base (GadgetTarget.CreateDefault)
    {
      Select (name, busy);

      MaterialModel = TGadgetMaterialModel.CreateDefault;
    }

    TGadgetTargetModel ()
      : base (GadgetTarget.CreateDefault)
    {
      MaterialModel = TGadgetMaterialModel.CreateDefault;
    }
    #endregion

    #region Members
    public void CopyFrom (TGadgetTargetModel alias)
    {
      if (alias.NotNull ()) {
        Select (alias.Name, alias.Busy);

        Model.CopyFrom (alias.Model);
        MaterialModel.CopyFrom (alias.MaterialModel);
      }
    } 
    #endregion

    #region Static
    public static TGadgetTargetModel Create (string name, bool busy) => new TGadgetTargetModel (name, busy);

    public static TGadgetTargetModel CreateDefault => new TGadgetTargetModel ();
    #endregion
  };
  //---------------------------//

}  // namespace