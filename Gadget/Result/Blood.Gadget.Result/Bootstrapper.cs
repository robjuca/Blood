﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition.Hosting;

using rr.Library.Infrastructure;
//---------------------------//

namespace Gadget.Result
{
  public class TBootstrapper : TBootstrapper<Shared.ViewModel.IShellViewModel>
  {
    #region Overrides
    protected override void ConfigureCatalog ()
    {
      m_CollectionCatalog = new AssemblyCatalog (typeof (Collection.TModuleCatalog).Assembly);
      m_FactoryCatalog = new AssemblyCatalog (typeof (Factory.TModuleCatalog).Assembly);
      m_ServicesCatalog = new AssemblyCatalog (typeof (Shared.Services.TModuleCatalog).Assembly);

      // modules
      AddToCatalog (m_CollectionCatalog);
      AddToCatalog (m_FactoryCatalog);
      AddToCatalog (m_ServicesCatalog);

      // create instance
      GetInstance (typeof (Shared.Services.Presentation.IServicesPresentation), null);
    }

    protected override void Dispose (bool disposing)
    {
      m_CollectionCatalog.Dispose ();
      m_FactoryCatalog.Dispose ();
      m_ServicesCatalog.Dispose ();

      base.Dispose (disposing);
    }
    #endregion

    #region Fields
    AssemblyCatalog                                   m_CollectionCatalog;
    AssemblyCatalog                                   m_FactoryCatalog;
    AssemblyCatalog                                   m_ServicesCatalog;
    #endregion
  };
  //---------------------------//

}  // namespace