/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListItemInfo
  {
    #region Property
    public TComponentModelItem ModelItem
    {
      get;
    }

    public Guid Id
    {
      get
      {
        return (ModelItem.Id);
      }
    }

    public bool IsEmpty
    {
      get
      {
        return (Id.Equals (Guid.Empty));
      }
    }

    public bool IsChecked
    {
      get; 
      set;
    }

    public string Name
    {
      get
      {
        return (ModelItem.Name);
      }
    }

    public int CategoryValue
    {
      get
      {
        return (Server.Models.Infrastructure.TCategoryType.ToValue (ModelItem.Category));
      }
    }
    #endregion

    #region Constructor
    TFactoryListItemInfo ()
    {
      ModelItem = TComponentModelItem.CreateDefault;
    }

    TFactoryListItemInfo (TComponentModelItem item)
      : this ()
    {
      ModelItem.CopyFrom (item);
    }

    TFactoryListItemInfo (TComponentModelItem item, bool isChecked)
      : this ()
    {
      ModelItem.CopyFrom (item);

      IsChecked = isChecked;
    }
    #endregion

    #region Members]
    internal bool Contains (Guid id)
    {
      return (Id.Equals (id));
    }

    internal bool Contains (TFactoryListItemInfo alias)
    {
      return (alias.NotNull () ? Id.Equals (alias.Id) : false);
    }

    internal void CopyFrom (TFactoryListItemInfo alias)
    {
      if (alias.NotNull ()) {
        ModelItem.CopyFrom (alias.ModelItem);
        IsChecked = alias.IsChecked;
      }
    }
    #endregion

    #region Static
    public static TFactoryListItemInfo Create (TComponentModelItem item) => new TFactoryListItemInfo (item);
    public static TFactoryListItemInfo Create (TComponentModelItem item, bool isChecked) => new TFactoryListItemInfo (item,isChecked);
    public static TFactoryListItemInfo CreateDefault => new TFactoryListItemInfo ();
    #endregion
  };
  //---------------------------//

}  // namespace
