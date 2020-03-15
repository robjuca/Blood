/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  //----- TActionModel
  public class TActionModel
  {
    #region Property
    public GadgetRegistration GadgetRegistrationModel
    {
      get;
    }

    public GadgetMaterial GadgetMaterialModel
    {
      get;
    }

    public GadgetTarget GadgetTargetModel
    {
      get;
    }

    public GadgetTest GadgetTestModel
    {
      get;
    }

    public GadgetResult GadgetResultModel
    {
      get;
    }

    public GadgetReport GadgetReportModel
    {
      get;
    }
    #endregion

    #region Constructor
    TActionModel ()
    {
      GadgetRegistrationModel = GadgetRegistration.CreateDefault;
      GadgetMaterialModel = GadgetMaterial.CreateDefault;
      GadgetTargetModel = GadgetTarget.CreateDefault;
      GadgetTestModel = GadgetTest.CreateDefault;
      GadgetResultModel = GadgetResult.CreateDefault;
      GadgetReportModel = GadgetReport.CreateDefault;
    }
    #endregion

    #region Static
    public static TActionModel CreateDefault => new TActionModel ();
    #endregion
  };
  //---------------------------//

}  // namespace