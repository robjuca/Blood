/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

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
              // Collection - Full
              if (message.Support.Argument.Types.IsOperation (TOperation.Collection, TExtension.Full)) {
                if (message.Result.IsValid) {
                  // Gadget Test
                  if (message.Support.Argument.Types.IsOperationCategory (TCategory.Test)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                  }
                }
              }

              // Select - ById
              if (message.Support.Argument.Types.IsOperation (TOperation.Select, TExtension.ById)) {
                if (message.Result.IsValid) {
                  // Gadget Test
                  if (message.Support.Argument.Types.IsOperationCategory (TCategory.Test)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, action);
                  }
                }
              }
            }

            // Reload
            if (message.IsAction (TInternalMessageAction.Reload)) {
              TDispatcher.Invoke (RefreshAllDispatcher);
              TDispatcher.Invoke (RequestDataDispatcher);
              TDispatcher.Invoke (ReloadDispatcher);
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
    public void OnSelectionChanged (GadgetTest gadget)
    {
      TDispatcher.BeginInvoke (SelectionChangedDispatcher, gadget);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      ApplyChanges ();

      RefreshCollection ("ModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Collection - Full
      var action = TEntityAction.Create (
        TCategory.Test,
        TOperation.Collection,
        TExtension.Full
      );

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction entityAction)
    {
      Model.Select (entityAction);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void SelectionChangedDispatcher (GadgetTest gadget)
    {
      if (Model.SelectionChanged (gadget)) {
        // to parent
        // Select - ById
        var entityAction = TEntityAction.Create (
          TCategory.Test,
          TOperation.Select,
          TExtension.ById
        );

        entityAction.Id = Model.Current.Id;

        var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (entityAction);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      else {
        // to Sibling
        var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void ResponseSelectByIdDispatcher (TEntityAction entityAction)
    {
      var component = TActionComponent.Create (TCategory.Test);
      TActionConverter.Select (TCategory.Test, component, entityAction);

      // to Sibling
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (component);

      DelegateCommand.PublishInternalMessage.Execute (message);
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