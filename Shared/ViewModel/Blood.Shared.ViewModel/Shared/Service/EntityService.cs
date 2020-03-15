/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Threading.Tasks;

using rr.Library.Types;
using rr.Library.Services;

using Server.Models.Infrastructure;
//---------------------------//

namespace Shared.ViewModel
{
  //----- TErrorEventArgs
  public class TErrorEventArgs
  {
    #region Property
    public TErrorMessage Error
    {
      get;
    }
    #endregion

    #region Constructor
    public TErrorEventArgs (TErrorMessage error)
    {
      Error = TErrorMessage.CreateDefault;
      Error.CopyFrom (error);
    }
    #endregion
  };
  //---------------------------//

  //----- TEntityService
  public class TEntityService : IEntityOperation
  {
    #region Property
    public TEntityService<IEntityDataContext> Service
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TEntityService ()
    {
    }
    #endregion

    #region Interface Members
    public void Operation (rr.Library.Services.TServiceAction<IEntityAction> serviceAction)
    {
      if (serviceAction.NotNull ()) {
        OperationAsync (serviceAction).ContinueWith (delegate
          {
          //?
        });
      }
    }
    #endregion

    #region Event
    public delegate void ShowErrorHandler (object sender, TErrorEventArgs e);

    public event ShowErrorHandler ShowError;
    #endregion

    #region Members
    public void SelectService (TEntityService<IEntityDataContext> service)
    {
      if (service.NotNull ()) {
        Service = service;
      }
    } 
    #endregion

    #region Await
    async Task OperationAsync (TServiceAction<IEntityAction> serviceAction)
    {
      if (Service.NotNull ()) {
        var param = serviceAction.Param as IEntityAction;
        string messageError = $"[{param.Operation.CategoryType.Category} - {param.Operation.Operation}]";

        try {
          var task = Service.OperationAsync (param);

          if (task == await Task.WhenAny (task).ConfigureAwait (false)) {
            if (task.Result.Result.IsValid.IsFalse ()) {
              var error = new TErrorMessage ("Database ERROR Services", messageError, task.Result.Result.ErrorContent as string)
              {
                Severity = TSeverity.Low
              };


              ErrorToShow (error);
            }

            serviceAction.ServiceArgs.Complete (task.Result, null);
          }
        }

        catch (Exception exception) {
          string msg = rr.Library.Helper.THelper.ExceptionStringFormat (serviceAction.ServiceArgs.CompletedCallbackName, exception);

          var error = new TErrorMessage ("Database ERROR", messageError, msg)
          {
            Severity = TSeverity.Low
          };

          ErrorToShow (error);

          serviceAction.ServiceArgs.Error = exception;
          serviceAction.ServiceArgs.Complete (param, null);
        }
      }
    }
    #endregion

    #region Static
    public static TEntityService CreateDefault => new TEntityService ();
    #endregion

    #region Support
    void ErrorToShow (TErrorMessage error)
    {
      ShowError?.Invoke (this, new TErrorEventArgs (error));
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
