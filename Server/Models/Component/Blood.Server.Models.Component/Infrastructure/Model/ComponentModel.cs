/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//---------------------------//

namespace Server.Models.Component
{
  public class TComponentModel
  {
    #region Property
    #region Settings
    public Settings SettingsModel
    {
      get;
      private set;
    }
    #endregion

    #region Component
    public ComponentInfo InfoModel
    {
      get;
      private set;
    }

    public ComponentStatus StatusModel
    {
      get;
      private set;
    }
    #endregion

    #region Extension
    public ExtensionGeometry GeometryModel
    {
      get;
      private set;
    }

    public ExtensionImage ImageModel
    {
      get;
      private set;
    }

    public ExtensionLayout LayoutModel
    {
      get;
      private set;
    }

    public ExtensionNode NodeModel
    {
      get;
      private set;
    }

    public Collection<ExtensionNode> NodeModelCollection
    {
      get;
      private set;
    }

    public ExtensionText TextModel
    {
      get;
      private set;
    }
    #endregion

    #region Gadget Medical Test
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

    #region Gadget Medical Care
    public GadgetRegistration GadgetRegistrationModel
    {
      get;
      private set;
    }

    public GadgetTests GadgetTestsModel
    {
      get;
      private set;
    }

    public GadgetReport GadgetReportModel
    {
      get;
      private set;
    }
    #endregion

    public Guid Id
    {
      get
      {
        return (InfoModel.Id);
      }
    }

    public string Name
    {
      get
      {
        return (InfoModel.Name);
      }
    }

    public string StyleHorizontal
    {
      get
      {
        return (LayoutModel.StyleHorizontal);
      }
    }

    public string StyleVertical
    {
      get
      {
        return (LayoutModel.StyleVertical);
      }
    }
    #endregion

    #region Constructor
    protected TComponentModel ()
    {
      SettingsModel = Settings.CreateDefault;

      InfoModel = ComponentInfo.CreateDefault;
      StatusModel = ComponentStatus.CreateDefault;
      
      GeometryModel = ExtensionGeometry.CreateDefault;
      ImageModel = ExtensionImage.CreateDefault;
      LayoutModel = ExtensionLayout.CreateDefault;
      NodeModel = ExtensionNode.CreateDefault;
      TextModel = ExtensionText.CreateDefault;

      NodeModelCollection = new Collection<ExtensionNode> ();

      GadgetMaterialModel = GadgetMaterial.CreateDefault;
      GadgetTargetModel = GadgetTarget.CreateDefault;
      GadgetTestModel = GadgetTest.CreateDefault;

      GadgetRegistrationModel = GadgetRegistration.CreateDefault;
      GadgetTestsModel = GadgetTests.CreateDefault;
      GadgetReportModel = GadgetReport.CreateDefault;
    }
    #endregion

    #region Members
    public void Select (ComponentInfo info)
    {
      InfoModel.CopyFrom (info);
    }

    public void Select (Settings settings)
    {
      SettingsModel.CopyFrom (settings);
    }

    public void Select (ComponentStatus status)
    {
      StatusModel.CopyFrom (status);
    }

    public void Select (ExtensionGeometry geometry)
    {
      GeometryModel.CopyFrom (geometry);
    }

    public void Select (ExtensionImage image)
    {
      ImageModel.CopyFrom (image);
    }

    public void Select (ExtensionLayout layout)
    {
      LayoutModel.CopyFrom (layout);
    }

    public void Select (ExtensionNode node)
    {
      NodeModel.CopyFrom (node);
    }

    public void Select (IList<ExtensionNode> list)
    {
      NodeModelCollection = new Collection<ExtensionNode> (list);
    }

    public void Select (ExtensionText text)
    {
      TextModel.CopyFrom (text);
    }

    public void Select (GadgetMaterial gadget)
    {
      GadgetMaterialModel.CopyFrom (gadget);
    }

    public void Select (GadgetTarget gadget)
    {
      GadgetTargetModel.CopyFrom (gadget);
    }

    public void Select (GadgetTest gadget)
    {
      GadgetTestModel.CopyFrom (gadget);
    }

    public void Select (GadgetRegistration gadget)
    {
      GadgetRegistrationModel.CopyFrom (gadget);
    }

    public void Select (GadgetTests gadget)
    {
      GadgetTestsModel.CopyFrom (gadget);
    }

    public void Select (GadgetReport gadget)
    {
      GadgetReportModel.CopyFrom (gadget);
    }

    public void Select (TEntityAction action)
    {
      if (action.NotNull ()) {
        var model = Create (action.ModelAction);
        CopyFrom (model);
      }
    }

    public TModelAction RequestModel ()
    {
      var modelAction = TModelAction.CreateDefault;
      modelAction.SettingsModel.CopyFrom (SettingsModel);

      modelAction.ComponentInfoModel.CopyFrom (InfoModel);
      modelAction.ComponentStatusModel.CopyFrom (StatusModel);
      
      modelAction.ExtensionImageModel.CopyFrom (ImageModel);
      modelAction.ExtensionGeometryModel.CopyFrom (GeometryModel);
      modelAction.ExtensionLayoutModel.CopyFrom (LayoutModel);
      modelAction.ExtensionTextModel.CopyFrom (TextModel);
      modelAction.ExtensionNodeModel.CopyFrom (NodeModel);

      modelAction.GadgetMaterialModel.CopyFrom (GadgetMaterialModel);
      modelAction.GadgetTargetModel.CopyFrom (GadgetTargetModel);
      modelAction.GadgetTestModel.CopyFrom (GadgetTestModel);

      modelAction.GadgetRegistrationModel.CopyFrom (GadgetRegistrationModel);
      modelAction.GadgetTestsModel.CopyFrom (GadgetTestsModel);
      modelAction.GadgetReportModel.CopyFrom (GadgetReportModel);

      return (modelAction);
    }

    public void RefreshModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        GadgetMaterialModel.RefreshModel (action);
        GadgetTargetModel.RefreshModel (action);
        GadgetTestModel.RefreshModel (action);

        GadgetRegistrationModel.RefreshModel (action);
        GadgetTestsModel.RefreshModel (action);
        GadgetReportModel.RefreshModel (action);
      }
    }
    #endregion

    #region Protected Members
    protected void CopyFrom (TComponentModel alias)
    {
      if (alias.NotNull ()) {
        SettingsModel.CopyFrom (alias.SettingsModel);

        InfoModel.CopyFrom (alias.InfoModel);
        StatusModel.CopyFrom (alias.StatusModel);
                
        ImageModel.CopyFrom (alias.ImageModel);
        GeometryModel.CopyFrom (alias.GeometryModel);
        LayoutModel.CopyFrom (alias.LayoutModel);
        TextModel.CopyFrom (alias.TextModel);
        NodeModel.CopyFrom (alias.NodeModel);

        NodeModelCollection = new Collection<ExtensionNode> (alias.NodeModelCollection);

        GadgetMaterialModel.CopyFrom (alias.GadgetMaterialModel);
        GadgetTargetModel.CopyFrom (alias.GadgetTargetModel);
        GadgetTestModel.CopyFrom (alias.GadgetTestModel);

        GadgetRegistrationModel.CopyFrom (alias.GadgetRegistrationModel);
        GadgetTestsModel.CopyFrom (alias.GadgetTestsModel);
        GadgetReportModel.CopyFrom (alias.GadgetReportModel);
      }
    }
    #endregion

    #region Static
    public static TComponentModel Create (TModelAction modelAction)
    {
      var model = new TComponentModel ();

      if (modelAction.NotNull ()) {
        model.SettingsModel.CopyFrom (modelAction.SettingsModel);

        model.InfoModel.CopyFrom (modelAction.ComponentInfoModel);
        model.StatusModel.CopyFrom (modelAction.ComponentStatusModel);

        model.ImageModel.CopyFrom (modelAction.ExtensionImageModel);
        model.GeometryModel.CopyFrom (modelAction.ExtensionGeometryModel);
        model.LayoutModel.CopyFrom (modelAction.ExtensionLayoutModel);
        model.TextModel.CopyFrom (modelAction.ExtensionTextModel);
        model.NodeModel.CopyFrom (modelAction.ExtensionNodeModel);

        model.GadgetMaterialModel.CopyFrom (modelAction.GadgetMaterialModel);
        model.GadgetTargetModel.CopyFrom (modelAction.GadgetTargetModel);
        model.GadgetTestModel.CopyFrom (modelAction.GadgetTestModel);

        model.GadgetRegistrationModel.CopyFrom (modelAction.GadgetRegistrationModel);
        model.GadgetTestsModel.CopyFrom (modelAction.GadgetTestsModel);
        model.GadgetReportModel.CopyFrom (modelAction.GadgetReportModel);
      }

      return (model);
    }

    public static TComponentModel CreateModel => new TComponentModel ();
    #endregion
  };
  //---------------------------//

}  // namespace