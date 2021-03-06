﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Globalization;

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

    public TActionComponent GadgetModel
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

      GadgetModel = TActionComponent.Create (TCategory.Material);

      AlertsModel = TAlertsModel.CreateDefault;

      m_Gadgets = new Dictionary<Guid, GadgetMaterial> ();
    }
    #endregion

    #region Members
    internal void RefreshModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      m_Gadgets.Clear ();
    }

    internal void SelectModel (TActionComponent component)
    {
      component.ThrowNull ();

      GadgetModel.Models.GadgetMaterialModel.CopyFrom (component.Models.GadgetMaterialModel);

      var entityAction = TEntityAction.CreateDefault;
      TActionConverter.Request (TCategory.Material, component, entityAction);

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

      if (propertyName.Equals ("TextProperty", StringComparison.InvariantCulture)) {
        AlertsModel.Select (isOpen: false); // default

        ComponentModelProperty.ValidateModel (true);

        var entityAction = TEntityAction.CreateDefault;
        entityAction.CategoryType.Select (TCategory.Material);

        RequestModel (entityAction);

        // test empty
        if (string.IsNullOrEmpty (entityAction.ModelAction.ExtensionTextModel.Text)) {
          ComponentModelProperty.ValidateModel (false);

          // show alerts
          AlertsModel.Select (TAlertsModel.TKind.Warning);
          AlertsModel.Select (Properties.Resource.RES_EMPTY, Properties.Resource.RES_TEXT_EMPTY);
          AlertsModel.Select (isOpen: true);

          res = false;
        }

        // test duplicated
        else {
          foreach (var gadget in m_Gadgets) {
            var material = gadget.Value.Material;
            var item = ComponentModelProperty.ExtensionModel.TextProperty;

            bool validateModel = string.Compare (material, item, true, CultureInfo.InvariantCulture).Equals (0).IsFalse ();

            // check same gadget (change)
            if (gadget.Value.Id.Equals (ComponentModelProperty.Id).IsFalse ()) {
              ComponentModelProperty.ValidateModel (validateModel);

              // show alerts
              if (validateModel.IsFalse ()) {
                var message = $"Material (Text = {material})";

                AlertsModel.Select (TAlertsModel.TKind.Warning);
                AlertsModel.Select (Properties.Resource.RES_DUPLICATED, message);
                AlertsModel.Select (isOpen: true);

                res = false;

                break;
              }
            }
          }
        }

        AlertsModel.Refresh ();

        if (res) {
          //TGadgetMaterialActionComponent.Select (GadgetModel, entityAction);
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
