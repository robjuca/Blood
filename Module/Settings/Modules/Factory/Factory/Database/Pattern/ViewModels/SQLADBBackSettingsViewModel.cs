﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;
using rr.Library.Types;

using Shared.Types;
using Shared.ViewModel;

using Module.Settings.Factory.Database.Presentation;
using Module.Settings.Factory.Database.Pattern.Models;
//---------------------------//

namespace Module.Settings.Factory.Database.Pattern.ViewModels
{
  [Export ("ModuleSQLADBBackSettingsViewModel", typeof (ISQLADBBackSettingsViewModel))]
  public class TSQLADBBackSettingsViewModel : TViewModelAware<TSQLADBBackSettingsModel>, IHandleMessageInternal, ISQLADBBackSettingsViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TSQLADBBackSettingsViewModel (IFactoryDatabasePresentation presentation)
      : base (presentation, new TSQLADBBackSettingsModel ())
    {
      TypeName = GetType ().Name;
    }
    #endregion

    #region Handle
    public void Handle (TMessageInternal message)
    {
      // from sibiling
      if (message.NotNull ()) {
        if (message.Node.IsSiblingToMe (TChild.Back, TypeInfo)) {
          if (message.Support.Argument.Types.Authentication.Equals (TAuthentication.SQL)) {
            if (message.IsAction (TInternalMessageAction.Select)) {
              Model.ClearCheck ();
              Model.Populate (message.Support.Argument.Types.ConnectionData);
              ApplyChanges ();
            }
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnDatabaseApplyCommadClicked ()
    {
      Model.Apply ();
      ApplyChanges ();

      // to sibiling front
      var message = new TFactoryMessageInternal (TInternalMessageAction.Change, TAuthentication.SQL, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Back);
      message.Support.Argument.Types.ConnectionData.CopyFrom (Model.DatabaseAuthentication);

      DelegateCommand.PublishInternalMessage.Execute (message);

      TDispatcher.Invoke (NotifyChageDispatcher);
    }

    public void OnBackCommadClicked ()
    {
      Model.ClearCheck ();
      ApplyChanges ();

      // to sibiling 
      var message = new TFactoryMessageInternal (TInternalMessageAction.EditLeave, TAuthentication.Windows, TypeInfo);
      message.Node.SelectRelationSibling (TChild.None);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Dispatcher
    void NotifyChageDispatcher ()
    {
      // notify parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Change, TypeInfo);
      message.Node.SelectRelationChild (TChild.Back);
      message.Support.Argument.Types.ConnectionData.CopyFrom (Model.DatabaseAuthentication);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      // to sibiling
      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TAuthentication.SQL, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Back);

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