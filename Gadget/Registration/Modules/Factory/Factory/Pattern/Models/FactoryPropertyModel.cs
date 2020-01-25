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
      ComponentModelProperty = TModelProperty.Create (TCategory.Registration);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      //TODO: what for??
      AlertsModel = TAlertsModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void SelectModel (TActionComponent component)
    {
      component.ThrowNull ();

      if (component.IsCategory(TCategory.Registration)) {
        var entityAction = TEntityAction.Create (TCategory.Registration);
        TActionConverter.Request (TCategory.Registration, component, entityAction);

        ComponentModelProperty.SelectModel (entityAction);
      }
    }

    internal void RequestModel (TEntityAction entityAction)
    {
      entityAction.ThrowNull ();

      ComponentModelProperty.RequestModel (entityAction);
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      var entityAction = TEntityAction.Create (TCategory.Registration);
      ComponentModelProperty.RequestModel (entityAction);

      TActionConverter.Select (TCategory.Registration, component, entityAction);
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
