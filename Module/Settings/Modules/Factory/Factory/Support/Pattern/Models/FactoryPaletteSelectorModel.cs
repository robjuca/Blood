/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

using rr.Library.Types;
using Shared.Resources;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.Models
{
  public class TFactoryPaletteSelectorModel : NotificationObject
  {
    #region Property
    public IEnumerable<Swatch> Swatches
    {
      get;
    }

    public ObservableCollection<TProcessInfo> ProcessItemsSource
    {
      get;
    }

    public string ProcessCount
    {
      get
      {
        return ($"module process [{ProcessItemsSource.Count}]");
      }
    }

    public TProcessInfo CurrentProcess
    {
      get
      {
        return (RequestCurrentProcess ());
      }
    }

    public string CurrentProcessName
    {
      get
      {
        return (CurrentProcess.Name);
      }
    }

    public bool IsApplyEnabled
    {
      get; 
      set;
    }

    public bool BaseThemeDarkChecked
    {
      get; 
      set;
    }

    public TObservableCommand ToggleBaseCommand
    {
      get;
      private set;
    }

    public TObservableCommand ApplyPrimaryCommand
    {
      get;
      private set;
    }

    public TObservableCommand ApplyAccentCommand
    {
      get;
      private set;
    }
    #endregion

    #region Contructor
    public TFactoryPaletteSelectorModel ()
    {
      Swatches = new SwatchesProvider ().Swatches;

      ProcessItemsSource = new ObservableCollection<TProcessInfo> ();

      ToggleBaseCommand = new TObservableCommand (new DelegateCommand<bool> (ApplyBaseCommandHandler));
      ApplyPrimaryCommand = new TObservableCommand (new DelegateCommand<Swatch> (ApplyPrimaryCommandHandler));
      ApplyAccentCommand = new TObservableCommand (new DelegateCommand<Swatch> (ApplyAccentCommandHandler));
    } 
    #endregion

    #region Event
    void ApplyBaseCommandHandler (bool isDark)
    {
      var paletteHelper = new PaletteHelper ();

      ITheme themes = paletteHelper.GetTheme ();
      themes.SetBaseTheme (isDark ? Theme.Dark : Theme.Light);

      paletteHelper.SetTheme (themes);

      var baseTheme = isDark ? TProcess.PALETTETHEMEDARK : TProcess.PALETTETHEMELIGHT;
      CurrentProcess.PaletteInfo.SetBaseTheme (baseTheme);
    }

    void ApplyPrimaryCommandHandler (Swatch swatch)
    {
      var primaryColor = swatch.ExemplarHue.Color;

      var paletteHelper = new PaletteHelper ();

      ITheme themes = paletteHelper.GetTheme ();
      themes.SetPrimaryColor (primaryColor);

      paletteHelper.SetTheme (themes);

      CurrentProcess.PaletteInfo.SetPalettePrimary (primaryColor);
    }

    void ApplyAccentCommandHandler (Swatch swatch)
    {
      var secondaryColor = swatch.AccentExemplarHue.Color;

      var paletteHelper = new PaletteHelper ();

      ITheme themes = paletteHelper.GetTheme ();
      themes.SetSecondaryColor (secondaryColor);

      paletteHelper.SetTheme (themes);


      CurrentProcess.PaletteInfo.SetPaletteAccent (secondaryColor);
    }
    #endregion

    #region Members
    internal void CleanupProcess ()
    {
      ProcessItemsSource.Clear ();
    }

    internal void ProcessSelected (TProcessInfo processInfo)
    {
      BaseThemeDarkChecked = processInfo.IsBaseThemeDark;

      var paletteHelper = new PaletteHelper ();

      ITheme themes = paletteHelper.GetTheme ();
      themes.SetBaseTheme (BaseThemeDarkChecked ? Theme.Dark : Theme.Light);
      themes.SetPrimaryColor (processInfo.PaletteInfo.RequestPrimary ());
      themes.SetSecondaryColor (processInfo.PaletteInfo.RequestAccent ());

      paletteHelper.SetTheme (themes);
    }

    internal bool SelectProcess ()
    {
      bool res = false;
      IsApplyEnabled = false;

      if (ProcessItemsSource.Count.Equals (0).IsFalse ()) {
        var processInfo = ProcessItemsSource [0];
        processInfo.IsChecked = true;

        BaseThemeDarkChecked = processInfo.IsBaseThemeDark;

        IsApplyEnabled = true;
        res = true;
      }

      return (res);
    }

    internal void AddProcessInfo (string processName, bool isAlive, TPaletteInfo paletteInfo)
    {
      ProcessItemsSource.Add (new TProcessInfo (processName, isAlive, paletteInfo));
    }
    #endregion

    #region Support
    TProcessInfo RequestCurrentProcess ()
    {
      foreach (var item in ProcessItemsSource) {
        if (item.IsChecked) {
          return (item);
        }
      }

      return (TProcessInfo.CreateDefault);
    }
    #endregion
  };
  //---------------------------//

  //----- TProcessInfo
  public class TProcessInfo
  {
    #region Property
    public string Name
    {
      get;
      private set;
    }

    public bool IsAlive
    {
      get;
      private set;
    }

    public Visibility AliveOnVisibility
    {
      get
      {
        return (IsAlive ? Visibility.Visible : Visibility.Collapsed);
      }
    }

    public Visibility AliveOffVisibility
    {
      get
      {
        return (IsAlive ? Visibility.Collapsed : Visibility.Visible);
      }
    }

    public bool IsChecked
    {
      get;
      set;
    }

    public TPaletteInfo PaletteInfo
    {
      get; 
    }

    public bool IsBaseThemeDark
    {
      get
      {
        return (PaletteInfo.IsBaseThemeDark);
      }
    }
    #endregion

    #region Constructor
    public TProcessInfo (string name, bool isAlive, TPaletteInfo paletteInfo)
      : this ()
    {
      Name = name;
      IsAlive = isAlive;

      PaletteInfo.CopyFrom (paletteInfo);
    }

    public TProcessInfo (string name, bool isAlive)
      : this ()
    {
      Name = name;
      IsAlive = isAlive;
    }

    TProcessInfo ()
    {
      Name = string.Empty;
      IsAlive = false;

      PaletteInfo = TPaletteInfo.CreateDefault;
    }
    #endregion

    #region Members
    public void Checked ()
    {
      IsChecked = IsAlive;
    }

    public void CopyFrom (TProcessInfo alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        IsAlive = alias.IsAlive;
        IsChecked = alias.IsChecked;

        PaletteInfo.CopyFrom (alias.PaletteInfo);
      }
    }
    #endregion

    #region Static
    public static TProcessInfo CreateDefault => new TProcessInfo (); 
    #endregion
  };
  //---------------------------//

  //----- TPaletteInfo
  public class TPaletteInfo
  {
    #region Property
    public string BaseTheme
    {
      get;
      private set;
    }

    public string PalettePrimary
    {
      get;
      private set;
    }

    public Color PaletteColorPrimary
    {
      get;
      private set;
    }

    public string PaletteAccent
    {
      get;
      private set;
    }

    public Color PaletteColorAccent
    {
      get;
      private set;
    }

    public bool IsBaseThemeDark
    {
      get
      {
        return (BaseTheme.Equals (TProcess.PALETTETHEMEDARK, StringComparison.InvariantCulture));
      }
    }
    #endregion

    #region Constructor
    TPaletteInfo ()
    {
      BaseTheme = TProcess.PALETTETHEMELIGHT;
      PalettePrimary = "blue";
      PaletteAccent = "lime";
    }

    TPaletteInfo (string baseTheme, string primaryColor, string accentColor)
      : this ()
    {
      BaseTheme = string.IsNullOrEmpty (BaseTheme) ? BaseTheme : baseTheme;
      PalettePrimary = string.IsNullOrEmpty (primaryColor) ? PalettePrimary : primaryColor;
      PaletteAccent = string.IsNullOrEmpty (accentColor) ? PaletteAccent : accentColor;
    }
    #endregion

    #region Members
    internal void SetBaseTheme (string baseTheme)
    {
      BaseTheme = baseTheme;
    }

    internal void SetPalettePrimary (Color color)
    {
      PaletteColorPrimary = color;
      PalettePrimary = color.ToString (CultureInfo.InvariantCulture);
    }

    internal void SetPaletteAccent (Color color)
    {
      PaletteColorAccent = color;
      PaletteAccent = color.ToString (CultureInfo.InvariantCulture);
    }

    internal void CopyFrom (TPaletteInfo alias)
    {
      if (alias.NotNull ()) {
        SetBaseTheme (alias.BaseTheme);

        PalettePrimary = alias.PalettePrimary;
        PaletteAccent = alias.PaletteAccent;
      }
    }

    internal Color RequestPrimary ()
    {
      var color = Color.FromRgb (10, 10, 10);

      if (string.IsNullOrEmpty (PalettePrimary).IsFalse ()) {
        if (PalettePrimary.Contains ("#")) {
          Int32 iColorInt = Convert.ToInt32 (PalettePrimary.Substring (1), 16);
          System.Drawing.Color c = System.Drawing.Color.FromArgb (iColorInt);
          color = Color.FromRgb (c.R, c.G, c.B);
        }
      }

      return (color);
    }

    internal Color RequestAccent ()
    {
      var color = Color.FromRgb (100, 100, 100);

      if (PaletteAccent.Contains ("#")) {
        Int32 iColorInt = Convert.ToInt32 (PaletteAccent.Substring (1), 16);
        System.Drawing.Color c = System.Drawing.Color.FromArgb (iColorInt);
        color = Color.FromRgb (c.R, c.G, c.B);
      }

      return (color);
    }
    #endregion

    #region Static
    public static TPaletteInfo Create (string baseTheme, string primaryColor, string accentColor) => new TPaletteInfo (baseTheme, primaryColor, accentColor);
    public static TPaletteInfo CreateDefault => new TPaletteInfo ();
    #endregion
  };
  //---------------------------//

}  // namespace
