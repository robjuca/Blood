/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public sealed class TModelAction
  {
    #region Property
    #region Settings
    public Settings SettingsModel
    {
      get;
      private set;
    }
    #endregion

    #region Category
    public CategoryRelation CategoryRelationModel
    {
      get;
      private set;
    } 
    #endregion

    #region Component
    public ComponentDescriptor ComponentDescriptorModel
    {
      get;
      private set;
    }

    public ComponentInfo ComponentInfoModel
    {
      get;
      private set;
    }

    public ComponentStatus ComponentStatusModel
    {
      get;
      private set;
    }

    public ComponentRelation ComponentRelationModel
    {
      get;
      private set;
    }
    #endregion

    #region Extension
    public ExtensionGeometry ExtensionGeometryModel
    {
      get;
      private set;
    }

    public ExtensionImage ExtensionImageModel
    {
      get;
      private set;
    }

    public ExtensionLayout ExtensionLayoutModel
    {
      get;
      private set;
    }

    public ExtensionNode ExtensionNodeModel
    {
      get;
      private set;
    }

    public ExtensionText ExtensionTextModel
    {
      get;
      private set;
    }
    #endregion

    #region Gadget
    public GadgetMaterial GadgetMaterialModel
    {
      get;
      private set;
    }

    public GadgetTarget GadgetTargetModel
    {
      get;
      private set;
    }

    public GadgetTest GadgetTestModel
    {
      get;
      private set;
    }
    #endregion
    #endregion

    #region Constructor
    TModelAction ()
    {
      // Settings
      SettingsModel = Settings.CreateDefault;

      // Category
      CategoryRelationModel = CategoryRelation.CreateDefault;

      // Component
      ComponentDescriptorModel = ComponentDescriptor.CreateDefault;
      ComponentInfoModel = ComponentInfo.CreateDefault;
      ComponentStatusModel = ComponentStatus.CreateDefault;
      ComponentRelationModel = ComponentRelation.CreateDefault;

      // Extension
      ExtensionGeometryModel = ExtensionGeometry.CreateDefault;
      ExtensionImageModel = ExtensionImage.CreateDefault;
      ExtensionLayoutModel = ExtensionLayout.CreateDefault;
      ExtensionNodeModel = ExtensionNode.CreateDefault;
      ExtensionTextModel = ExtensionText.CreateDefault;

      // Gadget
      GadgetMaterialModel = GadgetMaterial.CreateDefault;
      GadgetTargetModel = GadgetTarget.CreateDefault;
      GadgetTestModel = GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    public void CopyFrom (TModelAction alias)
    {
      if (alias.NotNull ()) {
        SettingsModel.CopyFrom (alias.SettingsModel);

        CategoryRelationModel.CopyFrom (alias.CategoryRelationModel);

        ComponentDescriptorModel.CopyFrom (alias.ComponentDescriptorModel);
        ComponentInfoModel.CopyFrom (alias.ComponentInfoModel);
        ComponentStatusModel.CopyFrom (alias.ComponentStatusModel);
        ComponentRelationModel.CopyFrom (alias.ComponentRelationModel);
        
        ExtensionGeometryModel.CopyFrom (alias.ExtensionGeometryModel);
        ExtensionImageModel.CopyFrom (alias.ExtensionImageModel);
        ExtensionLayoutModel.CopyFrom (alias.ExtensionLayoutModel);
        ExtensionNodeModel.CopyFrom (alias.ExtensionNodeModel);
        ExtensionTextModel.CopyFrom (alias.ExtensionTextModel);

        GadgetMaterialModel.CopyFrom (alias.GadgetMaterialModel);
        GadgetTargetModel.CopyFrom (alias.GadgetTargetModel);
        GadgetTestModel.CopyFrom (alias.GadgetTestModel);
      }
    }

    public static void SelectModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        if (action.ModelAction.ComponentInfoModel.Id.NotEmpty ()) {
          // update model action

          switch (action.CategoryType.Category) {
            case Infrastructure.TCategory.Material: {
                var entityAction = TEntityAction.CreateDefault;
                entityAction.ModelAction.CopyFrom (action.ModelAction);

                action.ModelAction.GadgetMaterialModel.CopyFrom (entityAction);
              }
              break;

            case Infrastructure.TCategory.Target: {
                var entityAction = TEntityAction.CreateDefault;
                entityAction.ModelAction.CopyFrom (action.ModelAction);

                action.ModelAction.GadgetTargetModel.CopyFrom (entityAction);
              }
              break;

            case Infrastructure.TCategory.Test: {
                var entityAction = TEntityAction.CreateDefault;
                entityAction.ModelAction.CopyFrom (action.ModelAction);

                action.ModelAction.GadgetTestModel.CopyFrom (entityAction);
              }
              break;
          }
        }
      }
    }
    #endregion

    #region Static
    public static TModelAction CreateDefault => (new TModelAction ()); 
    #endregion
  };
  //---------------------------//

}  // namespace