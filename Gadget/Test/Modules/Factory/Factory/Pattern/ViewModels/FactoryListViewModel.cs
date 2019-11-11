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
using Shared.ViewModel;

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryListViewModel", typeof (IFactoryListViewModel))]
  public class TFactoryListViewModel : TViewModelAware<TFactoryListModel>, IHandleMessageInternal, IFactoryListViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListViewModel (IFactoryPresentation presentation)
      : base (new TFactoryListModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from parent
        if (message.Node.IsParentToMe (TChild.List)) {
          
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.List)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            if (message.Support.Argument.Args.PropertyName.Equals ("all")) {
              TDispatcher.BeginInvoke (EditDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
            }
          }

          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            // to parent
            DelegateCommand.PublishInternalMessage.Execute (message);
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnTestListCommadClicked ()
    {
      Model.SlideIndex = 1;

      RaiseChanged ();
    }

    public void OnTargetListCommadClicked ()
    {
      Model.SlideIndex = 0;

      RaiseChanged ();
    }
    #endregion

    #region Dispatcher
    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      var relationCategory = Server.Models.Infrastructure.TCategoryType.FromValue (action.ModelAction.GadgetTestModel.RelationCategory);

      Model.SlideIndex =
        relationCategory.Equals (Server.Models.Infrastructure.TCategory.Test) ? 1 :
        relationCategory.Equals (Server.Models.Infrastructure.TCategory.Target) ? 0 : 
        Model.SlideIndex
      ;

      RaiseChanged ();
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