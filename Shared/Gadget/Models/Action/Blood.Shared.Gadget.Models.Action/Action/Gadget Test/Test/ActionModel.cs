/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public sealed class TGadgetTestModel : TActionModelBase<GadgetTest>
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

    public int TargetCount
    {
      get
      {
        return (Model.ContentCount);
      }
    }

    public TCategory RelationCategory
    {
      get
      {
        return (Model.RequestCategory ());
      }
    }

    public bool HasTarget
    {
      get
      {
        return (TargetCount.Equals (0).IsFalse ());
      }
    }

    public Visibility TestRelationVisibility
    {
      get
      {
        return (HasTarget ? RelationCategory.Equals (TCategory.Test) ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed);
      }
    }

    public Visibility TargetRelationVisibility
    {
      get
      {
        return (HasTarget ? RelationCategory.Equals (TCategory.Target) ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed);
      }
    }

    public Visibility DisableVisibility
    {
      get
      {
        return (Model.Enabled ? Visibility.Collapsed : Visibility.Visible);
      }
    }
    #endregion

    #region Constructor
    TGadgetTestModel (string name, bool busy)
      : base (GadgetTest.CreateDefault)
    {
      Select (name, busy);
    }

    TGadgetTestModel (GadgetTest gadget)
      : base (gadget)
    {
    }

    TGadgetTestModel ()
      : base (GadgetTest.CreateDefault)
    {
    }
    #endregion

    #region Members
    public void CopyFrom (TGadgetTestModel alias)
    {
      if (alias.NotNull ()) {
        Select (alias.Name, alias.Busy);

        Model.CopyFrom (alias.Model);
      }
    }
    #endregion

    #region Static
    public static TGadgetTestModel Create (GadgetTest gadget) => new TGadgetTestModel (gadget);
    public static TGadgetTestModel Create (string name, bool busy) => new TGadgetTestModel (name, busy);
    public static TGadgetTestModel CreateDefault => new TGadgetTestModel ();
    #endregion
  };
  //---------------------------//

}  // namespace