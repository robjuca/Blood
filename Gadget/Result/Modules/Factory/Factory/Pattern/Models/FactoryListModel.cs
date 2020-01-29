/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Server.Models.Infrastructure;
using Server.Models.Action;

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

    public ObservableCollection<TRegistrationInfo> RegistrationItemsSource
    {
      get;
    }

    public TComponentModelItem RegistrationCurrent
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
    public TFactoryListModel ()
    {
      TestItemsSource = new ObservableCollection<TGadgetTestInfo> ();
      RegistrationItemsSource = new ObservableCollection<TRegistrationInfo> ();

      RegistrationCurrent = TComponentModelItem.CreateDefault;

      m_TestCheckedItems = new Collection<TGadgetTestInfo> ();
      //m_MaterialFullCollection = new Dictionary<string, GadgetMaterial> ();
    }
    #endregion

    #region Members
    internal void Select (TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      // Material
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Material)) {
        //m_MaterialFullCollection.Clear ();

        //foreach (var gadget in action.CollectionAction.GadgetMaterialCollection) {
        //  if (gadget.Enabled) {
        //    m_MaterialFullCollection.Add (gadget.Material, gadget);
        //  }
        //}
      }

      // Test
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Test)) {
        TestItemsSource.Clear ();

        //foreach (var gadget in action.CollectionAction.GadgetTestCollection) {
        //  if (gadget.Enabled) {
        //    if (m_MaterialFullCollection.ContainsKey (gadget.Material)) {
        //      TestItemsSource.Add (TGadgetTestInfo.Create (gadget, m_MaterialFullCollection [gadget.Material]));

        //      action.IdCollection.Add (gadget.Id); // for future uodate
        //    }
        //  }
        //}
      }

      // Registration
      if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Registration)) {
        RegistrationItemsSource.Clear ();

        //var list = action.CollectionAction.GadgetRegistrationCollection
        //  .OrderBy (p => p.Name)
        //  .ToList ()
        //;

        //foreach (var gadget in list) {
        //  if (gadget.Enabled) {
        //    if (action.CollectionAction.ModelCollection.ContainsKey (gadget.Id)) {
        //      var modelAction = action.CollectionAction.ModelCollection [gadget.Id];
        //      modelAction.GadgetRegistrationModel.CopyFrom (gadget);

        //      action.ModelAction.CopyFrom (modelAction);

        //      var item = TComponentModelItem.Create (action);
        //      RegistrationItemsSource.Add (TRegistrationInfo.Create (item));
        //    }
        //  }
        //}
      }
    }

    internal void SelectMany (TEntityAction action)
    {
      // DATA IN:
      // action.IdCollection
      // action.CollectionAction.EntityCollection[id]

      action.ThrowNull ();

      foreach (var id in action.IdCollection) {
        foreach (var item in TestItemsSource) {
          //if (item.Contains (id)) {
          //  if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
          //    var entityAction = action.CollectionAction.EntityCollection [id];
          //    item.UpdateContents (entityAction);
          //  }
          //}
        }
      }
    }

    internal void RegistrationCurrentSelected (TRegistrationInfo item)
    {
      item.ThrowNull ();

      RegistrationCurrent.CopyFrom (item.ModelItem);
    }

    internal void Resultelected (TGadgetTestInfo item, bool isChecked)
    {
      item.ThrowNull ();

      if (isChecked) {
        m_TestCheckedItems.Add (item);
      }

      else {
        m_TestCheckedItems.Remove (item);
      }
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      var parentId = action.Id;
      var parentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

      //registration
      if (RegistrationCurrent.ValidateId) {
        //var componentRelation = ComponentRelation.CreateDefault;
        //componentRelation.ChildId = RegistrationCurrent.Id;
        //componentRelation.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (RegistrationCurrent.Category);
        //componentRelation.ParentId = parentId;
        //componentRelation.ParentCategory = parentCategory;

        //action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
      }

      //test
      foreach (var item in m_TestCheckedItems) {
        //if (item.Id.NotEquals (action.Id)) {
        //  //var componentRelation = ComponentRelation.CreateDefault;
        //  //componentRelation.ChildId = item.Id;
        //  //componentRelation.ChildCategory = item.CategoryValue;
        //  //componentRelation.ParentId = parentId;
        //  //componentRelation.ParentCategory = parentCategory;

        //  //action.CollectionAction.ComponentRelationCollection.Add (componentRelation);
        //}
      }
    }

    internal void EditEnter (TEntityAction action)
    {
      action.ThrowNull ();

      m_TestCheckedItems.Clear ();

      //var registration = GadgetRegistration.CreateDefault;
      var idList = new List<Guid> ();

      //var gadget = action.ModelAction.GadgetResultModel;
      //gadget.RequestContent (registration); // registration
      //gadget.RequestContent (idList); // content id (Test)

      // update Registration
      for (int index = 0; index < RegistrationItemsSource.Count; index++) {
        var registrationItem = RegistrationItemsSource [index];

        //if (registrationItem.Select (registration.Id)) {
        //  RegistrationCurrentSelected (registrationItem);
        //  break;
        //}
      }

      // update Test
      foreach (var id in idList) {
        foreach (var info in TestItemsSource) {
          info.Unselect ();

          //if (info.Id.Equals (id)) {
          //  m_TestCheckedItems.Add (info);
          //}
        }
      }

      foreach (var itemChecked in m_TestCheckedItems) {
        foreach (var item in TestItemsSource) {
          //item.Select (itemChecked.Id);
        }
      }
    }
    #endregion

    #region Fields
    readonly Collection<TGadgetTestInfo>                                            m_TestCheckedItems;
    //readonly Dictionary<string, GadgetMaterial>             m_MaterialFullCollection;
    #endregion
  };
  //---------------------------//

  //----- TRegistrationInfo
  public class TRegistrationInfo
  {
    #region Property
    public TComponentModelItem ModelItem
    {
      get;
    }

    public bool IsChecked
    {
      get; 
      set;
    }
    #endregion

    #region Constructor
    TRegistrationInfo (TComponentModelItem modelItem)
    {
      ModelItem = TComponentModelItem.CreateDefault;
      ModelItem.CopyFrom (modelItem);

      IsChecked = false;
    }
    #endregion

    #region Members
    internal bool Select (Guid id)
    {
      bool res = false;

      IsChecked = false;

      if (ModelItem.Id.Equals (id)) {
        IsChecked = true;
        res = true;
      }

      return (res);
    } 
    #endregion

    #region Static
    public static TRegistrationInfo Create (TComponentModelItem modeItem) => new TRegistrationInfo (modeItem); 
    #endregion
  };
  //---------------------------//

  //----- TGadgetTestInfo
  public class TGadgetTestInfo
  {
    #region Property
    public int CategoryValue
    {
      get
      {
        return (Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Test));
      }
    }

    //public string ContentCategoryString
    //{
    //  get
    //  {
    //    return (ContentCategory.ToString ());
    //  }
    //}

    //public Server.Models.Infrastructure.TCategory ContentCategory
    //{
    //  get
    //  {
    //    return (GadgetTest.RequestCategory ());
    //  }
    //}

    //public string Name
    //{
    //  get
    //  {
    //    return (GadgetTest.Test);
    //  }
    //}

    //public Guid Id
    //{
    //  get
    //  {
    //    return (GadgetTest.Id);
    //  }
    //}

    //public Collection<string> ContentNames
    //{
    //  get
    //  {
    //    var names = new Collection<string> ();
    //    GadgetTest.RequestContentNames (names);

    //    return (names);
    //  }
    //}

    //public GadgetMaterial GadgetMaterial
    //{
    //  get;
    //}

    //public string GadgetMaterialName
    //{
    //  get
    //  {
    //    return (GadgetMaterial.Material);
    //  }
    //}

    //public Collection<byte> GadgetMaterialImage
    //{
    //  get
    //  {
    //    return (GadgetMaterial.Image);
    //  }
    //}

    //public GadgetTest GadgetTest
    //{
    //  get; 
    //}

    //public Visibility GadgetTestVisibility
    //{
    //  get
    //  {
    //    return (ContentCategory.Equals (Server.Models.Infrastructure.TCategory.Test) ? Visibility.Visible : Visibility.Collapsed);
    //  }
    //}

    //public Visibility GadgetTargetVisibility
    //{
    //  get
    //  {
    //    return (ContentCategory.Equals (Server.Models.Infrastructure.TCategory.Target) ? Visibility.Visible : Visibility.Collapsed);
    //  }
    //}

    public bool IsChecked
    {
      get; 
      set;
    }
    #endregion

    #region Constructor
    //TGadgetTestInfo (GadgetTest gadgetTest, GadgetMaterial gadgetMaterial)
    //  : this ()
    //{
    //  if (gadgetTest.NotNull () && gadgetMaterial.NotNull ()) {
    //    GadgetMaterial.CopyFrom (gadgetMaterial);
    //    GadgetTest.CopyFrom (gadgetTest.Clone ());
    //  }
    //}

    //TGadgetTestInfo ()
    //{
    //  GadgetMaterial= GadgetMaterial.CreateDefault;
    //  GadgetTest = GadgetTest.CreateDefault;

    //  IsChecked = false;
    //}
    #endregion

    #region Members
    //internal bool Contains (Guid id)
    //{
    //  return (GadgetTest.Id.Equals (id));
    //}

    //internal void UpdateContents (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    GadgetTest.UpdateContents (action);
    //  }
    //}

    //internal bool Select (Guid id)
    //{
    //  bool res = false;

    //  if (Id.Equals (id)) {
    //    IsChecked = true;
    //    res = true;
    //  }

    //  return (res);
    //}

    internal void Unselect ()
    {
      IsChecked = false;
    }
    #endregion

    #region Static
    //public static TGadgetTestInfo Create (GadgetTest gadgetTest, GadgetMaterial gadgetMaterial) => new TGadgetTestInfo (gadgetTest, gadgetMaterial); 
    #endregion
  };
  //---------------------------//

}  // namespace
