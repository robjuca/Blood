/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
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
    public ObservableCollection<GadgetRegistration> RegistrationItemsSource
    {
      get;
    }

    public ObservableCollection<GadgetResult> ItemsSource
    {
      get;
    }

    public int RegistrationCount
    {
      get
      {
        return (RegistrationItemsSource.Count);
      }
    }

    public string Count
    {
      get
      {
        return ($"[ {ItemsSource.Count} ]");
      }
    }

    public GadgetRegistration RegistrationCurrent
    {
      get
      {
        return (RegistrationSelectedIndex.Equals (-1) ? GadgetRegistration.CreateDefault : RegistrationItemsSource [RegistrationSelectedIndex]);
      }
    }

    public int RegistrationSelectedIndex
    {
      get; 
      set;
    }

    public GadgetResult Current
    {
      get
      {
        return (ResultSelectedIndex.Equals (-1) ? GadgetResult.CreateDefault : ItemsSource [ResultSelectedIndex]);
      }
    }

    public int ResultSelectedIndex
    {
      get; 
      set;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      RegistrationItemsSource = new ObservableCollection<GadgetRegistration> ();
      ItemsSource = new ObservableCollection<GadgetResult> ();

      RegistrationSelectedIndex = -1;
      ResultSelectedIndex = -1;

      FullCollection = new Collection<GadgetResult> ();
    }
    #endregion

    #region Members
    internal void SelectRegistration (Collection<TActionComponent> gadgets)
    {
      gadgets.ThrowNull ();

      RegistrationItemsSource.Clear ();

      foreach (var component in gadgets) {
        var gadget = component.Models.GadgetRegistrationModel;

        if (gadget.Enabled) {
          RegistrationItemsSource.Add (gadget);
        }
      }

      if (RegistrationItemsSource.Any ()) {
        RegistrationSelectedIndex = 0;
      }
    }

    internal void SelectResult (Collection<TActionComponent> gadgets, Collection<Guid> idcontents)
    {
      gadgets.ThrowNull ();
      idcontents.ThrowNull ();

      FullCollection.Clear ();
      idcontents.Clear ();

      foreach (var component in gadgets) {
        FullCollection.Add (component.Models.GadgetResultModel);
      }

      foreach (var gadget in FullCollection) {
        var idList = new Collection<Guid> ();
        gadget.RequestContent (idList);

        foreach (var id in idList) {
          idcontents.Add (id);
        }
      }
    }

    internal void SelectMany (Collection<TActionComponent> gadgets)
    {
      gadgets.ThrowNull ();

      var gadgetRegistrationList = new Collection<GadgetRegistration> ();
      var gadgetTestList = new Collection<GadgetTest> ();

      foreach (var component in gadgets) {
        if (component.IsCategory (TCategory.Registration)) {
          gadgetRegistrationList.Add (component.Models.GadgetRegistrationModel);
        }

        if (component.IsCategory (TCategory.Test)) {
          gadgetTestList.Add (component.Models.GadgetTestModel);
        }
      }

      foreach (var gadget in FullCollection) {
        foreach (var gadgetRegistration in gadgetRegistrationList) {
          gadget.UpdateContent (gadgetRegistration);
        }

        gadget.UpdateContent (gadgetTestList);
      }

      RegistrationChanged (RegistrationCurrent);
    }

    internal void RegistrationChanged (GadgetRegistration gadget)
    {
      if (gadget.NotNull ()) {
        ItemsSource.Clear ();

        foreach (var gadgetResult in FullCollection) {
          if (gadgetResult.IsRegistrationContent (gadget.Id)) {
            ItemsSource.Add (gadgetResult);
          }
        }

        if (ItemsSource.Any ()) {
          ResultSelectedIndex = 0;
        }
      }
    }
    #endregion

    #region Property
    Collection<GadgetResult> FullCollection
    {
      get;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
