/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public class TGadgetTestComponent
  {
    #region Property
    public GadgetMaterial GadgetMaterialModel
    {
      get;
    }

    public GadgetTest GadgetTestModel
    {
      get;
    }

    public GadgetTarget GadgetTargetModel
    {
      get;
    }

    public Guid Id
    {
      get
      {
        return (CurrentGadgetCategory.Equals (TCategory.Target) ? GadgetTargetModel.Id : CurrentGadgetCategory.Equals (TCategory.Test) ? GadgetTestModel.Id : Guid.Empty);
      }
    }

    public bool ValidateId
    {
      get
      {
        return (Id.NotEmpty ());
      }
    }

    public bool IsEmpty
    {
      get
      {
        return (Id.Equals (Guid.Empty));
      }
    }

    public bool IsChecked
    {
      get;
      set;
    }

    public string Name
    {
      get
      {
        return (CurrentGadgetCategory.Equals (TCategory.Target) ? GadgetTargetModel.Target : CurrentGadgetCategory.Equals (TCategory.Test) ? GadgetTestModel.Test : string.Empty);
      }
    }

    public Collection<byte> MaterialImage
    {
      get
      {
        return (GadgetMaterialModel.Image);
      }
    }

    public TCategory CurrentGadgetCategory
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TGadgetTestComponent ()
    {
      GadgetMaterialModel = GadgetMaterial.CreateDefault;
      GadgetTestModel = GadgetTest.CreateDefault;
      GadgetTargetModel = GadgetTarget.CreateDefault;

      IsChecked = false;
      CurrentGadgetCategory = TCategory.None;
    }
    #endregion

    #region Members
    public void Select (GadgetMaterial gadget)
    {
      GadgetMaterialModel.CopyFrom (gadget);
    }

    public void Select (GadgetTarget gadget)
    {
      GadgetTargetModel.CopyFrom (gadget);
    }

    public void Select (GadgetTest gadget)
    {
      GadgetTestModel.CopyFrom (gadget);
    }

    public void Select (TCategory category)
    {
      CurrentGadgetCategory = category;
    }

    public void Check (bool isChecked)
    {
      IsChecked = isChecked;
    }

    public bool Contains (Guid id)
    {
      return (Id.Equals (id));
    }

    public bool Contains (TGadgetTestComponent alias)
    {
      return (alias.NotNull () ? Id.Equals (alias.Id) : false);
    }

    public void CopyFrom (TGadgetTestComponent alias)
    {
      if (alias.NotNull ()) {
        GadgetMaterialModel.CopyFrom (alias.GadgetMaterialModel);
        GadgetTargetModel.CopyFrom (alias.GadgetTargetModel);
        GadgetTestModel.CopyFrom (alias.GadgetTestModel);

        CurrentGadgetCategory = alias.CurrentGadgetCategory;
        IsChecked = alias.IsChecked;
      }
    }
    #endregion

    #region Static
    public static TGadgetTestComponent CreateDefault => new TGadgetTestComponent ();
    #endregion
  }
  //---------------------------//

}  // namespace
