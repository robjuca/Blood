﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using rr.Library.Types;
using rr.Library.Controls;
using rr.Library.Helper;

using Server.Models.Component;
using Server.Models.Action;
//---------------------------//

namespace Shared.Types
{
  public class TPropertyExtensionModel : NotificationObject
  {
    #region Properties
    #region Style
    [Category ("2 - Style")]
    [DisplayName ("Style Horizontal")]
    [Description ("current horizontal style")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorStyle), typeof (PropertyValueEditor))]
    public TStylePropertyInfo StyleHorizontalProperty
    {
      get;
      set;
    }

    [Category ("2 - Style")]
    [DisplayName ("Style Vertical")]
    [Description ("current vertical style")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorStyle), typeof (PropertyValueEditor))]
    public TStylePropertyInfo StyleVerticalProperty
    {
      get;
      set;
    } 
    #endregion

    #region Layout
    [Category ("3 - Layout")]
    [DisplayName ("Image Position")]
    [Description ("current image position")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorImagePosition), typeof (PropertyValueEditor))]
    public TImagePositionInfo ImagePositionProperty
    {
      get;
      set;
    }

    [Category ("3 - Layout")]
    [DisplayName ("Link")]
    [Description ("link for external Url")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorH60), typeof (PropertyValueEditor))]
    public string LinkProperty
    {
      get
      {
        return (""/*m_DocumentModel.ExternalLink*/);
      }

      set
      {
        //m_DocumentModel.ExternalLink = value;
        RaisePropertyChanged ();
      }
    }

    [Category ("3 - Layout")]
    [DisplayName ("Header Visibility")]
    [Description ("show header")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorVisibility), typeof (PropertyValueEditor))]
    public TVisibilityInfo HeaderVisibilityProperty
    {
      get;
      set;
    }

    [Category ("3 - Layout")]
    [DisplayName ("Footer Visibility")]
    [Description ("show footer")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorVisibility), typeof (PropertyValueEditor))]
    public TVisibilityInfo FooterVisibilityProperty
    {
      get;
      set;
    }

    [Category ("4 - Board")]
    [DisplayName ("Columns")]
    [Description ("select columns size")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorInt4), typeof (PropertyValueEditor))]
    public TInt4PropertyInfo ColumnsProperty
    {
      get;
      set;
    }

    [Category ("4 - Board")]
    [DisplayName ("Rows")]
    [Description ("select rows size")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorInt4), typeof (PropertyValueEditor))]
    public TInt4PropertyInfo RowsProperty
    {
      get;
      set;
    }
    #endregion

    #region Text
    [Category ("5 - Text")]
    [DisplayName ("Description")]
    [Description ("description info")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorH60), typeof (PropertyValueEditor))]
    public string DescriptionProperty
    {
      get
      {
        return (m_TextModel.Description);
      }

      set
      {
        m_TextModel.Description = value;

        RaisePropertyChanged (nameof (DescriptionProperty));
      }
    }

    [Category ("5 - Text")]
    [DisplayName ("Reference")]
    [Description ("reference info")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorH60), typeof (PropertyValueEditor))]
    public string ReferenceProperty
    {
      get
      {
        return (m_TextModel.Reference);
      }

      set
      {
        m_TextModel.Reference = value;

        RaisePropertyChanged (nameof (ReferenceProperty));
      }
    }

    [Category ("5 - Text")]
    [DisplayName ("Value")]
    [Description ("value info")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorH60), typeof (PropertyValueEditor))]
    public string ValueProperty
    {
      get
      {
        return (m_TextModel.Value);
      }

      set
      {
        m_TextModel.Value = value;

        RaisePropertyChanged (nameof (ValueProperty));
      }
    }

    [Category ("5 - Text")]
    [DisplayName ("External Link")]
    [Description ("link for external Url")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorH60), typeof (PropertyValueEditor))]
    public string ExternalLinkProperty
    {
      get
      {
        return (m_TextModel.ExternalLink);
      }

      set
      {
        m_TextModel.ExternalLink = value;

        RaisePropertyChanged (nameof (ExternalLinkProperty));
      }
    }

    [Category ("5 - Text")]
    [DisplayName ("Date")]
    [MaxLength (40)]
    [Description ("current date time")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorDate), typeof (PropertyValueEditor))]
    public TDatePropertyInfo DateProperty
    {
      get;
      set;
    }

    [Category ("5 - Text")]
    [DisplayName ("Text")]
    [MaxLength (40)]
    [Description ("max length = 40")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    public string TextProperty
    {
      get
      {
        return (m_TextModel.Text);
      }

      set
      {
        m_TextModel.Text = value;

        RaisePropertyChanged (nameof (TextProperty));
      }
    }
    #endregion

    #region ImageProperty
    [Category ("6 - Image")]
    [DisplayName ("Browse")]
    [Description ("current image")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TPictureEditor), typeof (PropertyValueEditor))]
    public System.Windows.Media.Imaging.BitmapImage ImageProperty
    {
      get
      {
        return (THelper.ByteArrayToBitmapImage (m_ImageModel.Image).Clone ());
      }

      set
      {
        if (m_ImageCleanupCommit) {
          m_ImageCleanupCommit = false;
        }

        else {
          if (value.NotNull ()) {
            m_ImageModel.Width = value.PixelWidth;
            m_ImageModel.Height = value.PixelHeight;

            m_ImageModel.Image = THelper.BitmapImageToByteArray (value);

            if (m_ImageModel.Image.NotNull ()) {
              RaisePropertyChanged (nameof (ImageProperty));
            }
          }
        }
      }
    }
    #endregion

    #region ComboBox
    [Category ("7 - Selection")]
    [DisplayName ("Selection")]
    [Description ("Select an item from list")]
    [RefreshProperties (RefreshProperties.All)]
    [Browsable (true)]
    [Editor (typeof (TTextEditorComboBox), typeof (PropertyValueEditor))]
    public TSelectionPropertyInfo SelectionProperty
    {
      get;
      set;
    } 
    #endregion
    #endregion

    #region Constructor
    TPropertyExtensionModel ()
    {
      m_ImageModel = ExtensionImage.CreateDefault;
      m_LayoutModel = ExtensionLayout.CreateDefault;
      //m_DocumentModel = ExtensionDocument.CreateDefault;
      m_GeometryModel = ExtensionGeometry.CreateDefault;
      m_TextModel = ExtensionText.CreateDefault;

      StyleHorizontalProperty = new TStylePropertyInfo ();
      StyleVerticalProperty = new TStylePropertyInfo ();

      ImagePositionProperty = new TImagePositionInfo ();
      ImagePositionProperty.PropertyChanged += OnPropertyChanged;

      HeaderVisibilityProperty = new TVisibilityInfo ("header");
      HeaderVisibilityProperty.PropertyChanged += OnPropertyChanged;

      FooterVisibilityProperty = new TVisibilityInfo ("footer");
      FooterVisibilityProperty.PropertyChanged += OnPropertyChanged;

      TPictureEditor.Cleanup += OnPictureEditorCleanup;

      ColumnsProperty = TInt4PropertyInfo.Create (4);
      RowsProperty = TInt4PropertyInfo.Create (4);

      SelectionProperty = TSelectionPropertyInfo.CreateDefault;

      DateProperty = TDatePropertyInfo.CreateDefault;
      DateProperty.PropertyChanged += OnPropertyChanged;

      m_Names = new Collection<string> ();
      m_ModelCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.None);
    }
    #endregion

    #region Members
    public void SelectModelCategory (Server.Models.Infrastructure.TCategory modelCategory)
    {
      if (modelCategory.NotNull ()) {
        m_ModelCategory = Server.Models.Infrastructure.TCategoryType.ToValue (modelCategory);

        m_Names.Clear ();

        // remove property not required

        switch (modelCategory) {
          //   Gadget Material
          case Server.Models.Infrastructure.TCategory.Material: {
              m_Names.Add ("LinkProperty");
              m_Names.Add ("ImagePositionProperty");
              m_Names.Add ("HeaderVisibilityProperty");
              m_Names.Add ("FooterVisibilityProperty");
              m_Names.Add ("StyleHorizontalProperty");
              m_Names.Add ("StyleVerticalProperty");
              m_Names.Add ("ColumnsProperty");
              m_Names.Add ("RowsProperty");
              m_Names.Add ("ReferenceProperty");
              m_Names.Add ("SelectionProperty");
              m_Names.Add ("ValueProperty");
              m_Names.Add ("DateProperty");
            }
            break;

          //   Gadget Target
          case Server.Models.Infrastructure.TCategory.Target: {
              m_Names.Add ("LinkProperty");
              m_Names.Add ("ImagePositionProperty");
              m_Names.Add ("HeaderVisibilityProperty");
              m_Names.Add ("FooterVisibilityProperty");
              m_Names.Add ("StyleHorizontalProperty");
              m_Names.Add ("StyleVerticalProperty");
              m_Names.Add ("ColumnsProperty");
              m_Names.Add ("RowsProperty");
              m_Names.Add ("ImageProperty");
              m_Names.Add ("ValueProperty");
              m_Names.Add ("DateProperty");
            }
            break;

          //   Gadget Test
          case Server.Models.Infrastructure.TCategory.Test: {
              m_Names.Add ("LinkProperty");
              m_Names.Add ("ImagePositionProperty");
              m_Names.Add ("HeaderVisibilityProperty");
              m_Names.Add ("FooterVisibilityProperty");
              m_Names.Add ("StyleHorizontalProperty");
              m_Names.Add ("StyleVerticalProperty");
              m_Names.Add ("ColumnsProperty");
              m_Names.Add ("RowsProperty");
              m_Names.Add ("ImageProperty");
              m_Names.Add ("ValueProperty");
              m_Names.Add ("ReferenceProperty");
              m_Names.Add ("SelectionProperty");
              m_Names.Add ("DateProperty");
            }
            break;

          //   Gadget Registration
          case Server.Models.Infrastructure.TCategory.Registration: {
              m_Names.Add ("LinkProperty");
              m_Names.Add ("ExternalLinkProperty");
              m_Names.Add ("ImagePositionProperty");
              m_Names.Add ("HeaderVisibilityProperty");
              m_Names.Add ("FooterVisibilityProperty");
              m_Names.Add ("StyleHorizontalProperty");
              m_Names.Add ("StyleVerticalProperty");
              m_Names.Add ("ColumnsProperty");
              m_Names.Add ("RowsProperty");
              m_Names.Add ("ValueProperty");
              m_Names.Add ("ReferenceProperty");
              m_Names.Add ("SelectionProperty");
            }
            break;

          //   Gadget Result
          case Server.Models.Infrastructure.TCategory.Result: {
              m_Names.Add ("LinkProperty");
              m_Names.Add ("ExternalLinkProperty");
              m_Names.Add ("ImagePositionProperty");
              m_Names.Add ("HeaderVisibilityProperty");
              m_Names.Add ("FooterVisibilityProperty");
              m_Names.Add ("StyleHorizontalProperty");
              m_Names.Add ("StyleVerticalProperty");
              m_Names.Add ("ColumnsProperty");
              m_Names.Add ("RowsProperty");
              m_Names.Add ("ImageProperty");
              m_Names.Add ("ValueProperty");
              m_Names.Add ("ReferenceProperty");
              m_Names.Add ("SelectionProperty");
            }
            break;

          //   Gadget Report
          case Server.Models.Infrastructure.TCategory.Report: {
              m_Names.Add ("LinkProperty");
              m_Names.Add ("ExternalLinkProperty");
              m_Names.Add ("ImagePositionProperty");
              m_Names.Add ("HeaderVisibilityProperty");
              m_Names.Add ("FooterVisibilityProperty");
              m_Names.Add ("StyleHorizontalProperty");
              m_Names.Add ("StyleVerticalProperty");
              m_Names.Add ("ColumnsProperty");
              m_Names.Add ("RowsProperty");
              m_Names.Add ("ImageProperty");
              m_Names.Add ("ValueProperty");
              m_Names.Add ("ReferenceProperty");
              m_Names.Add ("SelectionProperty");
            }
            break;
        }
      }
    }

    public void Initialize ()
    {
      StyleHorizontalProperty.Initialize (TContentStyle.Mode.Horizontal);
      StyleVerticalProperty.Initialize (TContentStyle.Mode.Vertical); 

      ColumnsProperty.Initialize ();
      RowsProperty.Initialize ();

      StyleHorizontalProperty.PropertyChanged += OnPropertyChanged;
      StyleVerticalProperty.PropertyChanged += OnPropertyChanged;

      ColumnsProperty.PropertyChanged += Int4PropertyChanged;
      RowsProperty.PropertyChanged += Int4PropertyChanged;

      ImagePositionProperty.SetupCollection (StyleHorizontalProperty.Current.StyleInfo, StyleVerticalProperty.Current.StyleInfo);

      SelectionProperty.PropertyChanged += OnPropertyChanged;
    }

    public void SelectModel (Server.Models.Infrastructure.TCategory category, TEntityAction action)
    {
      if (action.NotNull ()) {
        switch (category) {
          case Server.Models.Infrastructure.TCategory.Target:
            SelectionProperty.Select (action);
            break;
        }
      }
    }

    public void SelectModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        //HeaderVisibilityProperty.Select (action.ModelAction.ExtensionDocumentModel.HeaderVisibility, action.ModelAction.ExtensionDocumentModel.FooterVisibility);
        //FooterVisibilityProperty.Select (action.ModelAction.ExtensionDocumentModel.HeaderVisibility, action.ModelAction.ExtensionDocumentModel.FooterVisibility);

        ColumnsProperty.Select (action.ModelAction.ExtensionGeometryModel.SizeCols);
        RowsProperty.Select (action.ModelAction.ExtensionGeometryModel.SizeRows);

        if (action.Id.NotEmpty ()) {
          bool locked = (action.ModelAction.ComponentStatusModel.Locked || action.ModelAction.ComponentStatusModel.Busy);

          var styleHorizontalString = action.ModelAction.ExtensionLayoutModel.StyleHorizontal;
          var styleVerticalString = action.ModelAction.ExtensionLayoutModel.StyleVertical;

          var styleInfoHorizontal = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
          styleInfoHorizontal.Select (styleHorizontalString);

          var styleInfoVertical = TStyleInfo.Create (TContentStyle.Mode.Vertical);
          styleInfoVertical.Select (styleVerticalString);

          StyleHorizontalProperty.Select (styleInfoHorizontal, locked);
          StyleVerticalProperty.Select (styleInfoVertical, locked);

          if (locked) {
            ColumnsProperty.Lock ();
            RowsProperty.Lock ();
          }
        }

        // DO NOT CHANGE THIS ORDER
        ImagePositionProperty.Select (action.ModelAction.ExtensionGeometryModel.PositionImage);

        m_LayoutModel.CopyFrom (action.ModelAction.ExtensionLayoutModel);
        m_ImageModel.CopyFrom (action.ModelAction.ExtensionImageModel);
        //m_DocumentModel.CopyFrom (action.ModelAction.ExtensionDocumentModel);
        m_GeometryModel.CopyFrom (action.ModelAction.ExtensionGeometryModel);
        m_TextModel.CopyFrom (action.ModelAction.ExtensionTextModel);

        DateProperty.TheDate = m_TextModel.Date;

        SelectionProperty.SelectModel (action);
      }
    }

    public void Select (bool lockInt4, bool unlockInt4)
    {
      if (lockInt4) {
        LockInt4 ();
      }

      if (unlockInt4) {
        UnlockInt4 ();
      }
    }

    public void SelectionLock (bool lockCurrent)
    {
      SelectionProperty.Lock (lockCurrent);
    }

    public void SelectReport (TReportData reportData)
    {
      if (reportData.NotNull ()) {
        Select (reportData.Locked, reportData.Unlocked);
      }
    }

    public void RequestModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        Request ();

        action.ModelAction.ExtensionLayoutModel.CopyFrom (m_LayoutModel);
        action.ModelAction.ExtensionImageModel.CopyFrom (m_ImageModel);
        action.ModelAction.ExtensionTextModel.CopyFrom (m_TextModel);
        //action.ModelAction.ExtensionDocumentModel.CopyFrom (m_DocumentModel);
        action.ModelAction.ExtensionGeometryModel.CopyFrom (m_GeometryModel);

        SelectionProperty.Request (action);
      }
    }

    public void LockInt4 ()
    {
      ColumnsProperty.Lock ();
      RowsProperty.Lock ();
    }

    public void UnlockInt4 ()
    {
      ColumnsProperty.Unlock ();
      RowsProperty.Unlock ();
    }

    public void Cleanup ()
    {
      m_ImageModel = ExtensionImage.CreateDefault;
      m_LayoutModel = ExtensionLayout.CreateDefault;
      //m_DocumentModel = ExtensionDocument.CreateDefault;
      m_GeometryModel = ExtensionGeometry.CreateDefault;
      m_TextModel = ExtensionText.CreateDefault;

      UnlockInt4 ();
    }

    public void ImageCleanup ()
    {
      if (m_ImageModel.Image.NotNull ()) {
        m_LayoutModel.Width = 0;
        m_LayoutModel.Height = 0;

        m_ImageModel.Width = 0;
        m_ImageModel.Height = 0;
        m_ImageModel.Image = null;
      }
    }

    public void ValidateModel ()
    {
      var properties = TypeDescriptor.GetProperties (this);

      foreach (var name in m_Names) {
        var descriptor = properties [name];
        var attrib = (BrowsableAttribute) descriptor.Attributes [typeof (BrowsableAttribute)];
        ChangeAttribute (attrib);
      }
    }
    #endregion

    #region Event
    void OnPictureEditorCleanup (object sender, EventArgs e)
    {
      if (m_ImageModel.Image.NotNull ()) {
        m_ImageCleanupCommit = true;
        RaisePropertyChanged ("FrameImageCleanup");
      }
    }

    void Int4PropertyChanged (object sender, PropertyChangedEventArgs e)
    {
      m_GeometryModel.SizeCols = ColumnsProperty.Int4.Int4Value;
      m_GeometryModel.SizeRows = RowsProperty.Int4.Int4Value;

      RaisePropertyChanged (e.PropertyName);
    }

    void OnPropertyChanged (object sender, PropertyChangedEventArgs e)
    {
      string propertyName = e.PropertyName;

      if (propertyName.Equals ("StyleHorizontalProperty", StringComparison.InvariantCulture) || propertyName.Equals ("StyleVerticalProperty", StringComparison.InvariantCulture)) {
        ImagePositionProperty.SetupCollection (StyleHorizontalProperty.Current.StyleInfo, StyleVerticalProperty.Current.StyleInfo);
      }

      RaisePropertyChanged (propertyName);
    }
    #endregion

    #region Fields
    ExtensionLayout                                   m_LayoutModel;
    ExtensionImage                                    m_ImageModel;
    ExtensionText                                     m_TextModel;
    //ExtensionDocument                                 m_DocumentModel;
    ExtensionGeometry                                 m_GeometryModel;
    readonly Collection<string>                       m_Names;
    bool                                              m_ImageCleanupCommit;
    int                                               m_ModelCategory;
    #endregion

    #region Support
    static void ChangeAttribute (Attribute attribute)
    {
      System.Reflection.FieldInfo browsable = attribute.GetType ().GetField ("browsable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
      browsable.SetValue (attribute, false);
    }

    void Request ()
    {
      m_GeometryModel.PositionImage = ImagePositionProperty.Current.PositionString;

      m_TextModel.Date = DateProperty.TheDate;

      //var category = Server.Models.Infrastructure.TCategoryType.FromValue (m_ModelCategory);

      //if (category.Equals (Server.Models.Infrastructure.TCategory.Document)) {
      //  m_ImageModel.Width = ImagePositionProperty.Current.Size.Width;
      //  m_ImageModel.Height = ImagePositionProperty.Current.Size.Height;
      //}

      //m_DocumentModel.HeaderVisibility = HeaderVisibilityProperty.ToString ();
      //m_DocumentModel.FooterVisibility = FooterVisibilityProperty.ToString ();

      m_LayoutModel.StyleHorizontal = StyleHorizontalProperty.Current.StyleInfo.StyleString;
      m_LayoutModel.StyleVertical = StyleVerticalProperty.Current.StyleInfo.StyleString;
      m_LayoutModel.Width = StyleHorizontalProperty.Current.Size.Width;
      m_LayoutModel.Height = StyleVerticalProperty.Current.Size.Height;
    }
    #endregion

    #region Static
    public static TPropertyExtensionModel CreateDefault => new TPropertyExtensionModel ();
    #endregion
  };
  //---------------------------//

}  // namespace