/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<GadgetTest> ItemsSource
    {
      get;
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public GadgetTest Current
    {
      get;
    }

    public string Count
    {
      get
      {
        return ($"[ {ItemsSource.Count} ]");
      }
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      SelectedIndex = -1;

      ItemsSource = new ObservableCollection<GadgetTest> ();

      Current = GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Collection<TActionComponent> gadgets)
    {
      gadgets.ThrowNull ();

      ItemsSource.Clear ();

      foreach (var component in gadgets) {
        if (component.IsCategory (TCategory.Test)) {
          ItemsSource.Add (component.Models.GadgetTestModel);
        }
      }

      if (ItemsSource.Any ()) {
        SelectedIndex = 0;
        Current.CopyFrom (ItemsSource [0]);
      }
    }

    internal bool SelectionChanged (GadgetTest gadget)
    {
      if (gadget.IsNull ()) {
        return (false);
      }

      Current.CopyFrom (gadget);

      return (Current.ValidateId);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
