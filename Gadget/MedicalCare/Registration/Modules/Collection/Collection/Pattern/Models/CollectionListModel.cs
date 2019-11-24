/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
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

    public int RegistrationSelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem RegistrationCurrent
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      RegistrationItemsSource = new ObservableCollection<TComponentModelItem> ();

      RegistrationSelectedIndex = -1;

      RegistrationCurrent = TComponentModelItem.CreateDefault;

      // TODO: what for??
      Registrations = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      Registrations.Clear ();
      RegistrationItemsSource.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetRegistrationCollection) {
        if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetRegistrationModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          var item = TComponentModelItem.Create (action);

          Registrations.Add (item);
          RegistrationItemsSource.Add (item);
        }
      }

      if (RegistrationItemsSource.Count.Equals (0).IsFalse ()) {
        RegistrationSelectedIndex = 0;
      }
    }
    #endregion

    #region property
    Collection<TComponentModelItem> Registrations
    {
      get;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
