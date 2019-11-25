/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> RegistrationItemsSource
    {
      get;
    }

    public string RegistrationCount
    {
      get
      {
        return ($"[ {RegistrationItemsSource.Count} ]");
      }
    }

    public TComponentModelItem RegistrationCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TFactoryListModel ()
    {
      RegistrationItemsSource = new ObservableCollection<TComponentModelItem> ();

      RegistrationCurrent = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      RegistrationItemsSource.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetRegistrationCollection) {
        if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetRegistrationModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          var item = TComponentModelItem.Create (action);

          RegistrationItemsSource.Add (item);
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
