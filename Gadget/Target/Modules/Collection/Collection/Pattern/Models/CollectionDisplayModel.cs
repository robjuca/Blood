/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Server.Models.Action;

using Shared.ViewModel;
using Shared.Gadget.Models.Action;

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

    public TGadgetTargetModel GadgetModel
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
      GadgetModel = TGadgetTargetModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TGadgetTargetModel model)
    {
      GadgetModel = model ?? throw new System.ArgumentNullException (nameof (model));
      
      ComponentControlModel.SelectModel (model);

      BusyVisibility = GadgetModel.BusyVisibility;
    }

    internal void Cleanup ()
    {
      GadgetModel = TGadgetTargetModel.CreateDefault;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
