/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using rr.Library.Types;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Resources;
using Shared.Types;
using Shared.Gadget.Models.Action;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryPropertyModel : NotificationObject
  {
    #region Property
    public TModelProperty ComponentModelProperty
    {
      get;
      set;
    }

    public TAlertsModel AlertsModel
    {
      get;
    }
    #endregion

    #region Constructor
    public TFactoryPropertyModel ()
    {
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Target);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      AlertsModel = TAlertsModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void RefreshModel (TEntityAction entityAction)
    {
      if (entityAction.NotNull ()) {
        var gadgets = new Collection<TGadgetMaterialModel> ();
        TGadgetMaterialActionComponent.Select (gadgets, entityAction);

        if (gadgets.Any ()) {
          foreach (var item in gadgets) {
            var selection = Server.Models.Infrastructure.TSelectionInfo.Create (item.Model.Material, item.Id, item.Enabled);
            selection.SetImage (item.Model.GetImage ());

            entityAction.SupportAction.SelectionCollection.Add (selection);
          }

          ComponentModelProperty.ValidateModel (true);
          ComponentModelProperty.ExtensionModel.SelectModel (TCategory.Target, entityAction); // update Selection Property (Material list)

          AlertsModel.Select (isOpen: false); // default
        }

        else {
          ComponentModelProperty.ValidateModel (false);

          // show alerts
          var message = $"Material list is EMPTY!";

          AlertsModel.Select (TAlertsModel.TKind.Warning);
          AlertsModel.Select ("EMPTY", message);
          AlertsModel.Select (isOpen: true);
        }
      }
    }

    internal void SelectModel (TGadgetTargetModel model)
    {
      model.ThrowNull ();

      var entityAction = TEntityAction.CreateDefault;
      TGadgetTargetActionComponent.Request (model, entityAction);

      // update Material selection
      var tag = model.Model.MaterialId;
      entityAction.SupportAction.SelectionInfo.Select (model.MaterialModel.Model.Material, tag, enabled: model.Enabled);
      entityAction.SupportAction.SelectionInfo.SetImage (model.MaterialModel.Model.GetImage ());

      ComponentModelProperty.SelectModel (entityAction);
      ComponentModelProperty.SelectionLock (model.Busy);
    }

    internal void RequestModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        action.CollectionAction.ExtensionNodeCollection.Clear ();

        ComponentModelProperty.RequestModel (action);

        if (action.SupportAction.SelectionInfo.Tag is Guid materialId) {
          action.ModelAction.ComponentStatusModel.UseNodeModel = true;
          action.ModelAction.ComponentStatusModel.NodeReverse = true;

          //  Here gadget Material must be Parent
          action.ModelAction.ExtensionNodeModel.ChildId = ComponentModelProperty.Id;
          action.ModelAction.ExtensionNodeModel.ChildCategory = TCategoryType.ToValue (TCategory.Target);
          action.ModelAction.ExtensionNodeModel.ParentId = materialId;
          action.ModelAction.ExtensionNodeModel.ParentCategory = TCategoryType.ToValue (TCategory.Material);

          // update collection
          action.CollectionAction.ExtensionNodeCollection.Add (action.ModelAction.ExtensionNodeModel);
        }
      }
    }

    internal bool ValidateProperty (string propertyName)
    {
      bool res = true;

      if (propertyName.Equals ("TextProperty")) {
        AlertsModel.Select (isOpen: false); // default

        ComponentModelProperty.ValidateModel (true);

        var entityAction = TEntityAction.CreateDefault;
        entityAction.CategoryType.Select (TCategory.Material);

        RequestModel (entityAction);

        // test empty
        if (string.IsNullOrEmpty (entityAction.ModelAction.ExtensionTextModel.Text)) {
          ComponentModelProperty.ValidateModel (false);

          // show alerts
          var message = $"Material (Text = EMPTY)";

          AlertsModel.Select (TAlertsModel.TKind.Warning);
          AlertsModel.Select ("EMPTY ENTRY", message);
          AlertsModel.Select (isOpen: true);

          res = false;
        }

        AlertsModel.Refresh ();
      }

      return (res);
    }

    internal void ShowPanels ()
    {
      ComponentModelProperty.ShowPanels ();
    }

    internal void Cleanup ()
    {
      ComponentModelProperty.Cleanup ();
    }
    #endregion

    #region Event
    void OnModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      RaisePropertyChanged (e.PropertyName);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
