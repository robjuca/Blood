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

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryListViewModel", typeof (IFactoryListViewModel))]
  public class TFactoryListViewModel : TViewModelAware<TFactoryListModel>, IHandleMessageInternal, IInternalHandleParent, IFactoryListViewModel
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
                // Gadget Material
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Material)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (MaterialCollectionFullDispatcher, action);
                }
              }
            }
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.List)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            if (message.Support.Argument.Args.PropertyName.Equals ("Edit")) {
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

    public void InternalHandle (object message)
    {
      // used to childView comunicate with parentView
      if (message.NotNull ()) {
        if (message is TFactorySiblingMessageInternal msg) {
          // PropertySelect
          if (msg.IsAction (TInternalMessageAction.PropertySelect)) {
            Model.PropertyChanged (msg.Support.Argument.Args.PropertyName, msg.Support.Argument.Types.ReportData.Locked);

            TDispatcher.Invoke (RefreshAllDispatcher);
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnMaterialSelectionChanged (int selectedIndex)
    {
      if (selectedIndex.Equals (-1).IsFalse ()) {
        TDispatcher.Invoke (MaterialSelectionChangedDispatcher);
      }
    }

    public void OnSelectorTargetCommadClicked ()
    {
      Model.SlideIndex = 0;

      RaiseChanged ();
    }

    public void OnSelectorTestCommadClicked ()
    {
      Model.SlideIndex = 1;

      RaiseChanged ();
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      RefreshCollection ("MaterialModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Collection - Full (Material list - used to send RefreshModel)
      var entityAction = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Material,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (entityAction);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void MaterialCollectionFullDispatcher (Server.Models.Component.TEntityAction action)
    {
      // refresh model
      Model.MaterialRefreshModel (action);
      
      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.EditEnter (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void MaterialSelectionChangedDispatcher ()
    {
      var message = new TFactoryMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (Model.MaterialSelectionCurrent);

      NotifyChildViewModel (message);
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