/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> RegistrationSelectionItemsSource
    {
      get;
    }

    public ObservableCollection<TComponentModelItem> TestsItemsSource
    {
      get;
    }

    public int RegistrationSelectionItemsSourceCount
    {
      get
      {
        return (RegistrationSelectionItemsSource.Count);
      }
    }

    public string TestsCount
    {
      get
      {
        return ($"[ {TestsItemsSource.Count} ]");
      }
    }

    public int RegistrationSelectionSelectedIndex
    {
      get; 
      set;
    }

    public int TestsSelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem RegistrationSelectionCurrent
    {
      get;
    }

    public TComponentModelItem TestsCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      RegistrationSelectionItemsSource = new ObservableCollection<TComponentModelItem> ();
      TestsItemsSource = new ObservableCollection<TComponentModelItem> ();

      RegistrationSelectionSelectedIndex = -1;
      TestsSelectedIndex = -1;

      RegistrationSelectionCurrent = TComponentModelItem.CreateDefault;
      TestsCurrent = TComponentModelItem.CreateDefault;

      TestsFullCollection = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      // Registration
      if (action.CategoryType.IsCategory(Server.Models.Infrastructure.TCategory.Registration)) {
        RegistrationSelectionSelectedIndex = -1;
        RegistrationSelectionItemsSource.Clear ();

        foreach (var gadget in action.CollectionAction.GadgetRegistrationCollection) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetRegistrationModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          RegistrationSelectionItemsSource.Add (TComponentModelItem.Create (action));
        }

        if (RegistrationSelectionItemsSource.Any ()) {
          RegistrationSelectionSelectedIndex = 0;
        }
      }

      // Tests
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Tests)) {
        TestsFullCollection.Clear ();

        foreach (var gadget in action.CollectionAction.GadgetTestsCollection) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetTestsModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          TestsFullCollection.Add (TComponentModelItem.Create (action));
        }
      }
    }

    internal void RegistrationChanged (TComponentModelItem componentModelItem)
    {
      if (componentModelItem.NotNull ()) {
        if (componentModelItem.ValidateId) {
          RegistrationSelectionCurrent.CopyFrom (componentModelItem);

          TestsItemsSource.Clear ();

          foreach (var item in TestsFullCollection) {
            if (item.GadgetTestsModel.IsRegistrationContent (RegistrationSelectionCurrent.Id)) {
              TestsItemsSource.Add (item);
            }
          }

          if (TestsItemsSource.Any ()) {
            TestsSelectedIndex = 0;
          }
        }
      }
    }
    #endregion

    #region Property
    Collection<TComponentModelItem> TestsFullCollection
    {
      get;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
