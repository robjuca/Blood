/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Gadget.Models.Action;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListItemInfo
  {
    #region Property
    public TGadgetTestModel GadgetModel
    {
      get;
    }

    public Guid Id
    {
      get
      {
        return (GadgetModel.Id);
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
        return (GadgetModel.Name);
      }
    }

    public int CategoryValue
    {
      get
      {
        return (Server.Models.Infrastructure.TCategoryType.ToValue (GadgetModel.Model.RequestCategory ()));
      }
    }
    #endregion

    #region Constructor
    TFactoryListItemInfo ()
    {
      GadgetModel = TGadgetTestModel.CreateDefault;
    }

    TFactoryListItemInfo (TGadgetTestModel gadget)
      : this ()
    {
      GadgetModel.CopyFrom (gadget);
    }

    TFactoryListItemInfo (TGadgetTestModel gadget, bool isChecked)
      : this ()
    {
      GadgetModel.CopyFrom (gadget);

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
        GadgetModel.CopyFrom (alias.GadgetModel);
        IsChecked = alias.IsChecked;
      }
    }
    #endregion

    #region Static
    public static TFactoryListItemInfo Create (TGadgetTestModel gadget) => new TFactoryListItemInfo (gadget);
    public static TFactoryListItemInfo Create (TGadgetTestModel gadget, bool isChecked) => new TFactoryListItemInfo (gadget, isChecked);
    public static TFactoryListItemInfo CreateDefault => new TFactoryListItemInfo ();
    #endregion
  };
  //---------------------------//

}  // namespace
