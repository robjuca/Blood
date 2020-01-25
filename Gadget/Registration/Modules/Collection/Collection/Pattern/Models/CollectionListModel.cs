/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Action;
using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<GadgetRegistration> ItemsSource
    {
      get;
    }

    public string RegistrationCount
    {
      get
      {
        return ($"[ {ItemsSource.Count} ]");
      }
    }

    public int SelectedIndex
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      ItemsSource = new ObservableCollection<GadgetRegistration> ();

      SelectedIndex = -1;

      // TODO: what for??
      //Registrations = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void Select (TEntityAction entityAction)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      entityAction.ThrowNull ();

      //Registrations.Clear ();
      ItemsSource.Clear ();

      var gadgets = new Collection<TActionComponent> ();
      TActionConverter.Collection (TCategory.Registration, gadgets, entityAction);

      foreach (var model in gadgets) {
        ItemsSource.Add (model.Models.GadgetRegistrationModel);
      }

      //foreach (var gadget in action.CollectionAction.GadgetRegistrationCollection) {
      //  if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
      //    var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
      //    modelAction.GadgetRegistrationModel.CopyFrom (gadget);

      //    action.ModelAction.CopyFrom (modelAction);

      //    var item = TComponentModelItem.Create (action);

      //    Registrations.Add (item);
      //    RegistrationItemsSource.Add (item);
      //  }
      //}

      SelectedIndex = ItemsSource.Any () ? 0 : -1;
    }

    internal bool SelectionChanged (TActionComponent component)
    {
      component.ThrowNull ();

      bool res = false;

      if (SelectedIndex.Equals (-1).IsFalse ()) {
        var gadget = ItemsSource [SelectedIndex];
        component.Models.GadgetRegistrationModel.CopyFrom (gadget);
        res = true;
      }

      return (res);
    }
    #endregion

    #region property
    //Collection<TComponentModelItem> Registrations
    //{
    //  get;
    //} 
    #endregion
  };
  //---------------------------//

}  // namespace
