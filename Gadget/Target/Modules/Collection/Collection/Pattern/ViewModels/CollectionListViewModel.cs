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
              if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full)) {
                if (message.Result.IsValid) {
                  // Gadget Target
                  if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Target)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                  }

                  // Gadget Material
                  if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Material)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (RefreshModelDispatcher, action);
                  }
                }
              }

              // Select-ById
              if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
                if (message.Result.IsValid) {
                  var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseModelDispatcher, action);
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
    public void OnMaterialSelectionChanged ()
    {
      TDispatcher.Invoke (MaterialSelectionChangedDispatcher);
    }

    public void OnTargetSelectionChanged ()
    {
      TDispatcher.Invoke (TargetItemSelectedDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      ApplyChanges ();

      RefreshCollection ("MaterialModelItemsViewSource");
      RefreshCollection ("TargetModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Collection - Full (Target)
      var action = TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Target,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      // Collection - Full (Target list)
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);

      // to parent
      // Collection - Full (Material list - used to send RefreshModel)
      var entityAction = TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Material,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (entityAction);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseModelDispatcher (TEntityAction action)
    {
      // to Sibling (Select)
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (TComponentModelItem.Create (action));

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RefreshModelDispatcher (TEntityAction action)
    {
      // refresh model
      Model.RefreshModel (action);
      TDispatcher.Invoke (RefreshAllDispatcher);

      // to parent (RefreshModel)
      var message = new TCollectionMessageInternal (TInternalMessageAction.RefreshModel, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void MaterialSelectionChangedDispatcher ()
    {
      Model.MaterialChanged ();

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void TargetItemSelectedDispatcher ()
    {
      if (Model.TargetCurrent.ValidateId) {
        // to Sibling (Select)
        var component = TActionComponent.Create (TCategory.Target);
        Model.Request (component);

        var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
        message.Support.Argument.Args.Select (component);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      else {
        // to Sibling (Cleanup)
        var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
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