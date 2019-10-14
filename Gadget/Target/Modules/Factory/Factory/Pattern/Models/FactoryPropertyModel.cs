/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

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
    #endregion

    #region Constructor
    public TFactoryPropertyModel ()
    {
      ComponentModelProperty = TModelProperty.Create (Server.Models.Infrastructure.TCategory.Target);
      ComponentModelProperty.PropertyChanged += OnModelPropertyChanged;
    }
    #endregion

    #region Members
    internal void RefreshModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        foreach (var item in action.CollectionAction.GadgetMaterialCollection) {
          // only enable item
          if (item.Enabled) {
            var selection = Server.Models.Infrastructure.TSelectionInfo.Create (item.Material, item.Id);
            selection.SetImage (item.GetImage ());

            action.SupportAction.SelectionCollection.Add (selection);
          }
        }

        ComponentModelProperty.ExtensionModel.SelectModel (Server.Models.Infrastructure.TCategory.Target, action); // update Selection Property (Material list)
      }
    }

    internal void SelectModel (Server.Models.Component.TEntityAction action)
    {
      // update Material selection
      var tag = action.ModelAction.GadgetTargetModel.MaterialId;

      action.SupportAction.SelectionInfo.Select (string.Empty, tag);

      ComponentModelProperty.SelectModel (action);
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        action.CollectionAction.ExtensionNodeCollection.Clear ();

        ComponentModelProperty.RequestModel (action);

        if (action.SupportAction.SelectionInfo.Tag is Guid materialId) {
          action.ModelAction.ExtensionNodeModel.ChildId = materialId;
          action.ModelAction.ExtensionNodeModel.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Material);
          action.ModelAction.ExtensionNodeModel.ParentId = ComponentModelProperty.Id;
          action.ModelAction.ExtensionNodeModel.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Target);

          // update collection
          action.CollectionAction.ExtensionNodeCollection.Add (action.ModelAction.ExtensionNodeModel);
        }

        // update model
        action.ModelAction.GadgetTargetModel.CopyFrom (action);
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
