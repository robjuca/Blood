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
      : base (presentation, new TFactoryListModel ())
    {
      TypeName = GetType ().Name;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.NotNull ()) {
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
                  // Gadget Material
                  if (message.Support.Argument.Types.IsOperationCategory (TCategory.Material)) {
                    var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                    TDispatcher.BeginInvoke (MaterialCollectionFullDispatcher, action);
                  }
                }
              }
            }
          }

          // from sibilig
          if (message.Node.IsSiblingToMe (TChild.List, TypeInfo)) {
            // PropertySelect
            if (message.IsAction (TInternalMessageAction.PropertySelect)) {
              var propertyName = message.Support.Argument.Args.PropertyName;

              if (propertyName.Equals ("edit", StringComparison.InvariantCulture)) {
                if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                  TDispatcher.BeginInvoke (EditDispatcher, component);
                }
              }

              if (propertyName.Equals ("GadgetAdd", StringComparison.InvariantCulture) || propertyName.Equals ("GadgetRemove", StringComparison.InvariantCulture)) {
                Model.PropertyChanged (message.Support.Argument.Args.PropertyName, message.Support.Argument.Types.ReportData.Locked);

                TDispatcher.Invoke (RefreshAllDispatcher);
              }
            }

            // Reload
            if (message.IsAction (TInternalMessageAction.Reload)) {
              // to parent
              DelegateCommand.PublishInternalMessage.Execute (message);
            }

            // Cleanup
            if (message.IsAction (TInternalMessageAction.Cleanup)) {
              Model.Cleanup ();
              TDispatcher.Invoke (RefreshAllDispatcher);
            }
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnMaterialSelectionChanged ()
    {
      TDispatcher.Invoke (MaterialSelectionChangedDispatcher);
    }

    public void OnSelectorTargetCommadClicked ()
    {
      Model.SlideIndex = 0;

      ApplyChanges ();
    }

    public void OnSelectorTestCommadClicked ()
    {
      Model.SlideIndex = 1;

      ApplyChanges ();
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      ApplyChanges ();

      RefreshCollection ("MaterialModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Material - Collection - Full (Material list - used to send RefreshModel)
      var entityAction = TEntityAction.Create (
        TCategory.Material,
        TOperation.Collection,
        TExtension.Full
      );

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (entityAction);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void MaterialCollectionFullDispatcher (TEntityAction action)
    {
      // refresh model
      Model.MaterialRefreshModel (action);
      
      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void EditDispatcher (TActionComponent component)
    {
      Model.EditEnter (component);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void MaterialSelectionChangedDispatcher ()
    {
      var component = TActionComponent.Create (TCategory.Material);
      Model.Request (component);

      // to sibling
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (component);

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