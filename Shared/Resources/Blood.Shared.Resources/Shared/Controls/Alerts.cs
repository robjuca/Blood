/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
//---------------------------//

namespace Shared.Resources
{
  [TemplatePart (Name = PART_ALERTS, Type = typeof (Border))]
  [TemplatePart (Name = PART_BORDER_CAPTION, Type = typeof (Border))]
  [TemplatePart (Name = PART_BORDER_MESSAGE, Type = typeof (Border))]
  [TemplatePart (Name = PART_CAPTION, Type = typeof (TextBlock))]
  [TemplatePart (Name = PART_MESSAGE, Type = typeof (TextBox))]
  public sealed class TAlerts : Control
  {
    #region Property
    public TAlertsModel Model
    {
      get
      {
        return ((TAlertsModel) GetValue (ModelProperty));
      }

      set
      {
        SetValue (ModelProperty, value);
      }
    }
    #endregion

    #region Dependency Property
    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register ("Model", typeof (TAlertsModel), typeof (TAlerts), 
        new FrameworkPropertyMetadata (TAlertsModel.CreateDefault, ModelPropertyChanged));
    #endregion

    #region Constructor
    public TAlerts ()
    {
      DefaultStyleKey = typeof (TAlerts);
    }
    #endregion

    #region Members
    public void RefreshModel ()
    {
      var backgroundBorderCaption = new SolidColorBrush (Colors.White);
      var backgroundBorderMessage = new SolidColorBrush (Colors.White);

      var foregroundCaption = new SolidColorBrush (Colors.Black);
      var foregroundMessage = new SolidColorBrush (Colors.Black);

      switch (Model.Kind) {
        case TAlertsModel.TKind.Primary:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (0, 64, 135));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (204, 229, 255));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (204, 229, 255));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (0, 64, 135));
          break;

        case TAlertsModel.TKind.Secondary:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (78, 61, 65));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (226, 227, 229));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (226, 227, 229));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (78, 61, 65));
          break;

        case TAlertsModel.TKind.Success:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (21, 87, 36));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (212, 237, 218));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (212, 237, 218));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (21, 87, 36));
          break;

        case TAlertsModel.TKind.Danger:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (128, 28, 36));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (248, 215, 218));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (248, 215, 218));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (128, 28, 36));
          break;

        case TAlertsModel.TKind.Warning:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (151, 100, 4));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (255, 243, 205));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (255, 243, 205));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (151, 100, 4));
          break;

        case TAlertsModel.TKind.Info:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (38, 88, 102));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (209, 236, 241));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (209, 236, 241));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (38, 88, 102));
          break;

        case TAlertsModel.TKind.Light:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (140, 131, 134));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (254, 254, 254));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (254, 254, 254));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (140, 131, 134));
          break;

        case TAlertsModel.TKind.Dark:
          backgroundBorderCaption = new SolidColorBrush (Color.FromRgb (51, 35, 42));
          foregroundCaption = new SolidColorBrush (Color.FromRgb (214, 216, 217));

          backgroundBorderMessage = new SolidColorBrush (Color.FromRgb (214, 216, 217));
          foregroundMessage = new SolidColorBrush (Color.FromRgb (51, 35, 42));
          break;
      }

      if (GetTemplateChild (PART_BORDER_CAPTION) is Border borderCaption) {
        borderCaption.Background = backgroundBorderCaption;
      }

      if (GetTemplateChild (PART_CAPTION) is TextBlock textCaption) {
        textCaption.Text = Model.Caption;
        textCaption.Foreground = foregroundCaption;
      }

      if (GetTemplateChild (PART_BORDER_MESSAGE) is Border bordermessage) {
        bordermessage.Background = backgroundBorderMessage;
      }

      if (GetTemplateChild (PART_MESSAGE) is TextBox textMessage) {
        textMessage.Text = Model.Message;
        textMessage.Foreground = foregroundMessage;
      }

      if (GetTemplateChild (PART_ALERTS) is Border control) {
        control.Visibility = Model.IsOpen ? Visibility.Visible : Visibility.Collapsed;
      }
    } 
    #endregion

    #region Overrides
    public override void OnApplyTemplate ()
    {
      base.OnApplyTemplate ();

      RefreshModel ();
    }
    #endregion

    #region Event
    static void ModelPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TAlerts control) {
        if (e.NewValue is TAlertsModel model) {
          control.Model.CopyFrom (model);
          control.RefreshModel ();
        }
      }
    }
    #endregion

    #region Constants
    const string PART_ALERTS                                    = "PART_Alerts";
    const string PART_BORDER_CAPTION                            = "PART_BorderCaption";
    const string PART_BORDER_MESSAGE                            = "PART_BorderMessage";
    const string PART_CAPTION                                   = "PART_Caption";
    const string PART_MESSAGE                                   = "PART_Message";
    #endregion
  };
  //---------------------------//

}  // namespace