/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetTest
  {
    #region Property
    public int TargetCount
    {
      get
      {
        return (Targets.Count);
      }
    }

    public bool CanRemove
    {
      get
      {
        return (Enabled.IsFalse () && TargetCount.Equals (0));
      }
    }

    public bool HasRelation
    {
      get
      {
        return (Server.Models.Infrastructure.TCategoryType.FromValue (RelationCategory).Equals (Server.Models.Infrastructure.TCategory.Test));
      }
    }
    #endregion

    #region Constructor
    public GadgetTest ()
    {
      Id = Guid.Empty;

      Test = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      Enabled = false;

      RelationCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.None);

      Targets = new Collection<Guid> ();
      Relations = new Collection<GadgetTest> ();
    }

    public GadgetTest (GadgetTest alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (TEntityAction action)
    {
      if (action.NotNull ()) {
        CopyFrom (action.ModelAction);

        Targets.Clear ();

        // update
        if (action.CollectionAction.ComponentRelationCollection.Count.Equals (0)) {
          foreach (var item in action.CollectionAction.ComponentOperation.ParentCategoryCollection) {
            foreach (var relation in item.Value) {
              action.CollectionAction.ComponentRelationCollection.Add (relation);
            }
          }
        }

        foreach (var item in action.CollectionAction.ComponentRelationCollection) {
          if (item.ParentId.IsEmpty ()) {
            Targets.Add (item.ChildId);
          }

          else {
            if (item.ParentId.Equals (Id)) {
              Targets.Add (item.ChildId);
            }
          }
        }
      }
    }

    public void CopyFrom (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;

        Test = alias.Test;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;

        RelationCategory = alias.RelationCategory;

        Targets = new Collection<Guid> (alias.Targets);
        Relations = new Collection<GadgetTest> (alias.Relations);
      }
    }

    public void Change (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Test = alias.Test;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;

        RelationCategory = alias.RelationCategory;

        Targets = new Collection<Guid> (alias.Targets);
        Relations = new Collection<GadgetTest> (alias.Relations);
      }
    }

    public void RefreshModel (TEntityAction action)
    {
      // TODO: review
      if (action.NotNull ()) {
        if (action.CategoryType.IsCategory (Infrastructure.TCategory.Test)) {
          // update model action
          CopyFrom (action.ModelAction); // my self

          var relCategory = Server.Models.Infrastructure.TCategoryType.FromValue (RelationCategory);

          // target list
          foreach (var item in action.ComponentOperation.ParentIdCollection) {
            foreach (var relation in item.Value) {
              Targets.Add (relation.ChildId);

              if (relCategory.Equals (Server.Models.Infrastructure.TCategory.None)) {
                relCategory = Server.Models.Infrastructure.TCategoryType.FromValue (relation.ChildCategory);
              }
            }
          }

          // update
          RelationCategory = Server.Models.Infrastructure.TCategoryType.ToValue (relCategory);

          action.ModelAction.GadgetTestModel.CopyFrom (this);

          action.CollectionAction.GadgetTestCollection.Clear ();

          // update model collection
          foreach (var modelAction in action.CollectionAction.ModelCollection) {
            action.ModelAction.CopyFrom (modelAction.Value);

            var gadget = GadgetTest.CreateDefault;
            gadget.CopyFrom (action);

            modelAction.Value.GadgetTestModel.CopyFrom (gadget); // update colection

            action.CollectionAction.GadgetTestCollection.Add (gadget);
          }
        }
      }
    }

    public void UpdateModel (TEntityAction action)
    {
      Relations.Clear ();

      var relCategory = Server.Models.Infrastructure.TCategoryType.FromValue (RelationCategory);

      if (relCategory.Equals (Server.Models.Infrastructure.TCategory.None)) {
        if (TargetCount.Equals (0).IsFalse ()) {
          var targetId = Targets [0];

          foreach (var item in action.ComponentOperation.ParentCategoryCollection) {
            foreach (var relation in item.Value) {
              if (relation.ParentId.Equals (Id) && relation.ChildId.Equals (targetId)) {
                RelationCategory = relation.ChildCategory;

                if (HasRelation) {
                  foreach (var gadget in action.CollectionAction.GadgetTestCollection) {
                    if (ContainsTarget (gadget)) {
                      Relations.Add (gadget);
                    }
                  }
                }

                return;
              }
            }
          }
        }
      }
    }
    #endregion

    #region Static
    public static GadgetTest CreateDefault => (new GadgetTest ());
    #endregion

    #region Support
    void CopyFrom (TModelAction modelAction)
    {
      Id = modelAction.ComponentInfoModel.Id;

      Test = modelAction.ExtensionTextModel.Text;
      Description = modelAction.ExtensionTextModel.Description;
      ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
      Enabled = modelAction.ComponentInfoModel.Enabled;
    }

    bool ContainsTarget (GadgetTest gadget)
    {
      foreach (var targetId in Targets) {
        if (targetId.Equals (gadget.Id)) {
          return (true);
        }
      }

      return (false);
    }
    #endregion
  };
  //---------------------------//

}  // namespace