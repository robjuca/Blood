/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

using rr.Library.Types;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.Models
{
  public class TFactoryPaletteSelectorModel
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
        return (RequestCurrentProcess ().Name);
      }
    }

    public ICommand ToggleBaseCommand { get; } = new AnotherCommandImplementation (o => ApplyBase ((bool) o));
    public ICommand ApplyAccentCommand { get; } = new AnotherCommandImplementation (o => ApplyAccent ((Swatch) o));
    public ICommand ApplyPrimaryCommand { get; } = new AnotherCommandImplementation (o => ApplyPrimary ((Swatch) o));
    #endregion

    #region Contructor
    public TFactoryPaletteSelectorModel ()
    {
      Swatches = new SwatchesProvider ().Swatches;

      ProcessItemsSource = new ObservableCollection<TProcessInfo> ();
    } 
    #endregion

    #region Event
    static void ApplyBase (bool isDark)
    {
      new PaletteHelper ().SetLightDark (isDark);
    }

    static void ApplyPrimary (Swatch swatch)
    {
      new PaletteHelper ().ReplacePrimaryColor (swatch);
    }

    static void ApplyAccent (Swatch swatch)
    {
      new PaletteHelper ().ReplaceAccentColor (swatch);
    }
    #endregion

    #region Members
    internal void Select (string names, string alive)
    {
      names.ThrowNull ();
      alive.ThrowNull ();

      ProcessItemsSource.Clear ();

      var processNames = names.Split ('?');
      var processAlive = alive.Split ('?');

      for (int i = 0; i < processNames.Length; i++) {
        var nameInfo = processNames [i];
        var aliveInfo = processAlive [i];

        if (string.IsNullOrEmpty (nameInfo) || string.IsNullOrEmpty (aliveInfo)) {
          continue;
        }

        var processInfo = new TProcessInfo (nameInfo, bool.Parse (aliveInfo));
        ProcessItemsSource.Add (processInfo);
      }

      if (ProcessItemsSource.Count.Equals (0).IsFalse ()) {
        ProcessItemsSource [0].Checked ();
      }
    }
    #endregion

    #region Support
    TProcessInfo RequestCurrentProcess ()
    {
      var process = TProcessInfo.CreateDefault;

      foreach (var item in ProcessItemsSource) {
        if (item.IsChecked) {
          process.CopyFrom (item);
          break;
        }
      }

      return (process);
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
    #endregion

    #region Constructor
    public TProcessInfo (string name, bool isAlive)
    {
      Name = name;
      IsAlive = isAlive;
    }

    TProcessInfo ()
    {
      Name = string.Empty;
      IsAlive = false;
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
      }
    }
    #endregion

    #region Static
    public static TProcessInfo CreateDefault => new TProcessInfo (); 
    #endregion
  };
  //---------------------------//

}  // namespace
