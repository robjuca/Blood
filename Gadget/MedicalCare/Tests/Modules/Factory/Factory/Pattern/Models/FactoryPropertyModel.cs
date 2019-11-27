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
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Tests);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      AlertsModel = TAlertsModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        action.CollectionAction.ExtensionNodeCollection.Clear ();

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
  };
  //---------------------------//

}  // namespace
