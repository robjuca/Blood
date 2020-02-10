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

using Shared.Types;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Result
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
        HorizontalAlignment= HorizontalAlignment.Stretch,
        VerticalAlignment=VerticalAlignment.Stretch,
      };

      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 0 Result Property
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Star) }); // row 1 Result - Registration

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

      // Result - Property
      // Propery (row 0)

      // Gadget Name
      var textBlock = new TextBlock ()
      {
        Padding = new Thickness (2),
        FontWeight = FontWeights.SemiBold,
        Text = Model.ControlModel.GadgetName,
      };

      // Gadget Date
      var date = new TextBlock ()
      {
        Padding = new Thickness (2),
        FontWeight = FontWeights.SemiBold,
        Text = Model.ControlModel.Date.ToString (),
      };

      var stack = new StackPanel ()
      {
        Orientation = Orientation.Horizontal,
      };

      stack.Children.Add (textBlock);
      stack.Children.Add (date);

      // description 
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

      var stackAll = new StackPanel ();
      stackAll.Children.Add (stack);
      stackAll.Children.Add (textBox);

      var card = new MaterialDesignThemes.Wpf.Card
      {
        Margin = new Thickness (10, 0, 10, 10),
        Padding = new Thickness (2),
        HorizontalAlignment = HorizontalAlignment.Stretch,
        Content = stackAll,
      };

      card.SetValue (Grid.RowProperty, 0); // row 0
      m_Grid.Children.Add (card); // row 0

      // Result - Registration
      // Test (row 1 )
      var grid1 = new Grid ()
      {
        HorizontalAlignment = HorizontalAlignment.Stretch
      };

      grid1.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Star) }); // col 0 Result Test
      grid1.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Auto) }); // col 1 Result - Registration

      grid1.SetValue (Grid.RowProperty, 1); // row 1
      m_Grid.Children.Add (grid1); // row 1

      // TestList col 0
      var gadgets = new Collection<GadgetTest> ();
      Model.ControlModel.RequestContent (gadgets);

      var list = new ListBox ()
      {
        ItemsSource = gadgets,
        DisplayMemberPath= "GadgetName",
      };

      list.SetValue (Grid.ColumnProperty, 0); // col 0
      grid1.Children.Add (list); // col  0

      // Registration col 1
      var component = TActionComponent.Create (Server.Models.Infrastructure.TCategory.Registration);
      Model.ControlModel.RequestContent (component.Models.GadgetRegistrationModel);

      var componentControlModel = Registration.TComponentControlModel.CreateDefault;
      componentControlModel.SelectModel (component);

      var componentControl = new Registration.TComponentDisplayControl ()
      {
        Model = componentControlModel
      };

      componentControl.SetValue (Grid.ColumnProperty, 1); // col 1
      grid1.Children.Add (componentControl); // col  1
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