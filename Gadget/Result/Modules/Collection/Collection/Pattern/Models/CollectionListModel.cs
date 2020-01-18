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

    public ObservableCollection<TComponentModelItem> ResultItemsSource
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

    public string ResultCount
    {
      get
      {
        return ($"[ {ResultItemsSource.Count} ]");
      }
    }

    public int RegistrationSelectionSelectedIndex
    {
      get; 
      set;
    }

    public int ResultSelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem RegistrationSelectionCurrent
    {
      get;
    }

    public TComponentModelItem ResultCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      RegistrationSelectionItemsSource = new ObservableCollection<TComponentModelItem> ();
      ResultItemsSource = new ObservableCollection<TComponentModelItem> ();

      RegistrationSelectionSelectedIndex = -1;
      ResultSelectedIndex = -1;

      RegistrationSelectionCurrent = TComponentModelItem.CreateDefault;
      ResultCurrent = TComponentModelItem.CreateDefault;

      ResultFullCollection = new Collection<TComponentModelItem> ();
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

      // Result
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Result)) {
        ResultFullCollection.Clear ();

        foreach (var gadget in action.CollectionAction.GadgetResultCollection) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetResultModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          ResultFullCollection.Add (TComponentModelItem.Create (action));
        }
      }
    }

    internal void RegistrationChanged (TComponentModelItem componentModelItem)
    {
      if (componentModelItem.NotNull ()) {
        if (componentModelItem.ValidateId) {
          RegistrationSelectionCurrent.CopyFrom (componentModelItem);

          ResultItemsSource.Clear ();

          foreach (var item in ResultFullCollection) {
            if (item.GadgetResultModel.IsRegistrationContent (RegistrationSelectionCurrent.Id)) {
              ResultItemsSource.Add (item);
            }
          }

          if (ResultItemsSource.Any ()) {
            ResultSelectedIndex = 0;
          }
        }
      }
    }
    #endregion

    #region Property
    Collection<TComponentModelItem> ResultFullCollection
    {
      get;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
