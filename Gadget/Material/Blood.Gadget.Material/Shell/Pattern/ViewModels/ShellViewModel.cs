/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using Shared.Resources;
using Shared.ViewModel;

using Gadget.Material.Shell.Presentation;
using Gadget.Material.Shell.Pattern.Models;
//---------------------------//

namespace Gadget.Material.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TShellViewModel<TShellModel>, IShellViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (new TShellModel (), TProcess.GADGETMATERIAL)
    {
      presentation.ViewModel = this;
    }
    #endregion

    #region View Event
    
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

    #region Field
    #endregion
  };
  //---------------------------//

}  // namespace