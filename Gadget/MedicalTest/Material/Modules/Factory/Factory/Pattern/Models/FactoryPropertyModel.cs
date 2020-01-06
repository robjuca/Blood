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
using Server.Models.Gadget;

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
      ComponentModelProperty = TModelProperty.Create (TCategory.Material);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      AlertsModel = TAlertsModel.CreateDefault;

      m_Gadgets = new Dictionary<Guid, GadgetMaterial> ();
    }
    #endregion

    #region Members
    internal void RefreshModel (TEntityAction action)
    {
      action.ThrowNull ();

      m_Gadgets.Clear ();

      //foreach (var model in action.CollectionAction.GadgetMaterialCollection) {
      //  m_Gadgets.Add (model.Id, model);
      //}
    }

    internal void SelectModel (TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelProperty.SelectModel (action);
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelProperty.RequestModel (action);

      // update model
      //action.ModelAction.GadgetMaterialModel.CopyFrom (action);
    }

    internal void ShowPanels ()
    {
      ComponentModelProperty.ShowPanels ();
    }

    internal void ValidateProperty (string propertyName)
    {
      if (propertyName.Equals ("TextProperty")) {
        AlertsModel.Select (isOpen: false); // default

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

              break;
            }
          }
        }

        AlertsModel.Refresh ();
      }
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
