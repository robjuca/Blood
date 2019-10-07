/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

      m_Grid = new Grid () 
      {
        HorizontalAlignment= HorizontalAlignment.Center,
        VerticalAlignment=VerticalAlignment.Center,
      };

      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 0 target
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 1 description
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 2 external link

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

      // target (row 0)

      var grid = new Grid () 
      { 
        HorizontalAlignment= HorizontalAlignment.Center
      };

      grid.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Auto) }); // col 0 material
      grid.SetValue (Grid.RowProperty, 0);

      m_Grid.Children.Add (grid); // row 0

      // target
      if (string.IsNullOrEmpty (Model.ControlModel.Target).IsFalse ()) {
        var textBlock = new TextBlock ()
        {
          VerticalAlignment = VerticalAlignment.Center,
          Text = Model.ControlModel.Target,
        };

        textBlock.SetValue (Grid.ColumnProperty, 0);

        grid.Children.Add (textBlock);
      }

      // description (row 1)

      if (string.IsNullOrEmpty (Model.ControlModel.Description).IsFalse ()) {
        var textBox = new TextBox ()
        {
          Margin = new Thickness (3),
          Padding=new Thickness (3),
          MaxWidth = 400,
          MaxHeight = 100,
          TextWrapping = TextWrapping.Wrap,
          TextAlignment = TextAlignment.Center,
          VerticalAlignment= VerticalAlignment.Top,
          VerticalScrollBarVisibility= ScrollBarVisibility.Auto,
          Text = Model.ControlModel.Description,
        };

        textBox.SetValue (Grid.RowProperty, 1);

        m_Grid.Children.Add (textBox); // row 1
      }

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
    readonly Grid                                     m_Grid;
    #endregion
  };
  //---------------------------//

}  // namespace