/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.ObjectModel;
using System.Globalization;
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
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch,
      };

      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 0 Result Property
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 1 Result - Registration

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
      var gadgetName = new TextBlock ()
      {
        Padding = new Thickness (2),
        FontWeight = FontWeights.SemiBold,
        Text = Model.ControlModel.GadgetName,
      };

      gadgetName.SetValue (Grid.ColumnProperty, 0); // col 0

      // Gadget Date
      var date = new TextBlock ()
      {
        Padding = new Thickness (2),
        FontWeight = FontWeights.SemiBold,
        Text = Model.ControlModel.Date.ToString ("dd-MMM-yy", CultureInfo.InvariantCulture),
        HorizontalAlignment = HorizontalAlignment.Right,
      };

      date.SetValue (Grid.ColumnProperty, 1); // col 1

      var gridTop = new Grid ();
      gridTop.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Star) }); // col 0 Info
      gridTop.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Star) }); // col 1 Date
      gridTop.SetValue (Grid.RowProperty, 0); // row 0
      gridTop.Children.Add (gadgetName); // col 0
      gridTop.Children.Add (date); // col 1


      // description 
      var description = new TextBox ()
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

      description.SetValue (Grid.RowProperty, 1); // row 1

      var gridCard = new Grid ();
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 0 
      m_Grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 1 
      gridCard.Children.Add (gridTop);
      gridCard.Children.Add (description);

      var card = new MaterialDesignThemes.Wpf.Card
      {
        Margin = new Thickness (1, 0, 1, 10),
        Padding = new Thickness (2),
        HorizontalAlignment = HorizontalAlignment.Stretch,
        Content = gridCard,
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

      #region Test List
      // Test col 0
      // TestList 
      var gadgets = new Collection<GadgetTest> ();
      Model.ControlModel.RequestContent (gadgets);

      var testInfo = new TextBlock
      {
        FontWeight = FontWeights.SemiBold,
        Text = $"test list [{gadgets.Count}]",
      };

      var testStack = new StackPanel ();
      testStack.SetValue (Grid.ColumnProperty, 0); // col 0
      testStack.Children.Add (testInfo);
      grid1.Children.Add (testStack); // col  0

      string itemTemplate = @"
          <DataTemplate
              xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              xmlns:converters='clr-namespace:rr.Library.Converter;assembly=rr.Library.Converter' >

              <DataTemplate.Resources>
                <converters:TCollectionToBitmapImageConverter  x:Key='CollectionToBitmapImageConverter' />
              </DataTemplate.Resources>

                <StackPanel>
                  <StackPanel Orientation='Horizontal'>
                        <Image Width='16'
                               Height='16'
                               ToolTip='{Binding Material}'
                                Source='{Binding GadgetImage, Converter={StaticResource CollectionToBitmapImageConverter}}' />

                        <ContentControl  Visibility='{Binding ContentTestVisibility}'
                                         Style='{DynamicResource GadgetTestMiniIcon}' />

                        <ContentControl  Visibility='{Binding ContentTargetVisibility}'
                                         Style='{DynamicResource GadgetTargetMiniIcon}' />

                        <TextBlock Margin='5 0 0 0'
                                   Text='{Binding GadgetName}'
                                   FontWeight='Bold' 
                                   Foreground='DarkBlue'
                                   VerticalAlignment='Center' />
                  </StackPanel>

                  <TextBox FontSize='10px' IsReadOnly='true' MaxWidth='400' TextWrapping='Wrap' Text='{Binding Description}' />

                  <ItemsControl Margin='10 5 0 0'
                                ItemsSource='{Binding ContentNamesFull}' />
                </StackPanel>
          </DataTemplate>"
      ;

      using (var sr = new System.IO.MemoryStream (System.Text.Encoding.UTF8.GetBytes (itemTemplate))) {
        var testList = new ListBox ()
        {
          Margin = new Thickness (0, 5, 0, 0),
          ItemsSource = gadgets,
          HorizontalAlignment = HorizontalAlignment.Left,
          ItemTemplate = System.Windows.Markup.XamlReader.Load (sr) as DataTemplate,
        };

        var scroll = new ScrollViewer ()
        {
          Margin = new Thickness (0, 0, 10, 0),
          Height = 230,
          HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
          VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
          Padding = new Thickness (5),
          Content = testList
        };

        testStack.Children.Add (scroll);
      }
      #endregion

      #region Registration
      // Registration col 1
      var component = TActionComponent.Create (Server.Models.Infrastructure.TCategory.Registration);
      Model.ControlModel.RequestContent (component.Models.GadgetRegistrationModel);

      var componentControlModel = Registration.TComponentControlModel.CreateDefault;
      componentControlModel.SelectModel (component);

      var componentControl = new Registration.TComponentDisplayControl ()
      {
        Model = componentControlModel,
        HorizontalAlignment = HorizontalAlignment.Right,
      };

      componentControl.SetValue (Grid.ColumnProperty, 1); // col 1
      grid1.Children.Add (componentControl); // col  1 
      #endregion
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
    readonly Grid m_Grid;
    #endregion
  };
  //---------------------------//

}  // namespace