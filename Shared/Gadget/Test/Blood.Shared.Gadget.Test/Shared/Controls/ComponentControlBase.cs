/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

using Server.Models.Infrastructure;

using Shared.Types;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Test
{
  public abstract class TComponentControlBase : ContentControl
  {
    #region Dependency Property
    public static readonly DependencyProperty ComponentControlModelProperty =
      DependencyProperty.Register ("Model", typeof (TComponentControlModel), typeof (TComponentControlBase),
      new FrameworkPropertyMetadata (TComponentControlModel.CreateDefault, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ModelPropertyChanged));
    #endregion

    #region Property
    public TComponentControlModel Model
    {
      get
      {
        return (TComponentControlModel) GetValue (ComponentControlModelProperty);
      }
      set
      {
        SetValue (ComponentControlModelProperty, value);
      }
    }
    #endregion

    #region Constructor
    protected TComponentControlBase ()
    {
      Background = Brushes.White;

      m_Grid = new Grid ()
      {
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top,
      };

      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 0 material image, test name
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 1 description
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 2 external link
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Star) }); // row 3 targets

      AddChild (m_Grid);

      ControlMode = TControlMode.None;
    }

    protected TComponentControlBase (TControlMode mode)
      : this ()
    {
      ControlMode = mode;
    }
    #endregion

    #region Members
    public void RefreshDesign ()
    {
      m_Grid.Children.Clear ();

      // material image, test name (row 0)
      var stack = new StackPanel ()
      {
        Orientation = Orientation.Horizontal
      };

      if (Model.HasImage) {
        // image
        var imageSource = rr.Library.Helper.THelper.ByteArrayToBitmapImage (Model.MaterialImage);

        if (imageSource.NotNull ()) {
          var image = new Image ()
          {
            Stretch = Stretch.Fill,
            Width = 18,
            Height = 18,
            Source = imageSource.Clone (),
            Margin = new Thickness (0, 0, 10, 0),
          };

          stack.Children.Add (image);
        }
      }
      
      // name
      var textBlock = new TextBlock ()
      {
        Margin = new Thickness (10, 0, 0, 0),
        VerticalAlignment = VerticalAlignment.Center,
        Text = Model.ControlModel.GadgetName,
        FontWeight = FontWeights.UltraBold,
        FontSize = 14,
      };
      
      stack.Children.Add (textBlock);

      var cz = new ColorZone ()
      {
        Padding = new Thickness (3),
        Mode = Model.HasComponentControlModels ? ColorZoneMode.Accent : ColorZoneMode.PrimaryLight,
        Content = stack,
      };

      cz.SetValue (Grid.RowProperty, 0);  // row  0
      m_Grid.Children.Add (cz);

      // test description (row 1)
      var textBox = new TextBox ()
      {
        Margin = new Thickness (3),
        Padding = new Thickness (3),
        FontSize = 11,
        TextWrapping = TextWrapping.Wrap,
        TextAlignment = TextAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top,
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        Text = Model.ControlModel.Description,
      };
      

      textBox.SetValue (Grid.RowProperty, 1);  // row  1
      m_Grid.Children.Add (textBox);

      // external link (row 2)
      if (string.IsNullOrEmpty (Model.ControlModel.ExternalLink).IsFalse ()) {
        try {
          var externalLink = new System.Windows.Documents.Hyperlink (new System.Windows.Documents.Run ("more info"))
          {
            NavigateUri = new Uri (Model.ControlModel.ExternalLink),
            TargetName = "_blanc",
          };

          var textBoxLink = new TextBlock (externalLink)
          {
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness (3),
          };

          textBoxLink.SetValue (Grid.RowProperty, 2);
          m_Grid.Children.Add (textBoxLink); // row 2
        }

        catch (Exception) {
          // do nothing
        }
      }

      // targets (row 3)
      InsertTargetRelation ();
      InsertTestRelation ();
    }
    #endregion

    #region Callback
    static void ModelPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TComponentControlBase control) {
        if (e.NewValue is TComponentControlModel) {
          control.ModelValidated = true; // first time
          control.RefreshDesign ();
        }
      }
    }
    #endregion

    #region Property
    internal bool ModelValidated
    {
      get;
      set;
    }

    TControlMode ControlMode
    {
      get;
      set;
    }
    #endregion

    #region Fields
    readonly Grid                                     m_Grid;
    #endregion

    #region Support
    void InsertTargetRelation ()
    {
      if (Model.RequestCategory ().Equals (TCategory.Target)) {
        var targetsItemSource = new Collection<GadgetTarget> ();
        Model.ControlModel.RequestContent (targetsItemSource);

        string itemTemplate = @"
          <DataTemplate
              xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                <StackPanel>
                  <TextBlock FontWeight='Bold' Foreground='DarkBlue' Text='{Binding GadgetName}' />
                  <TextBox FontSize='10px' IsReadOnly='true' MaxWidth='680' TextWrapping='Wrap' Text='{Binding Description}' />
                  <TextBlock Foreground='DarkGreen' Text='{Binding Reference}' />
                </StackPanel>
          </DataTemplate>"
        ;

        using (var sr = new System.IO.MemoryStream (System.Text.Encoding.UTF8.GetBytes (itemTemplate))) {
          var listboxTargets = new ListBox ()
          {
            ItemsSource = targetsItemSource,
            ItemTemplate = System.Windows.Markup.XamlReader.Load (sr) as DataTemplate,
          };

          listboxTargets.SetValue (Grid.RowProperty, 3);  // row  3
          m_Grid.Children.Add (listboxTargets);
        }
      }
    }

    void InsertTestRelation ()
    {
      if (Model.RequestCategory ().Equals (TCategory.Test) ) {
        var stack = new StackPanel ();

        var scrow = new ScrollViewer
        {
          VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
          CanContentScroll= false,
        };

        scrow.SetValue (Grid.RowProperty, 3); // row 3
        scrow.Content = stack;

        m_Grid.Children.Add (scrow);

        if (Model.HasComponentControlModels) {
          foreach (var controlModel in Model.ComponentControlModels) {
            var cc = new TComponentDisplayControl
            {
              Model = controlModel
            };

            stack.Children.Add (cc);
          }
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace