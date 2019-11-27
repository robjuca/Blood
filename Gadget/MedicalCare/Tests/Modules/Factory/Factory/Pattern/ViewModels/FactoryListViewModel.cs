/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Shared.Gadget.Tests;

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
          // DatabaseValidated
          if (message.IsAction (TInternalMessageAction.DatabaseValidated)) {
            TDispatcher.Invoke (RequestDataDispatcher);
          }

          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Collection-Full
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full)) {
              if (message.Result.IsValid) {
                // Gadget Test
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Test)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }

                // Gadget Registration
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Registration)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }
              }
            }
          }
        }

        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.List)) {
          
        }
      }
    }
    #endregion

    #region Event
    public void OnCheckedRegistrationCommadClicked (object obj)
    {
      if (obj is TComponentModelItem item) {
        TDispatcher.BeginInvoke (RegistrationItemSelectedDispatcher, item);
      }
    }

    public void OnTestItemChecked (object obj)
    {
      if (obj is TComponentModelItem item) {
        TDispatcher.BeginInvoke (TestItemCheckedDispatcher, item);
      }
    }

    public void OnTestItemUnchecked (object obj)
    {
      if (obj is TComponentModelItem item) {
        TDispatcher.BeginInvoke (TestItemUncheckedDispatcher, item);
      }
    }

    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      RefreshCollection ("TestModelItemsViewSource");
      RefreshCollection ("RegistrationModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Test - Collection - Full 
      var action = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Registration,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);

      // Registration - Collection - Full 
      action = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Test,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      // Collection - Full (Registration list)
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void RegistrationItemSelectedDispatcher (TComponentModelItem item)
    {
      Model.RegistrationCurrentSelected (item);

      RaiseChanged ();
    }

    void TestItemCheckedDispatcher (TComponentModelItem item)
    {
      Model.TestSelected (item, isChecked:true);

      RaiseChanged ();
    }

    void TestItemUncheckedDispatcher (TComponentModelItem item)
    {
      Model.TestSelected (item, isChecked: false);

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