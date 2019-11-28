/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.Message;
using Shared.ViewModel;

using Module.Settings.Factory.Support.Presentation;
using Module.Settings.Factory.Support.Pattern.Models;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.ViewModels
{
  [Export ("ModuleSettingsFactorySupportPaletteSelectorViewModel", typeof (IFactoryPaletteSelectorViewModel))]
  public class TFactoryPaletteSelectorViewModel : TViewModelAware<TFactoryPaletteSelectorModel>, IHandleMessageModule, IFactoryPaletteSelectorViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryPaletteSelectorViewModel (IFactorySupportPresentation presentation)
      : base (new TFactoryPaletteSelectorModel ())
    {
      presentation.ViewModel = this;
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        
      }
    }
    #endregion

    

    #region Property
    IDelegateCommand DelegateCommand
    {
      get
      {
        return (PresentationCommand as IDelegateCommand);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace