/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;

using Server.Models.Action;
using Shared.Gadget.Models.Action;

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

    public TGadgetTestModel GadgetModel
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
      GadgetModel = TGadgetTestModel.CreateDefault;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TGadgetTestModel gadget)
    {
      gadget.ThrowNull ();

      GadgetModel.CopyFrom (gadget);

      ComponentControlModel.SelectModel (gadget);

      BusyVisibility = GadgetModel.BusyVisibility;
    }

    //internal void RequestModel (TEntityAction entityAction)
    //{
    //  entityAction.ThrowNull ();

    //  TGadgetTestActionComponent.Request (GadgetModel, entityAction);

    //  //ComponentControlModel.RequestComponents (action);

    //  //if (action.Param2 is Collection<GadgetTest> list) {
    //  //  if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Test)) {
    //  //    action.ModelAction.GadgetTestModel.UpdateContents (list);
    //  //  }
    //  //}
    //}

    internal void Cleanup ()
    {
      GadgetModel = TGadgetTestModel.CreateDefault;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
