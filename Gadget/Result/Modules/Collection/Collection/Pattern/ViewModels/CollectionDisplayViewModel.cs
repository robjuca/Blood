/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Infrastructure;
using Server.Models.Action;

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;
using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
using Shared.Gadget.Result;

using Gadget.Collection.Presentation;
using Gadget.Collection.Pattern.Models;
//---------------------------//

namespace Gadget.Collection.Pattern.ViewModels
{
  [Export ("ModuleCollectionDisplayViewModel", typeof (ICollectionDisplayViewModel))]
  public class TCollectionDisplayViewModel : TViewModelAware<TCollectionDisplayModel>, IHandleMessageInternal, ICollectionDisplayViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TCollectionDisplayViewModel (ICollectionPresentation presentation)
      : base (presentation, new TCollectionDisplayModel ())
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
          if (message.Node.IsParentToMe (TChild.Display)) {
            // Response
            if (message.IsAction (TInternalMessageAction.Response)) {
              // Remove
              if (message.Support.Argument.Types.IsOperation (TOperation.Remove)) {
                if (message.Result.IsValid) {
                  Model.Cleanup ();
                  ApplyChanges ();

                  // notify List            
                  TDispatcher.Invoke (ReloadDispatcher);
                }
              }
            }
          }

          // from sibilig
          if (message.Node.IsSiblingToMe (TChild.Display, TypeInfo)) {
            // Select
            if (message.IsAction (TInternalMessageAction.Select)) {
              if (message.Support.Argument.Args.Param1 is TActionComponent component) {
                if (message.Support.Argument.Args.Param2 is Dictionary<Guid, GadgetMaterial> materialDictionary) {
                  var tuple = Tuple.Create (component, materialDictionary);

                  TDispatcher.BeginInvoke (SelectDispatcher, tuple);
                }
              }
            }

            // Cleanup
            if (message.IsAction (TInternalMessageAction.Cleanup)) {
              TDispatcher.Invoke (CleanupDispatcher);
            }
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnEditCommadClicked ()
    {
      TDispatcher.Invoke (EditDispatcher);
    }

    public void OnRemoveCommadClicked ()
    {
      TDispatcher.Invoke (RemoveDispatcher);
    }

    public void OnModifyCommadClicked ()
    {
      TDispatcher.Invoke (ModifyDispatcher);
    }
    #endregion

    #region Dispatcher
    void SelectDispatcher (Tuple<TActionComponent, Dictionary<Guid, GadgetMaterial>> tuple)
    {
      if (tuple.NotNull ()) {
        Model.Select (tuple.Item1, tuple.Item2);
      }

      if (FrameworkElementView.FindName ("DisplayControl") is TComponentDisplayControl control) {
        control.RefreshDesign ();
      }

      ApplyChanges ();
    }

    void EditDispatcher ()
    {
      var component = TActionComponent.Create (TCategory.Result);
      Model.Request (component);

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Edit, TChild.Display, TypeInfo);
      message.Support.Argument.Args.Select (component);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RemoveDispatcher ()
    {
      // Remove
      var action = TEntityAction.Create (TCategory.Result, TOperation.Remove);
      action.Id = Model.Id;

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ModifyDispatcher ()
    {
      var component = TActionComponent.Create (TCategory.Result);
      Model.Request (component);

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Modify, TChild.Display, TypeInfo);
      message.Support.Argument.Args.Select (component);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ReloadDispatcher ()
    {
      // to Sibling
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Reload, TChild.Display, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void CleanupDispatcher ()
    {
      Model.Cleanup ();
      ApplyChanges ();
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