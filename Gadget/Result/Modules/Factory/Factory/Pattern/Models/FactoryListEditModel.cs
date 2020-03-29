/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListEditModel
  {
    #region Property
    public ObservableCollection<TActionComponent> TestItemsSource
    {
      get;
    }

    public ObservableCollection<GadgetRegistration> RegistrationItemsSource
    {
      get;
    }

    public GadgetRegistration RegistrationCurrent
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

    public string TestCount
    {
      get
      {
        return ($"[ {TestItemsSource.Count} ]");
      }
    }

    public string TestCheckedCount
    {
      get
      {
        return ($"[ {m_TestCheckedItems.Count} ]");
      }
    }

    public bool TestListEnabled
    {
      get
      {
        return (RegistrationCurrent.ValidateId);
      }
    }
    #endregion

    #region Constructor
    public TFactoryListEditModel ()
    {
      TestItemsSource = new ObservableCollection<TActionComponent> ();
      RegistrationItemsSource = new ObservableCollection<GadgetRegistration> ();

      RegistrationCurrent = GadgetRegistration.CreateDefault;

      m_TestCheckedItems = new Collection<GadgetTest> ();
    }
    #endregion

    #region Members
    internal void SelectRegistration (Collection<TActionComponent> gadgets)
    {
      gadgets.ThrowNull ();

      RegistrationItemsSource.Clear ();

      foreach (var component in gadgets) {
        RegistrationItemsSource.Add (component.Models.GadgetRegistrationModel);
      }
    }

    internal void SelectTest (Collection<TActionComponent> gadgets)
    {
      gadgets.ThrowNull ();

      TestItemsSource.Clear ();

      foreach (var component in gadgets) {
        TestItemsSource.Add (component);
      }
    }

    internal void SelectTestMany (Collection<TActionComponent> gadgets)
    {
      gadgets.ThrowNull ();

      foreach (var component in gadgets) {
        if (component.IsCategory (TCategory.Test)) {
          var gadgetMaterial = component.Models.GadgetMaterialModel;
          var gadgetTest = component.Models.GadgetTestModel;

          var componentItem = RequestTest (gadgetTest.Id);

          if (componentItem.IsCategory (TCategory.Test)) {
            componentItem.Models.GadgetTestModel.Select (gadgetMaterial);
            componentItem.Models.GadgetTestModel.UpdateFrom (gadgetTest);
          }
        }
      }
    }

    internal void RequestTestIdCollection (Collection<Guid> idCollection)
    {
      idCollection.ThrowNull ();

      idCollection.Clear ();

      foreach (var component in TestItemsSource) {
        idCollection.Add (component.Models.GadgetTestModel.Id);
      }
    }

    internal void RequestTestCollection (Collection<TActionComponent> gadgets)
    {
      gadgets.ThrowNull ();

      gadgets.Clear ();

      foreach (var component in TestItemsSource) {
        gadgets.Add (component);
      }
    }

    internal void RegistrationCurrentSelected (GadgetRegistration gadget)
    {
      gadget.ThrowNull ();

      RegistrationCurrent.CopyFrom (gadget);
    }

    internal void Request (TActionComponent component)
    {
      component.ThrowNull ();

      //registration
      component.Models.GadgetResultModel.AddContent (RegistrationCurrent);

      //test
      foreach (var gadget in m_TestCheckedItems) {
        component.Models.GadgetResultModel.AddContent (gadget);
      }
    }

    internal void TestSelected (TActionComponent component, bool isChecked)
    {
      component.ThrowNull ();

      if (isChecked) {
        AddCheck (component.Models.GadgetTestModel);
      }

      else {
        RemoveCheck (component.Models.GadgetTestModel);
      }
    }

    internal void EditEnter (TActionComponent component)
    {
      component.ThrowNull ();

      Cleanup ();

      var registration = GadgetRegistration.CreateDefault;
      var idList = new List<Guid> ();

      var gadget = component.Models.GadgetResultModel;
      gadget.RequestContent (registration); // registration
      gadget.RequestContent (idList); // content id (Test)

      // update Registration
      for (int index = 0; index < RegistrationItemsSource.Count; index++) {
        var registrationItem = RegistrationItemsSource [index];

        if (registrationItem.Contains (registration.Id)) {
          registrationItem.IsChecked = true;

          RegistrationCurrentSelected (registrationItem);
          EnableAllRegistration (isEnabled: false);
          break;
        }
      }

      // update Test
      foreach (var id in idList) {
        foreach (var someComponent in TestItemsSource) {
          var gadgetTest = someComponent.Models.GadgetTestModel;

          if (gadgetTest.Contains (id)) {
            gadgetTest.IsChecked = true;
            m_TestCheckedItems.Add (gadgetTest);
          }
        }
      }
    }

    internal void Cleanup ()
    {
      EnableAllRegistration (isEnabled: true);
      UncheckAllRegistration ();

      RegistrationCurrent.CopyFrom (GadgetRegistration.CreateDefault);

      m_TestCheckedItems.Clear ();

      UncheckAllItemsSource ();
    }
    #endregion

    #region Fields
    readonly Collection<GadgetTest> m_TestCheckedItems;
    #endregion

    #region Support
    bool AddCheck (GadgetTest gadget)
    {
      foreach (var item in m_TestCheckedItems) {
        if (item.Contains (gadget.Id)) {
          return (false);
        }
      }

      m_TestCheckedItems.Add (gadget);

      return (true);
    }

    bool RemoveCheck (GadgetTest gadget)
    {
      foreach (var item in m_TestCheckedItems) {
        if (item.Contains (gadget.Id)) {
          m_TestCheckedItems.Remove (item);
          return (true);
        }
      }

      return (false);
    }

    void EnableAllRegistration (bool isEnabled)
    {
      for (int index = 0; index < RegistrationItemsSource.Count; index++) {
        var registrationItem = RegistrationItemsSource [index];
        registrationItem.Enabled = isEnabled;
      }
    }

    void UncheckAllRegistration ()
    {
      for (int index = 0; index < RegistrationItemsSource.Count; index++) {
        var registrationItem = RegistrationItemsSource [index];
        registrationItem.IsChecked = false;
      }
    }

    void UncheckAllItemsSource ()
    {
      foreach (var component in TestItemsSource) {
        component.Models.GadgetTestModel.IsChecked = false;
      }
    }

    TActionComponent RequestTest (Guid id)
    {
      foreach (var item in TestItemsSource) {
        if (item.IsCategory (TCategory.Test)) {
          if (item.Models.GadgetTestModel.Contains (id)) {
            return (item);
          }
        }
      }

      return (TActionComponent.CreateDefault);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
