/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Report
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetReport ControlModel
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetReport.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (TActionComponent component)
    {
      if (component.NotNull ()) {
        ControlModel.CopyFrom (component.Models.GadgetReportModel);
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace