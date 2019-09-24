/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using rr.Library.Types;

using Shared.ViewModel;
//---------------------------//

namespace Shared.Gadget.Material
{
  public class TComponentControlModel
  {
    #region Property
    public string PropertyName
    {
      get;
      set;
    }

    public string Material
    {
      get;
      set;
    }

    public string Description
    {
      get;
      set;
    }

    public string ExternalLink
    {
      get;
      set;
    }

    public byte [] Image
    {
      get;
      set;
    }

    public Guid Id
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      Cleanup ();
    }
    #endregion

    #region Members
    public void SelectModel (string propertyName, Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        PropertyName = propertyName;

        Id = action.ModelAction.ComponentInfoModel.Id;

        //HorizontalStyleString = action.ModelAction.ExtensionLayoutModel.StyleHorizontal;
        //VerticalStyleString = action.ModelAction.ExtensionLayoutModel.StyleVertical;
        //Width = action.ModelAction.ExtensionLayoutModel.Width;
        //Height = action.ModelAction.ExtensionLayoutModel.Height;

        //ImageGeometry.Position.Position = action.ModelAction.ExtensionGeometryModel.PositionImage;
        //ImageGeometry.Size.Width = action.ModelAction.ExtensionImageModel.Width;
        //ImageGeometry.Size.Height = action.ModelAction.ExtensionImageModel.Height;
        Image = action.ModelAction.ExtensionImageModel.Image;
        
        //HeaderVisibility = action.ModelAction.ExtensionMaterialModel.HeaderVisibility;
        //FooterVisibility = action.ModelAction.ExtensionMaterialModel.FooterVisibility;

        //ExternalLink = action.ModelAction.ExtensionMaterialModel.ExternalLink;

        //if (propertyName.Equals ("all")) {
        //  RtfHeader = action.ModelAction.ExtensionMaterialModel.Header;
        //  RtfFooter = action.ModelAction.ExtensionMaterialModel.Footer;
        //  RtfParagraph = action.ModelAction.ExtensionMaterialModel.Paragraph;
        //}
      }
    }

    public void SelectModel (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        ComponentModelItem.CopyFrom (item);

        Id = item.Id;

        //HorizontalStyleString = item.LayoutModel.StyleHorizontal;
        //VerticalStyleString = item.LayoutModel.StyleVertical;

        //Width = item.LayoutModel.Width;
        //Height = item.LayoutModel.Height;

        //ImageGeometry.Position.Position = item.GeometryModel.PositionImage;
        //ImageGeometry.Size.Width = item.ImageModel.Width;
        //ImageGeometry.Size.Height = item.ImageModel.Height;
        //ImageDistorted = item.ImageModel.Distorted;
        Image = item.ImageModel.Image;

        //HeaderVisibility = item.MaterialModel.HeaderVisibility;
        //FooterVisibility = item.MaterialModel.FooterVisibility;

        //RtfHeader = item.MaterialModel.Header;
        //RtfFooter = item.MaterialModel.Footer;
        //RtfParagraph = item.MaterialModel.Paragraph;

        ExternalLink = item.TextModel.ExternalLink;
      }
    }

    public void RequestComponentModel (List<TComponentModelItem> models)
    {
      if (models.NotNull ()) {
        models.Clear ();
        models.Add (ComponentModelItem);
      }
    }

    public void RequestNodeModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        // node
        action.CollectionAction.ExtensionNodeCollection.Clear ();

        var nodeModel = Server.Models.Component.ExtensionNode.CreateDefault;
        nodeModel.ChildId = Id;
        //nodeModel.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Material);
        nodeModel.Position = 0.ToString ();

        action.CollectionAction.ExtensionNodeCollection.Add (nodeModel);
      }
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        PropertyName = alias.PropertyName;

        //HorizontalStyleString = alias.HorizontalStyleString;
        //VerticalStyleString = alias.VerticalStyleString;
        //Width = alias.Width;
        //Height = alias.Height;

        //ImageGeometry.CopyFrom (alias.ImageGeometry);
        
        //Image = alias.Image;

        //HeaderVisibility = alias.HeaderVisibility;
        //FooterVisibility = alias.FooterVisibility;

        ExternalLink = alias.ExternalLink;

        Material = alias.Material;
        Description = alias.Description;
        //RtfParagraph = alias.RtfParagraph;
      }
    }
    public void Cleanup ()
    {
      PropertyName = string.Empty;
      ExternalLink = string.Empty;
      //HorizontalStyleString = string.Empty;
      //VerticalStyleString = string.Empty;
      //Width = 0;
      //Height = 0;
      //ImageGeometry = TGeometry.CreateDefault;
      Image = null;
      //HeaderVisibility = string.Empty;
      //FooterVisibility = string.Empty;
      Material = string.Empty;
      Description = string.Empty;
      //RtfParagraph = string.Empty;

      //ImageDistorted = false;
      //ImageInfoReport = string.Empty;
      //InfoReport = string.Empty;

      Id = Guid.Empty;

      ComponentModelItem = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Property
    TComponentModelItem ComponentModelItem
    {
      get;
      set;
    }
    #endregion

    #region Static
    public static TComponentControlModel Create (TComponentModelItem item)
    {
      var model = CreateDefault;
      model.SelectModel (item);

      return (model);
    }
    
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace