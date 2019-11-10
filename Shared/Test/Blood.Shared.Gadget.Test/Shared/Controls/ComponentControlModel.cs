/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Shared.ViewModel;
//---------------------------//

namespace Shared.Gadget.Test
{
  public class TComponentControlModel
  {
    #region Property
    public Server.Models.Component.GadgetTest ControlModel
    {
      get;
    }

    public Collection<TComponentModelItem> Targets
    {
      get; 
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = Server.Models.Component.GadgetTest.CreateDefault;

      Targets = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        Targets.Clear ();

        ControlModel.CopyFrom (action.ModelAction.GadgetTestModel);

        foreach (var targetId in ControlModel.Targets) {
          if (action.CollectionAction.EntityCollection.ContainsKey (targetId)) {
            var targetAction = action.CollectionAction.EntityCollection [targetId];

            Targets.Add (TComponentModelItem.Create (targetAction));
          }
        }
      }
    }

    public void SelectTargets (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        Targets.Clear ();

        if (action.Param1 is Collection<TComponentModelItem> targets) {
          foreach (var item in targets) {
            Targets.Add (item);
          }
        }
      }
    }

    public void RequestTargets (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        action.Param1 = Targets;
      }
    }

    public void AddTarget (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        var modelItem = SelectTarget (item.Id);

        if (modelItem.ValidateId.IsFalse ()) {
          Targets.Add (item);
        }
      }
    }

    public void RemoveTarget (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        var modelItem = SelectTarget (item.Id);

        if (modelItem.ValidateId) {
          Targets.Remove (modelItem);
        }
      }
    }

    public void Cleanup ()
    {
      ControlModel.CopyFrom (Server.Models.Component.GadgetTest.CreateDefault);
      Targets.Clear ();
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion

    #region Support
    TComponentModelItem SelectTarget (Guid id)
    {
      foreach (var item in Targets) {
        if (item.Id.Equals (id)) {
          return (item);
        }
      }

      return (TComponentModelItem.CreateDefault);
    } 
    #endregion
  };
  //---------------------------//

}  // namespace