/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class GadgetTests
  {
    #region Property
     
    #endregion

    #region Constructor
    public GadgetTests ()
    {
      Id = Guid.Empty;

      Name = string.Empty;
      Description = string.Empty;
      Enabled = false;
    }

    public GadgetTests (GadgetTests alias)
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

    public void CopyFrom (GadgetTests alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Name = alias.Name;
        Description = alias.Description;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetTests alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        Description = alias.Description;
        Enabled = alias.Enabled;
      }
    }

    public GadgetTests Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }

    public void RefreshModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        if (action.CategoryType.IsCategory (Infrastructure.TCategory.Tests)) {
          // update model action
          CopyFrom (action.ModelAction); // my self
          action.ModelAction.GadgetTestsModel.CopyFrom (this); 

          // update model collection
          action.CollectionAction.GadgetMaterialCollection.Clear ();

          foreach (var modelAction in action.CollectionAction.ModelCollection) {
            var gadget = GadgetTests.CreateDefault;
            gadget.CopyFrom (modelAction.Value);

            action.CollectionAction.GadgetTestsCollection.Add (gadget);
          }
        }
      }
    }

    public void Refresh (TEntityAction action)
    {
      if (action.NotNull ()) {
        if (action.CategoryType.IsCategory (Infrastructure.TCategory.Tests)) {
          // update model action
          CopyFrom (action.ModelAction); // my self
          action.ModelAction.GadgetTestsModel.CopyFrom (this);
        }
      }
    }
    #endregion

    #region Static
    public static GadgetTests CreateDefault => (new GadgetTests ());
    #endregion

    #region Support
    void CopyFrom (TModelAction modelAction)
    {
      Id = modelAction.ComponentInfoModel.Id;
      Name = modelAction.ExtensionTextModel.Text;
      Description = modelAction.ExtensionTextModel.Description;
      Enabled = modelAction.ComponentInfoModel.Enabled;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace