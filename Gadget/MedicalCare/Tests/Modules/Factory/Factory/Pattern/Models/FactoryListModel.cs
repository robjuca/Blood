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
    public ObservableCollection<TGadgetTestInfo> TestItemsSource
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
      TestItemsSource = new ObservableCollection<TGadgetTestInfo> ();
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

              if (modelAction.ComponentStatusModel.Busy.IsFalse ()) {
                TestItemsSource.Add (TGadgetTestInfo.Create (gadget));

                action.IdCollection.Add (gadget.Id); // for future uodate
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

    internal void SelectMany (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.IdCollection
      // action.CollectionAction.EntityCollection[id]

      action.ThrowNull ();

      foreach (var id in action.IdCollection) {
        foreach (var item in TestItemsSource) {
          if (item.Contains (id)) {
            if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
              var entityAction = action.CollectionAction.EntityCollection [id];
              item.UpdateContents (entityAction);
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
    readonly Collection<TComponentModelItem>                    m_TestCheckedItems;
    #endregion
  };
  //---------------------------//

  //----- TGadgetTestInfo
  public class TGadgetTestInfo
  {
    #region Property
    public string Category
    {
      get
      {
        return (Gadget.RequestCategory().ToString ());
      }
    }

    public string Name
    {
      get
      {
        return (Gadget.Test);
      }
    }

    public Collection<string> ContentNames
    {
      get
      {
        var names = new Collection<string> ();
        Gadget.RequestContentNames (names, useSeparator: true);

        return (names);
      }
    }

    public Server.Models.Component.GadgetTest Gadget
    {
      get; 
    }
    #endregion

    #region Constructor
    TGadgetTestInfo (Server.Models.Component.GadgetTest gadget)
      : this ()
    {
      if (gadget.NotNull ()) {
        Gadget.CopyFrom (gadget.Clone ());
      }
    }

    TGadgetTestInfo ()
    {
      Gadget = Server.Models.Component.GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    public bool Contains (Guid id)
    {
      return (Gadget.Id.Equals (id));
    }

    public void UpdateContents (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        Gadget.UpdateContents (action);
      }
    } 
    #endregion

    #region Static
    public static TGadgetTestInfo Create (Server.Models.Component.GadgetTest gadget) => new TGadgetTestInfo (gadget); 
    #endregion
  };
  //---------------------------//

}  // namespace
