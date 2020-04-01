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
using Shared.Gadget.Models.Component;
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

      m_Registration = GadgetRegistration.CreateDefault;
    }
    #endregion

    #region Members
    internal void EditEnter (TActionComponent component)
    {
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Result)) {
          var gadget = component.Models.GadgetResultModel;

          if (gadget.HasRegistration) {
            gadget.RequestContent (m_Registration);
          }

          var entityAction = TEntityAction.CreateDefault;
          TActionConverter.Request (TCategory.Result, component, entityAction);

          ComponentModelProperty.SelectModel (entityAction);

          ValidateProperty ();
        }
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

    internal bool RequestModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        ComponentModelProperty.RequestModel (action);

        if (action.Param1 is TActionComponent component) {
          var gadget = component.Models.GadgetResultModel;

          if (gadget.HasRegistration) {
            if (m_Registration.Contains (gadget.RegistrationId).IsFalse ()) {
              gadget.RequestContent (m_Registration);
            }
          }
        }

        return (ValidateProperty ());
      }

      return (false);
    }

    internal void Select (GadgetRegistration gadget)
    {
      if (gadget.NotNull ()) {
        m_Registration.CopyFrom (gadget);

        ValidateProperty ();
      }
    }

    internal void ShowPanels ()
    {
      ComponentModelProperty.ShowPanels ();
    }

    internal void Cleanup ()
    {
      ComponentModelProperty.Cleanup ();

      m_Registration = GadgetRegistration.CreateDefault;
    }
    #endregion

    #region Event
    void OnModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      RaisePropertyChanged (e.PropertyName);
    }
    #endregion

    #region Fields
    GadgetRegistration                                          m_Registration; 
    #endregion

    #region Support
    bool ValidateProperty ()
    {
      AlertsModel.Select (isOpen: false); // default

      // TextProperty
      var textProperty = ComponentModelProperty.ExtensionModel.TextProperty;
      var emptyText = string.IsNullOrEmpty (textProperty);
      var emptyRegistration = m_Registration.ValidateId.IsFalse ();

      bool validateModel = emptyText.IsFalse () && emptyRegistration.IsFalse ();

      ComponentModelProperty.ValidateModel (validateModel);

      // show alerts
      if (validateModel.IsFalse ()) {
        string message = emptyText ? Properties.Resource.RES_TEXT_EMPTY : string.Empty;
        message += Environment.NewLine;
        message += emptyRegistration ? Properties.Resource.RES_REGISTRATION_EMPTY : string.Empty;

        AlertsModel.Select (TAlertsModel.TKind.Warning);
        AlertsModel.Select (Properties.Resource.RES_EMPTY, message);
        AlertsModel.Select (isOpen: true);
      }

      AlertsModel.Refresh ();

      return (validateModel);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
