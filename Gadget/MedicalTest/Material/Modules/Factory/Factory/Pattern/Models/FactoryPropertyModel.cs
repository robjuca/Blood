/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using rr.Library.Types;

using Server.Models.Infrastructure;
using Server.Models.Action;
using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;

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

    public TGadgetMaterialModel GadgetModel
    {
      get; 
    }

    public TAlertsModel AlertsModel
    {
      get;
    }
    #endregion

    #region Constructor
    public TFactoryPropertyModel ()
    {
      ComponentModelProperty = TModelProperty.Create (TCategory.Material);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      GadgetModel = TGadgetMaterialModel.CreateDefault;

      AlertsModel = TAlertsModel.CreateDefault;

      m_Gadgets = new Dictionary<Guid, GadgetMaterial> ();
    }
    #endregion

    #region Members
    internal void RefreshModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      m_Gadgets.Clear ();

      //foreach (var model in action.CollectionAction.GadgetMaterialCollection) {
      //  m_Gadgets.Add (model.Id, model);
      //}
    }

    internal void SelectModel (TGadgetMaterialModel model)
    {
      model.ThrowNull ();

      GadgetModel.CopyFrom (model);

      var entityAction = TEntityAction.CreateDefault;
      TActionComponent.Request (model, entityAction);

      ComponentModelProperty.SelectModel (entityAction);
    }

    internal void RequestModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      ComponentModelProperty.RequestModel (entityAction);
    }

    internal void ShowPanels ()
    {
      ComponentModelProperty.ShowPanels ();
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

        // test duplicated
        else {
          foreach (var gadget in m_Gadgets) {
            var material = gadget.Value.Material;
            var item = ComponentModelProperty.ExtensionModel.TextProperty;

            bool validateModel = string.Compare (material, item, true).Equals (0).IsFalse ();

            // check same gadget (change)
            if (gadget.Value.Id.Equals (ComponentModelProperty.Id).IsFalse ()) {
              ComponentModelProperty.ValidateModel (validateModel);

              // show alerts
              if (validateModel.IsFalse ()) {
                var message = $"Material (Text = {material})";

                AlertsModel.Select (TAlertsModel.TKind.Warning);
                AlertsModel.Select ("DUPLICATED ENTRY", message);
                AlertsModel.Select (isOpen: true);

                res = false;

                break;
              }
            }
          }
        }

        AlertsModel.Refresh ();

        if (res) {
          TActionComponent.Select (GadgetModel, entityAction);
        }
      }

      return (res);
    }

    internal void Cleanup ()
    {
      ComponentModelProperty.Cleanup ();

      AlertsModel.Select (isOpen: false); // default
      AlertsModel.Refresh ();
    }
    #endregion

    #region Event
    void OnModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      RaisePropertyChanged (e.PropertyName);
    }
    #endregion

    #region Fields
    readonly Dictionary<Guid, GadgetMaterial>                         m_Gadgets;
    #endregion
  };
  //---------------------------//

}  // namespace
