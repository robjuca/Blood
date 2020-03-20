/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Action;
using Server.Models.Infrastructure;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;
using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;

using Gadget.Collection.Presentation;
using Gadget.Collection.Pattern.Models;
//---------------------------//

namespace Gadget.Collection.Pattern.ViewModels
{
  [Export ("ModuleCollectionListViewModel", typeof (ICollectionListViewModel))]
  public class TCollectionListViewModel : TViewModelAware<TCollectionListModel>, IHandleMessageInternal, ICollectionListViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TCollectionListViewModel (ICollectionPresentation presentation)
      : base (presentation, new TCollectionListModel ())
    {
      TypeName = GetType ().Name;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.NotNull ()) {
        if (message.IsModule (TResource.TModule.Collection)) {
          // from parent
          if (message.Node.IsParentToMe (TChild.List)) {
            // DatabaseValidated
            if (message.IsAction (TInternalMessageAction.DatabaseValidated)) {
              TDispatcher.Invoke (RequestDataDispatcher);
            }

            // Response
            if (message.IsAction (TInternalMessageAction.Response)) {
              // Collection-Full
              if (message.Support.Argument.Types.IsOperation (TOperation.Collection, TExtension.Full)) {
                if (message.Result.IsValid) {
                  // Gadget Registration
                  if (message.Support.Argument.Types.IsOperationCategory (TCategory.Registration)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                  }

                  // Gadget Result
                  if (message.Support.Argument.Types.IsOperationCategory (TCategory.Result)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                  }
                }
              }

              // Select-ById
              if (message.Support.Argument.Types.IsOperation (TOperation.Select, TExtension.ById)) {
                if (message.Result.IsValid) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseModelDispatcher, action);
                }
              }

              // Select-Many
              if (message.Support.Argument.Types.IsOperation (TOperation.Select, TExtension.Many)) {
                if (message.Result.IsValid) {
                  if (message.Support.Argument.Types.IsOperationCategory (TCategory.Dummy)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (SelectManyDispatcher, action);
                  }
                }
              }
            }

            // Reload
            if (message.IsAction (TInternalMessageAction.Reload)) {
              TDispatcher.Invoke (RefreshAllDispatcher);
              TDispatcher.Invoke (RequestDataDispatcher);
            }
          }

          // from sibilig
          if (message.Node.IsSiblingToMe (TChild.List, TypeInfo)) {
            // Reload
            if (message.IsAction (TInternalMessageAction.Reload)) {
              TDispatcher.Invoke (RefreshAllDispatcher);
              TDispatcher.Invoke (RequestDataDispatcher);
              TDispatcher.Invoke (ReloadDispatcher);
            }
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnRegistrationChanged (GadgetRegistration gadget)
    {
      Model.RegistrationChanged (gadget);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    public void OnResultChanged (GadgetResult gadget)
    {
      TDispatcher.BeginInvoke (ResultChangedDispatcher, gadget);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      ApplyChanges ();

      RefreshCollection ("RegistrationItemsViewSource");
      RefreshCollection ("ItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Collection - Full (Result)
      var action = TEntityAction.Create (
        TCategory.Result,
        TOperation.Collection,
        TExtension.Full
      );

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);

      // to parent
      // Collection - Full (Registration)
      action = TEntityAction.Create (
        TCategory.Registration,
        TOperation.Collection,
        TExtension.Full
      );

      message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      var gadgets = new Collection<TActionComponent> ();

      // Collection - Full (Registration)
      if (action.CategoryType.IsCategory (TCategory.Registration)) {
        TActionConverter.Collection (TCategory.Registration, gadgets, action);
        Model.SelectRegistration (gadgets);
      }

      // Collection - Full (Result )
      if (action.CategoryType.IsCategory (TCategory.Result)) {
        TActionConverter.Collection (TCategory.Result, gadgets, action);

        Model.SelectResult (gadgets, action.IdCollection);

        // update
        // Test - Select - Many
        action.Operation.Select (TCategory.Dummy, TOperation.Select, TExtension.Many);
        
        var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void ResponseModelDispatcher (TEntityAction action)
    {
      // to Sibling (Select)
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (TComponentModelItem.Create (action));

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void SelectManyDispatcher (TEntityAction action)
    {
      var gadgets = new Collection<TActionComponent> ();
      TActionConverter.SelectMany (TCategory.Result, gadgets, action);

      Model.SelectMany (gadgets);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void ResultChangedDispatcher (GadgetResult gadget)
    {
      if (gadget.IsNull ()) {
        // to Sibling (Cleanup)
        var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      else {
        // to Sibling (Select)
        var component = TActionComponent.Create (TCategory.Result);
        component.Models.GadgetResultModel.CopyFrom (gadget);

        var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
        message.Support.Argument.Args.Select (component);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void ReloadDispatcher ()
    {
      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Reload, TChild.List, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
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