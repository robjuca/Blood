/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.Generic;
//---------------------------//

namespace Shared.Resources
{
  public static class TProcess
  {
    #region Data
    public enum TName
    {
      ModuleSettings,
      GadgetMaterial,
      GadgetTarget,
      GadgetTest,
    };
    #endregion

    #region Property
    public static Dictionary<TName, string> ModuleKey
    {
      get;
    }

    public static Dictionary<TName, string> ModuleExecutable
    {
      get;
    }

    public static string GADGETMATERIAL => TName.GadgetMaterial.ToString ();

    public static string GADGETTARGET => TName.GadgetTarget.ToString ();

    public static string GADGETTEST => TName.GadgetTest.ToString ();

    public static string MODULESETTINGS => TName.ModuleSettings.ToString ();
    #endregion

    #region Constructor
    static TProcess ()
    {
      ModuleKey = new Dictionary<TName, string>
      {
        { TName.ModuleSettings, "Module.Settings" },
        { TName.GadgetMaterial, "Gadget.Material" },
        { TName.GadgetTarget, "Gadget.Target" },
        { TName.GadgetTest, "Gadget.Test" },
      };

      ModuleExecutable = new Dictionary<TName, string> ();

      foreach (var moduleKey in ModuleKey) {
        var pe = $"Blood.{moduleKey.Value}.exe";

        ModuleExecutable.Add (moduleKey.Key, pe);
      }
    }
    #endregion

  };
  //---------------------------//

}  // namespace