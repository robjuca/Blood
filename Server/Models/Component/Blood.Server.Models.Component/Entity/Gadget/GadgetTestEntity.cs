/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetTest
  {
    #region Constructor
    public GadgetTest ()
    {
      Id = Guid.Empty;
      Enabled = false;
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
      }
    }

    public void CopyFrom (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Enabled = alias.Enabled;
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
      Enabled = modelAction.ComponentInfoModel.Enabled;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace