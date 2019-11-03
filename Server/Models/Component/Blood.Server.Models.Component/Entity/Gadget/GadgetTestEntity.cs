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
    #endregion

    #region Constructor
    public GadgetTest ()
    {
      Id = Guid.Empty;

      Test = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      Enabled = false;
      Targets = new Collection<Guid> ();
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
        Targets = new Collection<Guid> (alias.Targets);
      }
    }

    public void Change (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Test = alias.Test;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;
        Targets = new Collection<Guid> (alias.Targets);
      }
    }

    public void RefreshModel (TEntityAction action)
    {
      // TODO: review
      if (action.NotNull ()) {
        if (action.CategoryType.IsCategory (Infrastructure.TCategory.Test)) {
          // update model action
          CopyFrom (action.ModelAction); // my self
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
    #endregion
  };
  //---------------------------//

}  // namespace