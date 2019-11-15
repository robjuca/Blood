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

    public int RelationCategoryValue
    {
      get
      {
        return (ControlModel.RelationCategory);
      }
    }

    public Server.Models.Infrastructure.TCategory RelationCategory
    {
      get
      {
        return (Server.Models.Infrastructure.TCategoryType.FromValue (RelationCategoryValue));
      }
    }

    public Collection<TComponentModelItem> Targets
    {
      get; 
    }

    public Collection<TComponentControlModel> RelationControlModels
    {
      get;
    }

    public bool HasRelationModels
    {
      get
      {
        return (RelationControlModels.Count.Equals (0).IsFalse ());
      }
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = Server.Models.Component.GadgetTest.CreateDefault;

      Targets = new Collection<TComponentModelItem> ();
      RelationControlModels = new Collection<TComponentControlModel> ();
    }
    #endregion

    #region Members
    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        Targets.Clear ();
        RelationControlModels.Clear ();

        ControlModel.CopyFrom (action.ModelAction.GadgetTestModel);

        foreach (var targetId in ControlModel.Targets) {
          if (action.CollectionAction.EntityCollection.ContainsKey (targetId)) {
            var targetAction = action.CollectionAction.EntityCollection [targetId];

            Targets.Add (TComponentModelItem.Create (targetAction));

            if (targetAction.CategoryType.Category.Equals (Server.Models.Infrastructure.TCategory.Test)) {
              var controlModel = TComponentControlModel.CreateDefault;
              controlModel.SelectModel (targetAction);

              RelationControlModels.Add (controlModel);
            }
          }
        }
      }
    }

    public void SelectTargets (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        Targets.Clear ();
        RelationControlModels.Clear ();

        if (action.Param1 is Collection<TComponentModelItem> targets) {
          foreach (var item in targets) {
            Targets.Add (item);
          }
        }

        if (action.Param2 is Collection<TComponentControlModel> relations) {
          foreach (var item in relations) {
            RelationControlModels.Add (item);
          }
        }
      }
    }

    public void RequestTargets (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        action.Param1 = Targets;
        action.Param2 = RelationControlModels;
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

    public bool IsRelationCategory (Server.Models.Infrastructure.TCategory category)
    {
      return (RelationCategory.Equals (category));
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        Cleanup ();

        ControlModel.CopyFrom (alias.ControlModel);

        foreach (var item in alias.Targets) {
          Targets.Add (item);
        }

        foreach (var item in alias.RelationControlModels) {
          RelationControlModels.Add (item);
        }
      }
    }

    public TComponentControlModel RequestRelationModel (Guid id)
    {
      foreach (var item in RelationControlModels) {
        if (item.ControlModel.Id.Equals (id)) {
          return (item);
        }
      }

      return (null);
    }
    public void Cleanup ()
    {
      ControlModel.CopyFrom (Server.Models.Component.GadgetTest.CreateDefault);

      Targets.Clear ();
      RelationControlModels.Clear ();
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

  //----- TRelationComponentControlModel
  public class TRelationComponentControlModel
  {
    #region Property
    public TComponentControlModel ControlModel
    {
      get;
      set;
    }

    public TComponentModelItem ModelItem
    {
      get; 
    }
    #endregion

    #region Constructor
    TRelationComponentControlModel (TComponentControlModel controlModel, TComponentModelItem modelItem)
      : this ()
    {
      if (controlModel.NotNull ()) {
        ControlModel.CopyFrom (controlModel);
      }

      if (modelItem.NotNull ()) {
        ModelItem.CopyFrom (modelItem);
      }
    }

    TRelationComponentControlModel (TComponentModelItem modelItem)
      : this ()
    {
      if (modelItem.NotNull ()) {
        ModelItem.CopyFrom (modelItem);
      }
    }

    TRelationComponentControlModel ()
    {
      ControlModel = TComponentControlModel.CreateDefault;
      ModelItem = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Static
    public static TRelationComponentControlModel Create (TComponentModelItem modelItem) => new TRelationComponentControlModel (modelItem);

    public static TRelationComponentControlModel Create (TComponentControlModel controlModel, TComponentModelItem modelItem) => new TRelationComponentControlModel (controlModel, modelItem); 
    #endregion
  };
  //---------------------------//

}  // namespace