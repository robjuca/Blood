/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.Generic;
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

    public ICommand ToggleBaseCommand { get; } = new AnotherCommandImplementation (o => ApplyBase ((bool) o));
    public ICommand ApplyAccentCommand { get; } = new AnotherCommandImplementation (o => ApplyAccent ((Swatch) o));
    public ICommand ApplyPrimaryCommand { get; } = new AnotherCommandImplementation (o => ApplyPrimary ((Swatch) o));
    #endregion

    #region Contructor
    public TFactoryPaletteSelectorModel ()
    {
      Swatches = new SwatchesProvider ().Swatches;
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

  };
  //---------------------------//

}  // namespace
