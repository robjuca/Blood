/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Server.Models.Action;
using Server.Models.Infrastructure;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public static class TActionConverter
  {
    #region Static Members
    #region Collection
    public static void Collection (TCategory category, Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull () && entityAction.NotNull ()) {
        switch (category) {
          case TCategory.Material:
            TGadgetMaterialConverter.Collection (gadgets, entityAction);
            break;
         
          case TCategory.Target:
            TGadgetTargetConverter.Collection (gadgets, entityAction);
            break;
          
          case TCategory.Test:
            TGadgetTestConverter.Collection (gadgets, entityAction);
            break;
          
          case TCategory.Registration:
            TGadgetRegistrationConverter.Collection (gadgets, entityAction);
            break;
          
          case TCategory.Result:
            TGadgetResultConverter.Collection (gadgets, entityAction);
            break;
          
          case TCategory.Report:
            TGadgetReportConverter.Collection (gadgets, entityAction);
            break;
        }
      }
    }
    #endregion

    #region Select
    public static void Select (TCategory category, TActionComponent component, TEntityAction entityAction)
    {
      if (component.NotNull () && entityAction.NotNull ()) {
        switch (category) {
          case TCategory.Material:
            TGadgetMaterialConverter.Select (component, entityAction);
            break;

          case TCategory.Target:
            TGadgetTargetConverter.Select (component, entityAction);
            break;

          case TCategory.Test:
            TGadgetTestConverter.Select (component, entityAction);
            break;

          case TCategory.Registration:
            TGadgetRegistrationConverter.Select (component, entityAction);
            break;

          case TCategory.Result:
            TGadgetResultConverter.Select (component, entityAction);
            break;

          case TCategory.Report:
            TGadgetReportConverter.Select (component, entityAction);
            break;
        }
      }
    }

    public static void SelectMany (TCategory category, Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      if (gadgets.NotNull () && entityAction.NotNull ()) {
        switch (category) {
          case TCategory.Test:
            TGadgetTestConverter.SelectMany (gadgets, entityAction);
            break;

          case TCategory.Result:
            TGadgetResultConverter.SelectMany (gadgets, entityAction);
            break;
        }
      }
    }
    #endregion

    #region Request
    public static void Request (TCategory category, TActionComponent component, TEntityAction entityAction)
    {
      if (component.NotNull () && entityAction.NotNull ()) {
        switch (category) {
          case TCategory.Material:
            TGadgetMaterialConverter.Request (component, entityAction);
            break;

          case TCategory.Target:
            TGadgetTargetConverter.Request (component, entityAction);
            break;

          case TCategory.Test:
            TGadgetTestConverter.Request (component, entityAction);
            break;

          case TCategory.Registration:
            TGadgetRegistrationConverter.Request (component, entityAction);
            break;

          case TCategory.Result:
            TGadgetResultConverter.Request (component, entityAction);
            break;

          case TCategory.Report:
            TGadgetReportConverter.Request (component, entityAction);
            break;
        }
      }
    }

    public static void ModifyValue (TCategory category, TActionComponent component, TEntityAction entityAction)
    {
      if (component.NotNull () && entityAction.NotNull ()) {
        switch (category) {
          case TCategory.Result:
            TGadgetResultConverter.ModifyValue (component, entityAction);
            break;
        }
      }
    }

    public static void ModifyStatus (TCategory category, TActionComponent component, TEntityAction entityAction)
    {
      if (component.NotNull () && entityAction.NotNull ()) {
        switch (category) {
          case TCategory.Result:
            TGadgetResultConverter.ModifyStatus (component, entityAction);
            break;
        }
      }
    }
    #endregion
    #endregion
  };
  //---------------------------//

}  // namespace