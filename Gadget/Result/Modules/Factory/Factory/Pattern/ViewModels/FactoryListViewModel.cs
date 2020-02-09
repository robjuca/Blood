﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;
using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;

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
            if (message.Support.Argument.Types.IsOperation (TOperation.Collection, TExtension.Full)) {
              if (message.Result.IsValid) {
                // Gadget Registration
                if (message.Support.Argument.Types.IsOperationCategory (TCategory.Registration)) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }

                // Gadget Test
                if (message.Support.Argument.Types.IsOperationCategory (TCategory.Test)) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }
              }
            }

            // Select-Many
            if (message.Support.Argument.Types.IsOperation (TOperation.Select, TExtension.Many)) {
              if (message.Result.IsValid) {
                // Gadget Test
                if (message.Support.Argument.Types.IsOperationCategory (TCategory.Test)) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (SelectManyDispatcher, action);
                }
              }
            }
          }
        }

        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.List, TypeInfo)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            var propertyName = message.Support.Argument.Args.PropertyName;

            if (propertyName.Equals ("edit")) {
              TDispatcher.BeginInvoke (EditDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
            }
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestModelDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnCheckedRegistrationCommadClicked (object obj)
    {
      if (obj is GadgetRegistration gadget) {
        TDispatcher.BeginInvoke (RegistrationItemSelectedDispatcher, gadget);
      }
    }

    public void OnTestItemChecked (object obj)
    {
      if (obj is TActionComponent component) {
        TDispatcher.BeginInvoke (TestItemCheckedDispatcher, component);
      }
    }

    public void OnTestItemUnchecked (object obj)
    {
      if (obj is TActionComponent component) {
        TDispatcher.BeginInvoke (TestItemUncheckedDispatcher, component);
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
      // Registration - Collection - Full 
      var action = TEntityAction.Create (
        TCategory.Registration,
        TOperation.Collection,
        TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);

      // Test - Collection - Full 
      action = TEntityAction.Create (
        TCategory.Test,
        TOperation.Collection,
        TExtension.Full
      );

      message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      var gadgets = new Collection<TActionComponent> ();

      // Collection - Full (Registration, Test)

      // Registration
      if (action.CategoryType.IsCategory (TCategory.Registration)) {
        TActionConverter.Collection (TCategory.Registration, gadgets, action);
        Model.SelectRegistration (gadgets);
      }

      // Test
      if (action.CategoryType.IsCategory (TCategory.Test)) {
        TActionConverter.Collection (TCategory.Test, gadgets, action);
        Model.SelectTest (gadgets);

        // update
        // Test - Select - Many
        action.Operation.Select (TCategory.Test, TOperation.Select, TExtension.Many);
        Model.RequestTestIdCollection (action.IdCollection);

        var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void RequestModelDispatcher (TEntityAction action)
    {
      if (action.NotNull ()) {
        var component = TActionComponent.Create (TCategory.Result);
        Model.Request (component);

        TActionConverter.Request (TCategory.Result, component, action);

        // to Sibling
        var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void SelectManyDispatcher (TEntityAction action)
    {
      // Test
      if (action.CategoryType.IsCategory (TCategory.Test)) {
        var gadgets = new Collection<TActionComponent> ();
        Model.RequestTestCollection (gadgets);

        TActionConverter.SelectMany (TCategory.Test, gadgets, action);

        Model.SelectTestMany (gadgets);
      }

      RaiseChanged ();
    }

    void RegistrationItemSelectedDispatcher (GadgetRegistration gadget)
    {
      Model.RegistrationCurrentSelected (gadget);

      RaiseChanged ();
    }

    void TestItemCheckedDispatcher (TActionComponent component)
    {
      Model.TestSelected (component, isChecked: true);

      RaiseChanged ();
    }

    void TestItemUncheckedDispatcher (TActionComponent component)
    {
      Model.TestSelected (component, isChecked: false);

      RaiseChanged ();
    }

    void EditDispatcher (TEntityAction action)
    {
      //Model.EditEnter (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
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