/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using rr.Library.Helper;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TEntityActionBase<T> : IEntityAction
    where T : TCategoryType
  {
    #region Property
    public T CategoryType
    {
      get;
      private set;
    }

    public Collection<Guid> IdCollection
    {
      get;
      private set;
    }

    public Dictionary<Guid, Collection<Guid>> IdDictionary
    {
      get;
      private set;
    }

    public TValidationResult Result
    {
      get;
      set;
    }

    public string ConnectionString
    {
      get;
      set;
    }

    public Guid Id
    {
      get;
      set;
    }

    public object Param1
    {
      get;
      set;
    }

    public object Param2
    {
      get;
      set;
    }

    public TEntityOperation<TCategoryType> Operation
    {
      get;
    }

    public TSupportAction SupportAction
    {
      get;
    }
    #endregion

    #region Constructor
    protected TEntityActionBase ()
    {
      IdCollection = new Collection<Guid> ();
      IdDictionary = new Dictionary<Guid, Collection<Guid>> ();

      Result = TValidationResult.CreateDefault;

      var categoryType = TCategoryType.Create (TCategory.None);
      Operation = new TEntityOperation<TCategoryType> (categoryType);

      SupportAction = TSupportAction.CreateDefault;
    }

    public TEntityActionBase (T categoryType, string connectionString)
      : this ()
    {
      CategoryType = categoryType;

      ConnectionString = connectionString;
      Operation.Select (categoryType);
    }

    public TEntityActionBase (T categoryType, string connectionString, object param1, object param2)
      : this ()
    {
      CategoryType = categoryType;

      ConnectionString = connectionString;
      Operation.Select (categoryType);

      Param1 = param1;
      Param2 = param2;
    }
    #endregion

    #region Members
    public void SelectConnection (string connectionString)
    {
      ConnectionString = connectionString;
    }

    public void SelectCategoryType (T categoryType)
    {
      CategoryType.CopyFrom (categoryType);
    }

    public void CopyFrom (IList<Guid> alias)
    {
      if (alias.NotNull ()) {
        IdCollection = new Collection<Guid> (alias);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace