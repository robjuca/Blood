/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

using Shared.Types;
//---------------------------//

namespace Shared.Gadget.Registration
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
        HorizontalAlignment= HorizontalAlignment.Center,
        VerticalAlignment=VerticalAlignment.Top,
      };

      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 0 image 
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 1 registration name
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 2 description

      m_Card = new Card
      {
        Margin = new Thickness (5),
        Padding = new Thickness (2),
        Width = 200,
        Content = m_Grid,
      };

      AddChild (m_Card);

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

      // image (row 0)
      if (Model.ControlModel.HasImage) {
        var imageSource = rr.Library.Helper.THelper.ByteArrayToBitmapImage (Model.ControlModel.GetImage ());

        if (imageSource.NotNull ()) {
          var image = new Image ()
          {
            Stretch = Stretch.UniformToFill,
            Height = 140,
            VerticalAlignment = VerticalAlignment.Top,
            Source = imageSource,
          };

          image.SetValue (Grid.RowProperty, 0);  // row 0
          m_Grid.Children.Add (image);
        }
      }

      // Registration (row 1)
      if (string.IsNullOrEmpty (Model.ControlModel.GadgetName).IsFalse ()) {
        var textBlock = new TextBlock ()
        {
          Padding = new Thickness (2),
          FontWeight = FontWeights.SemiBold,
          Text = Model.ControlModel.GadgetName,
        };

        textBlock.SetValue (Grid.RowProperty, 1);  // row 1

        m_Grid.Children.Add (textBlock);
      }

      // description (row 2)
      if (string.IsNullOrEmpty (Model.ControlModel.Description).IsFalse ()) {
        var textBox = new TextBox ()
        {
          Margin = new Thickness (3),
          Padding = new Thickness (3),
          MaxWidth = 400,
          MaxHeight = 100,
          TextWrapping = TextWrapping.Wrap,
          TextAlignment = TextAlignment.Center,
          VerticalAlignment = VerticalAlignment.Top,
          VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
          Text = Model.ControlModel.Description,
        };

        textBox.SetValue (Grid.RowProperty, 2);

        m_Grid.Children.Add (textBox); // row 2
      }
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
    readonly Card                                     m_Card;
    #endregion
  };
  //---------------------------//

}  // namespace