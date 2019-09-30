/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using rr.Library.Types;

using Server.Models.Component;

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

    public bool ErrorTitle
    {
      get; 
      set;
    }

    public bool ErrorCaption
    {
      get;
      set;
    }

    public bool ErrorMessage
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TFactoryPropertyModel ()
    {
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Material);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;

      m_Gadgets = new Dictionary<Guid, GadgetMaterial> ();
    }
    #endregion

    #region Members
    internal void RefreshModel (TEntityAction action)
    {
      action.ThrowNull ();

      m_Gadgets.Clear ();

      foreach (var model in action.CollectionAction.GadgetMaterialCollection) {
        m_Gadgets.Add (model.Id, model);
      }
    }

    internal void SelectModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelProperty.SelectModel (action);
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelProperty.RequestModel (action);

      // update model
      action.ModelAction.GadgetMaterialModel.CopyFrom (action);
    }

    internal void ShowPanels ()
    {
      ComponentModelProperty.ShowPanels ();
    }

    internal void ValidateProperty (string propertyName)
    {
      if (propertyName.Equals ("TextProperty")) {
        foreach (var gadget in m_Gadgets) {
          var material = gadget.Value.Material;
          var item = ComponentModelProperty.ExtensionModel.TextProperty;

          bool validateModel = string.Compare (material, item, true).Equals (0).IsFalse ();

          ComponentModelProperty.ValidateModel (validateModel);
        }
      }
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

    #region Fields
    readonly Dictionary<Guid, Server.Models.Component.GadgetMaterial>                         m_Gadgets;
    #endregion
  };
  //---------------------------//

}  // namespace
