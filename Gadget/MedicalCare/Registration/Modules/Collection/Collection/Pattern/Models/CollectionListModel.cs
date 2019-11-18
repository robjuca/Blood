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
    public ObservableCollection<TComponentModelItem> MaterialItemsSource
    {
      get;
    }

    public ObservableCollection<TComponentModelItem> RegistrationItemsSource
    {
      get;
    }

    public string MaterialCount
    {
      get
      {
        return ($"[ {MaterialItemsSource.Count} ]");
      }
    }

    public string RegistrationCount
    {
      get
      {
        return ($"[ {RegistrationItemsSource.Count} ]");
      }
    }

    public TComponentModelItem MaterialSelected
    {
      get;
      set;
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
      MaterialItemsSource = new ObservableCollection<TComponentModelItem> ();
      RegistrationItemsSource = new ObservableCollection<TComponentModelItem> ();

      RegistrationSelectedIndex = -1;

      RegistrationCurrent = TComponentModelItem.CreateDefault;

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

      foreach (var gadget in action.CollectionAction.GadgetRegistrationCollection) {
        var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        modelAction.GadgetRegistrationModel.CopyFrom (gadget);

        action.ModelAction.CopyFrom (modelAction);

        Registrations.Add (TComponentModelItem.Create (action));
      }
    }

    internal void RefreshModel (Server.Models.Component.TEntityAction action)
    {
      // for gadget Material
      MaterialItemsSource.Clear ();

      foreach (var gadget in action.CollectionAction.GadgetMaterialCollection) {
        if (gadget.Enabled) {
          var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
          modelAction.GadgetMaterialModel.CopyFrom (gadget);

          action.ModelAction.CopyFrom (modelAction);

          MaterialItemsSource.Add (TComponentModelItem.Create (action));

          foreach (var item in Registrations) {
            // Node reverse here
            if (item.NodeModel.ParentId.Equals (gadget.Id)) {
              item.GadgetMaterialModel.CopyFrom (gadget);
            }
          }
        }
      }

      if (MaterialItemsSource.Count > 0) {
        MaterialSelected = MaterialItemsSource [0];
      }
    }

    // TODO: review
    internal void MaterialChanged (int selectdIndex)
    {
      RegistrationItemsSource.Clear ();

      if (selectdIndex.Equals (-1)) {
        RegistrationSelectedIndex = -1;
      }

      else {
        foreach (var item in Registrations) {
          //if (item.GadgetRegistrationModel.MaterialId.Equals (MaterialSelected.GadgetMaterialModel.Id)) {
          //  RegistrationItemsSource.Add (item);
          //}
        }

        if (RegistrationItemsSource.Count > 0) {
          RegistrationSelectedIndex = 0;
        }
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
