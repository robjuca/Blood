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

      GadgetResultDictionary = new Dictionary<Guid, GadgetResult> ();
      GadgetMaterialDictionary = new Dictionary<Guid, GadgetMaterial> ();
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

    internal void SelectResult (Collection<TActionComponent> gadgets, Dictionary<Guid, Collection<Guid>> idcontents)
    {
      gadgets.ThrowNull ();
      idcontents.ThrowNull ();

      GadgetResultDictionary.Clear ();
      idcontents.Clear ();

      foreach (var component in gadgets) {
        var gadget = component.Models.GadgetResultModel;

        GadgetResultDictionary.Add (gadget.Id, gadget);
      }

      foreach (var item in GadgetResultDictionary) {
        var gadget = item.Value;

        var idList = new Collection<Guid> ();
        gadget.RequestContent (idList);

        if (gadget.HasRegistration) {
          idList.Add (gadget.RegistrationId);
        }

        idcontents.Add (gadget.Id, idList);
      }
    }

    internal void SelectMany (Guid id, Dictionary<Guid, Collection<TActionComponent>> gadgetDictionary)
    {
      if (id.NotEmpty () && gadgetDictionary.NotNull ()) {
        GadgetMaterialDictionary.Clear ();

        if (GadgetResultDictionary.ContainsKey (id)) {
          var gadgetResult = GadgetResultDictionary [id];

          var gadgetRegistrationList = new Collection<GadgetRegistration> ();
          var gadgetTestList = new Collection<GadgetTest> ();

          foreach (var item in gadgetDictionary) {
            var gadgetId = item.Key;
            var components = item.Value;

            foreach (var component in components) {
              if (component.IsCategory (TCategory.Registration)) {
                gadgetRegistrationList.Add (component.Models.GadgetRegistrationModel);
              }

              if (component.IsCategory (TCategory.Test)) {
                if (GadgetMaterialDictionary.ContainsKey (component.Models.GadgetMaterialModel.Id).IsFalse ()) {
                  GadgetMaterialDictionary.Add (component.Models.GadgetMaterialModel.Id, component.Models.GadgetMaterialModel);
                }

                component.Models.GadgetTestModel.Select (component.Models.GadgetMaterialModel); // update Material

                gadgetTestList.Add (component.Models.GadgetTestModel);
              }
            }
          }

          foreach (var gadgetRegistration in gadgetRegistrationList) {
            gadgetResult.UpdateContent (gadgetRegistration);
          }

          gadgetResult.UpdateContent (gadgetTestList);
        }

        RegistrationChanged (RegistrationCurrent);
      }
    }

    internal void RequestMaterial (Dictionary<Guid, GadgetMaterial> materialDictionary)
    {
      if (materialDictionary.NotNull ()) {
        materialDictionary.Clear ();

        foreach (var item in GadgetMaterialDictionary) {
          materialDictionary.Add (item.Key, item.Value);
        }
      }
    }

    internal void RegistrationChanged (GadgetRegistration gadget)
    {
      if (gadget.NotNull ()) {
        ItemsSource.Clear ();

        foreach (var item in GadgetResultDictionary) {
          var gadgetResult = item.Value;

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
    Dictionary<Guid, GadgetResult> GadgetResultDictionary
    {
      get;
    }

    Dictionary<Guid, GadgetMaterial> GadgetMaterialDictionary
    {
      get;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
