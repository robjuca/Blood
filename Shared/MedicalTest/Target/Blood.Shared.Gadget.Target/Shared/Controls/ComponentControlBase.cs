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

namespace Shared.Gadget.Target
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

      m_StackPanel = new StackPanel ();

      m_Card = new Card ()
      {
        Padding = new Thickness (6),
        Content = m_StackPanel,
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
      m_StackPanel.Children.Clear ();

       // target 
       var stack = new StackPanel() 
      { 
        Orientation = Orientation.Horizontal
      };

      // material image
      var materialImage = new Image
      {
        Source = rr.Library.Helper.THelper.ByteArrayToBitmapImage (Model.ChildControlModel.Image)
      };

      stack.Children.Add (materialImage);

      // target
      if (string.IsNullOrEmpty (Model.ControlModel.Target).IsFalse ()) {
        var textBlock = new TextBlock ()
        {
          Margin = new Thickness (10, 0, 0, 0),
          VerticalAlignment = VerticalAlignment.Center,
          FontWeight = FontWeights.Bold,
          Text = Model.ControlModel.Target,
        };

        stack.Children.Add (textBlock);
      }

      var cz = new ColorZone ()
      {
        Mode = ColorZoneMode.PrimaryLight,
        Content = stack,
      };

      m_StackPanel.Children.Add (cz);
      m_StackPanel.Children.Add (new Separator ());

      // description 
      if (string.IsNullOrEmpty (Model.ControlModel.Description).IsFalse ()) {
        var textBox = new TextBox ()
        {
          Margin = new Thickness (3),
          Padding = new Thickness (3),
          MaxWidth = 420,
          MaxHeight = 500,
          TextWrapping = TextWrapping.Wrap,
          TextAlignment = TextAlignment.Justify,
          HorizontalAlignment = HorizontalAlignment.Left,
          VerticalAlignment = VerticalAlignment.Top,
          VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
          Text = Model.ControlModel.Description,
        };

        m_StackPanel.Children.Add (textBox); 
      }

      // reference 
      if (string.IsNullOrEmpty (Model.ControlModel.Reference).IsFalse ()) {
        var textBox = new TextBox ()
        {
          Margin = new Thickness (3),
          Padding = new Thickness (3),
          MaxWidth = 420,
          FontWeight = FontWeights.SemiBold,
          TextWrapping = TextWrapping.Wrap,
          TextAlignment = TextAlignment.Left,
          VerticalAlignment = VerticalAlignment.Top,
          VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
          Text = Model.ControlModel.Reference,
        };

        m_StackPanel.Children.Add (textBox); 
      }

      // external link 
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

          textBoxLink.SetValue (Grid.RowProperty, 3);

          m_StackPanel.Children.Add (textBoxLink); // row 3
        }

        //TODO: what for??
        catch (Exception) {
          // do nothing
        }
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
    readonly StackPanel                               m_StackPanel;
    readonly Card                                     m_Card;
    #endregion
  };
  //---------------------------//

}  // namespace