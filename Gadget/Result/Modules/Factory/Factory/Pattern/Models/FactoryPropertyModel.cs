/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

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
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Result);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      AlertsModel = TAlertsModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void EditEnter (TActionComponent component)
    {
      if (component.NotNull ()) {
        var entityAction = TEntityAction.CreateDefault;
        TActionConverter.Request (TCategory.Result, component, entityAction);

        ComponentModelProperty.SelectModel (entityAction);

        ValidateProperty ("TextProperty");
      }
    }

    internal void ModifyEnter (TActionComponent component)
    {
      if (component.NotNull ()) {
        var entityAction = TEntityAction.CreateDefault;
        TActionConverter.Request (TCategory.Result, component, entityAction);

        ComponentModelProperty.SelectModel (entityAction);
        ComponentModelProperty.ValidateModel (validated: false);
        ComponentModelProperty.IsComponentModelEnabled = false;
        ComponentModelProperty.IsExtensionModelEnabled = false;
      }
    }

    internal void RequestModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        //action.CollectionAction.ExtensionNodeCollection.Clear ();

        ComponentModelProperty.RequestModel (action);

        
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

    #region Support
    internal void ValidateProperty (string propertyName)
    {
      if (propertyName.Equals ("TextProperty")) {
        AlertsModel.Select (isOpen: false); // default

        var textProperty = ComponentModelProperty.ExtensionModel.TextProperty;
        bool validateModel = string.IsNullOrEmpty (textProperty).IsFalse ();

        ComponentModelProperty.ValidateModel (validateModel);

        // show alerts
        if (validateModel.IsFalse ()) {
          var message = $"Test (Text = EMPTY)";

          AlertsModel.Select (TAlertsModel.TKind.Warning);
          AlertsModel.Select ("ENTRY EMPTY", message);
          AlertsModel.Select (isOpen: true);
        }

        AlertsModel.Refresh ();
      }
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
