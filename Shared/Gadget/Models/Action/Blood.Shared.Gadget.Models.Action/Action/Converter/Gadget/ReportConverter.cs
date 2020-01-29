/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public static class TGadgetReportConverter
  {
    #region Static Members
    public static void Collection (Collection<TActionComponent> gadgets, TEntityAction entityAction)
    {
      gadgets.Clear ();

      if (entityAction.CategoryType.IsCategory (TCategory.Report)) {
        var gadgetCollection = new Collection<GadgetReport> ();

        foreach (var item in entityAction.CollectionAction.ModelCollection) {
          var modelAction = item.Value;
          var gadget = GadgetReport.CreateDefault;

          gadget.Id = modelAction.ComponentInfoModel.Id;
          gadget.GadgetName = modelAction.ExtensionTextModel.Text;
          gadget.Description = modelAction.ExtensionTextModel.Description;
          gadget.ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
          gadget.SetDate (modelAction.ExtensionTextModel.Date);
          gadget.Enabled = modelAction.ComponentInfoModel.Enabled;

          gadget.GadgetInfo = modelAction.ComponentInfoModel.Name;
          gadget.Busy = modelAction.ComponentStatusModel.Busy;

          gadgetCollection.Add (gadget);
        }

        // sort
        var list = gadgetCollection
          .OrderBy (p => p.GadgetInfo)
          .ToList ()
        ;

        foreach (var model in list) {
          var component = TActionComponent.Create (TCategory.Report);
          component.Models.GadgetReportModel.CopyFrom (model);

          gadgets.Add (component);
        }
      }
    }

    public static void Select (TActionComponent component, TEntityAction entityAction)
    {
      if (entityAction.CategoryType.IsCategory (TCategory.Report)) {
        component.Select (TCategory.Report);

        var gadget = component.Models.GadgetReportModel;

        gadget.Id = entityAction.ModelAction.ComponentInfoModel.Id;
        gadget.GadgetName = entityAction.ModelAction.ExtensionTextModel.Text;
        gadget.SetDate (entityAction.ModelAction.ExtensionTextModel.Date);
        gadget.Description = entityAction.ModelAction.ExtensionTextModel.Description;
        gadget.ExternalLink = entityAction.ModelAction.ExtensionTextModel.ExternalLink;
        gadget.Enabled = entityAction.ModelAction.ComponentInfoModel.Enabled;
        
        gadget.GadgetInfo = entityAction.ModelAction.ComponentInfoModel.Name;
        gadget.Busy = entityAction.ModelAction.ComponentStatusModel.Busy;
      }
    }

    public static void Request (TActionComponent component, TEntityAction entityAction)
    {
      if (component.IsCategory (TCategory.Report)) {
        var gadget = component.Models.GadgetReportModel;

        entityAction.Id = gadget.Id;
        entityAction.CategoryType.Select (TCategory.Report);

        entityAction.ModelAction.ComponentInfoModel.Name = gadget.GadgetInfo;
        entityAction.ModelAction.ComponentStatusModel.Busy = gadget.Busy;

        entityAction.ModelAction.ComponentInfoModel.Id = gadget.Id;
        entityAction.ModelAction.ExtensionTextModel.Text = gadget.GadgetName;
        entityAction.ModelAction.ExtensionTextModel.Date = gadget.Date;
        entityAction.ModelAction.ExtensionTextModel.Description = gadget.Description;
        entityAction.ModelAction.ComponentInfoModel.Enabled = gadget.Enabled;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace