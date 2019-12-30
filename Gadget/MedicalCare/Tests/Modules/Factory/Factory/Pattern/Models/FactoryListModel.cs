/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

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

      m_TestCheckedItems = new Collection<TGadgetTestInfo> ();
      m_MaterialFullCollection = new Dictionary<string, Server.Models.Component.GadgetMaterial> ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      // Material
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Material)) {
        m_MaterialFullCollection.Clear ();

        foreach (var gadget in action.CollectionAction.GadgetMaterialCollection) {
          if (gadget.Enabled) {
            m_MaterialFullCollection.Add (gadget.Material, gadget);
          }
        }
      }

      // Test
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Test)) {
        TestItemsSource.Clear ();

        foreach (var gadget in action.CollectionAction.GadgetTestCollection) {
          if (gadget.Enabled) {
            if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
              var modelAction = action.CollectionAction.ModelCollection [gadget.Id];

              if (modelAction.ComponentStatusModel.Busy.IsFalse ()) {
                if (m_MaterialFullCollection.ContainsKey (gadget.Material)) {
                  TestItemsSource.Add (TGadgetTestInfo.Create (gadget, m_MaterialFullCollection [gadget.Material]));

                  action.IdCollection.Add (gadget.Id); // for future uodate
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

    internal void TestSelected (TGadgetTestInfo item, bool isChecked)
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
    readonly Collection<TGadgetTestInfo>                                            m_TestCheckedItems;
    readonly Dictionary<string, Server.Models.Component.GadgetMaterial>             m_MaterialFullCollection;
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
        return (ContentCategory.ToString ());
      }
    }

    public Server.Models.Infrastructure.TCategory ContentCategory
    {
      get
      {
        return (GadgetTest.RequestCategory ());
      }
    }

    public string Name
    {
      get
      {
        return (GadgetTest.Test);
      }
    }

    public Collection<string> ContentNames
    {
      get
      {
        var names = new Collection<string> ();
        GadgetTest.RequestContentNames (names);

        return (names);
      }
    }

    public Server.Models.Component.GadgetMaterial GadgetMaterial
    {
      get;
    }

    public string GadgetMaterialName
    {
      get
      {
        return (GadgetMaterial.Material);
      }
    }

    public Collection<byte> GadgetMaterialImage
    {
      get
      {
        return (GadgetMaterial.Image);
      }
    }

    public Server.Models.Component.GadgetTest GadgetTest
    {
      get; 
    }

    public Visibility GadgetTestVisibility
    {
      get
      {
        return (ContentCategory.Equals (Server.Models.Infrastructure.TCategory.Test) ? Visibility.Visible : Visibility.Collapsed);
      }
    }

    public Visibility GadgetTargetVisibility
    {
      get
      {
        return (ContentCategory.Equals (Server.Models.Infrastructure.TCategory.Target) ? Visibility.Visible : Visibility.Collapsed);
      }
    }
    #endregion

    #region Constructor
    TGadgetTestInfo (Server.Models.Component.GadgetTest gadgetTest, Server.Models.Component.GadgetMaterial gadgetMaterial)
      : this ()
    {
      if (gadgetTest.NotNull () && gadgetMaterial.NotNull ()) {
        GadgetMaterial.CopyFrom (gadgetMaterial);
        GadgetTest.CopyFrom (gadgetTest.Clone ());
      }
    }

    TGadgetTestInfo ()
    {
      GadgetMaterial= Server.Models.Component.GadgetMaterial.CreateDefault;
      GadgetTest = Server.Models.Component.GadgetTest.CreateDefault;
    }
    #endregion

    #region Members
    public bool Contains (Guid id)
    {
      return (GadgetTest.Id.Equals (id));
    }

    public void UpdateContents (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        GadgetTest.UpdateContents (action);
      }
    } 
    #endregion

    #region Static
    public static TGadgetTestInfo Create (Server.Models.Component.GadgetTest gadgetTest, Server.Models.Component.GadgetMaterial gadgetMaterial) => new TGadgetTestInfo (gadgetTest, gadgetMaterial); 
    #endregion
  };
  //---------------------------//

}  // namespace
