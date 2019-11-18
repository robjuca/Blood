/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

using Shared.Resources;
using Shared.Types;
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
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Registration);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      AlertsModel = TAlertsModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void RefreshModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        if (action.CollectionAction.GadgetMaterialCollection.Count.Equals (0)) {
          ComponentModelProperty.ValidateModel (false);

          // show alerts
          var message = $"Material list is EMPTY!";

          AlertsModel.Select (TAlertsModel.TKind.Warning);
          AlertsModel.Select ("EMPTY", message);
          AlertsModel.Select (isOpen: true);
        }

        else {
          foreach (var item in action.CollectionAction.GadgetMaterialCollection) {
            var selection = Server.Models.Infrastructure.TSelectionInfo.Create (item.Material, item.Id, item.Enabled);
            selection.SetImage (item.GetImage ());

            action.SupportAction.SelectionCollection.Add (selection);
          }

          ComponentModelProperty.ValidateModel (true);
          ComponentModelProperty.ExtensionModel.SelectModel (Server.Models.Infrastructure.TCategory.Registration, action); // update Selection Property (Material list)

          AlertsModel.Select (isOpen: false); // default
        }
      }
    }

    internal void SelectModel (Server.Models.Component.TEntityAction action)
    {
      // TODO: review
      // update Material selection
      //var tag = action.ModelAction.GadgetRegistrationModel.MaterialId;

      //action.SupportAction.SelectionInfo.Select (string.Empty, tag, enabled: true);

      //ComponentModelProperty.SelectModel (action);
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        action.CollectionAction.ExtensionNodeCollection.Clear ();

        ComponentModelProperty.RequestModel (action);

        if (action.SupportAction.SelectionInfo.Tag is Guid materialId) {
          action.ModelAction.ComponentStatusModel.UseNodeModel = true;
          action.ModelAction.ComponentStatusModel.NodeReverse = true;

          //  Here gadget Material must be Parent
          action.ModelAction.ExtensionNodeModel.ChildId = ComponentModelProperty.Id;
          action.ModelAction.ExtensionNodeModel.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Registration);
          action.ModelAction.ExtensionNodeModel.ParentId = materialId;
          action.ModelAction.ExtensionNodeModel.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Material);

          // update collection
          action.CollectionAction.ExtensionNodeCollection.Add (action.ModelAction.ExtensionNodeModel);
        }

        // update model
        action.ModelAction.GadgetRegistrationModel.CopyFrom (action);
      }
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
