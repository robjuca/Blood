/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

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
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Test);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      AlertsModel = TAlertsModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void EditEnter (TGadgetTestModel gadget)
    {
      if (gadget.NotNull ()) {
        var entityAction = TEntityAction.CreateDefault;
        TGadgetTestActionComponent.Request (gadget, entityAction);

        ComponentModelProperty.SelectModel (entityAction);

        ValidateProperty ("TextProperty");
      }
    }

    internal void SelectModel (TEntityAction action)
    {
      ComponentModelProperty.SelectModel (action);
    }

    internal void RequestModel (TEntityAction action)
    {
      ComponentModelProperty.RequestModel (action);

      // update model
      //action.ModelAction.GadgetTargetModel.CopyFrom (action);
    }

    internal void ShowPanels ()
    {
      ComponentModelProperty.ShowPanels ();
    }

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
  };
  //---------------------------//

}  // namespace
