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
    public ObservableCollection<TComponentModelItem> TestItemsSource
    {
      get;
    }

    public ObservableCollection<TComponentModelItem> RegistrationItemsSource
    {
      get;
    }

    public string TestCount
    {
      get
      {
        return ($"[ {TestItemsSource.Count} ]");
      }
    }

    public string RegistrationCount
    {
      get
      {
        return ($"[ {RegistrationItemsSource.Count} ]");
      }
    }

    public string TestCheckedCount
    {
      get
      {
        return ($"[ {m_TestCheckedItems.Count} ]");
      }
    }

    public TComponentModelItem RegistrationCurrent
    {
      get;
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
    public TFactoryListModel ()
    {
      TestItemsSource = new ObservableCollection<TComponentModelItem> ();
      RegistrationItemsSource = new ObservableCollection<TComponentModelItem> ();

      RegistrationCurrent = TComponentModelItem.CreateDefault;

      m_TestCheckedItems = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      // Test
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Test)) {
        TestItemsSource.Clear ();

        foreach (var gadget in action.CollectionAction.GadgetTestCollection) {
          if (gadget.Enabled) {
            if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
              var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
              modelAction.GadgetTestModel.CopyFrom (gadget);

              if (modelAction.ComponentStatusModel.Busy.IsFalse ()) {
                action.ModelAction.CopyFrom (modelAction);

                var item = TComponentModelItem.Create (action);

                if (item.GadgetTestModel.HasRelation) {
                  TestItemsSource.Add (item);
                }
              }
            }
          }
        }
      }

      // Registration
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Registration)) {
        RegistrationItemsSource.Clear ();

        foreach (var gadget in action.CollectionAction.GadgetRegistrationCollection) {
          if (gadget.Enabled) {
            if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
              var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
              modelAction.GadgetRegistrationModel.CopyFrom (gadget);

              action.ModelAction.CopyFrom (modelAction);

              var item = TComponentModelItem.Create (action);
              RegistrationItemsSource.Add (item);
            }
          }
        }
      }
    }

    internal void RegistrationCurrentSelected (TComponentModelItem item)
    {
      item.ThrowNull ();

      RegistrationCurrent.CopyFrom (item);
    }

    internal void TestSelected (TComponentModelItem item, bool isChecked)
    {
      item.ThrowNull ();

      if (isChecked) {
        m_TestCheckedItems.Add (item);
      }

      else {
        m_TestCheckedItems.Remove (item);
      }
    }
    #endregion

    #region Fields
    readonly Collection<TComponentModelItem>                              m_TestCheckedItems; 
    #endregion
  };
  //---------------------------//

}  // namespace
