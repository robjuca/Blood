/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetResult : TGadgetBase
  {
    #region Data
    //----- TContent
    class TContent
    {
      #region Property
      internal bool IsEmpty
      {
        get
        {
          return (IdCollection.Count.Equals (0));
        }
      }

      internal int Count
      {
        get
        {
          return (IdCollection.Count);
        }
      }
      #endregion

      #region Constructor
      TContent ()
      {
        IdCollection = new Collection<Guid> ();
        TestCollection = new Collection<GadgetTest> ();

        Registration = GadgetRegistration.CreateDefault;
      }
      #endregion

      #region Members
      internal bool IsRegistration (Guid id)
      {
        return (Registration.Id.Equals (id));
      }

      internal void Add (GadgetRegistration gadget)
      {
        if (gadget.NotNull ()) {
          Registration.CopyFrom (gadget);
        }
      }

      internal bool Add (Guid id, int categoryValue)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (Contains (id).IsFalse ()) {
            var category = Server.Models.Infrastructure.TCategoryType.FromValue (categoryValue);

            if (category.Equals (Server.Models.Infrastructure.TCategory.Registration)) {
              Registration.Id = id;

              res = true;
            }

            if (category.Equals (Server.Models.Infrastructure.TCategory.Test)) {
              IdCollection.Add (id);

              res = true;
            }
          }
        }

        return (res);
      }

      internal bool Add (GadgetTest gadget)
      {
        var res = false;

        if (gadget.NotNull ()) {
          if (Contains (gadget.Id).IsFalse ()) {
            IdCollection.Add (gadget.Id);
            TestCollection.Add (gadget);

            res = true;
          }
        }

        return (res);
      }

      //internal void Add (Shared.Gadget.Models.Component.TEntityAction action)
      //{
      //  /*
      //  action.CollectionAction.EntityCollection (contents to add)
      //  action.CollectionAction.EntityCollection [id] (ModelAction content to add Test )
      //  */

      //  if (action.NotNull ()) {
      //    if (IsEmpty.IsFalse ()) {
      //      foreach (var id in IdCollection) {
      //        if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
      //          var contentAction = action.CollectionAction.EntityCollection [id];
      //          var gadget = contentAction.ModelAction.GadgetTestModel;

      //          if (ContainsTest (gadget.Id).IsFalse ()) {
      //            gadget.AddContent (contentAction);

      //            TestCollection.Add (gadget);
      //          }
      //        }
      //      }
      //    }
      //  }
      //}

      internal bool Remove (Guid id)
      {
        return (RemoveFromCollection (id));
      }

      internal bool Remove (GadgetTest gadget)
      {
        return (gadget.NotNull () ? Remove (gadget.Id) : false);
      }

      internal void Request (GadgetRegistration gadget)
      {
        if (gadget.NotNull ()) {
          gadget.CopyFrom (Registration);
        }
      }

      internal void Request (IList<Guid> list)
      {
        if (list.NotNull ()) {
          list.Clear ();

          foreach (var id in IdCollection) {
            list.Add (id);
          }
        }
      }

      internal void Request (IList<GadgetTest> collection)
      {
        if (collection.NotNull ()) {
          if (IsEmpty.IsFalse ()) {

            collection.Clear ();

            var list = TestCollection
              .OrderBy (p => p.GadgetName)
              .ToList ()
            ;

            foreach (var item in list) {
              collection.Add (item);
            }
          }
        }
      }

      internal void Request (IList<string> collection, bool useSeparator)
      {
        if (collection.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            var separator = useSeparator ? " - " : string.Empty;

            collection.Clear ();

            var list = TestCollection
              .OrderBy (p => p.GadgetName)
              .ToList ()
            ;

            foreach (var item in list) {
              collection.Add (separator + item.GadgetName);
            }
          }
        }
      }

      //internal void Update (TEntityAction action)
      //{
      //  /*
      //   action.CollectionAction.EntityCollection[id]{ModelAction}
      //  */

      //  if (action.NotNull ()) {
      //    if (IsEmpty.IsFalse ()) {
      //      foreach (var id in IdCollection) {
      //        if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
      //          var contentAction = action.CollectionAction.EntityCollection [id];

      //          var gadget = contentAction.ModelAction.GadgetTestModel;
      //          gadget.AddContent (contentAction);

      //          if (ContainsTest (gadget.Id).IsFalse ()) {
      //            TestCollection.Add (gadget);
      //          }
      //        }
      //      }
      //    }
      //  }
      //}

      internal void Update (Collection<GadgetTest> list)
      {
        if (list.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            foreach (var gadgetTest in list) {
              if (Contains (gadgetTest.Id)) {
                if (ContainsTest (gadgetTest.Id).IsFalse ()) {
                  TestCollection.Add (gadgetTest);
                }
              }
            }
          }
        }
      }

      internal void CopyFrom (TContent alias)
      {
        if (alias.NotNull ()) {
          Cleanup ();

          Registration.CopyFrom (alias.Registration);

          foreach (var id in alias.IdCollection) {
            IdCollection.Add (id);
          }

          foreach (var gadgetTest in alias.TestCollection) {
            TestCollection.Add (gadgetTest.Clone ());
          }
        }
      }

      internal TContent Clone ()
      {
        var model = CreateDefault;
        model.CopyFrom (this);

        return (model);
      }

      internal void Cleanup ()
      {
        IdCollection = new Collection<Guid> ();
        TestCollection = new Collection<GadgetTest> ();

        Registration = GadgetRegistration.CreateDefault;
      }
      #endregion

      #region Property
      Collection<Guid> IdCollection
      {
        get;
        set;
      }

      Collection<GadgetTest> TestCollection
      {
        get;
        set;
      }

      GadgetRegistration Registration
      {
        get;
        set;
      }
      #endregion

      #region Static
      internal static TContent CreateDefault => new TContent ();
      #endregion

      #region Support
      bool Contains (Guid id)
      {
        foreach (var targetId in IdCollection) {
          if (targetId.Equals (id)) {
            return (true);
          }
        }

        return (false);
      }

      bool ContainsTest (Guid id)
      {
        foreach (var gadget in TestCollection) {
          if (gadget.Id.Equals (id)) {
            return (true);
          }
        }

        return (false);
      }

      bool RemoveFromCollection (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (Contains (id)) {
            IdCollection.Remove (id);
            RemoveTest (id);

            res = true;
          }
        }

        return (res);
      }

      bool RemoveTest (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          foreach (var item in TestCollection) {
            if (item.Id.Equals (id)) {
              TestCollection.Remove (item);
              res = true;
              break;
            }
          }
        }

        return (res);
      }
      #endregion
    };
    #endregion

    #region Property
    public DateTime Date
    {
      get; 
      private set;
    }

    public int ContentCount
    {
      get
      {
        return (Content.Count);
      }
    }

    public bool CanRemove
    {
      get
      {
        return (Enabled.IsFalse () && Content.IsEmpty);
      }
    }
    #endregion

    #region Constructor
    public GadgetResult ()
      : base ()
    {
      Date = DateTime.Now;

      Content = TContent.CreateDefault;
    }

    public GadgetResult (GadgetResult alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public bool AddContentId (Guid id, int catergoryValue)
    {
      return (Content.Add (id, catergoryValue));
    }

    public void RequestContent (IList<Guid> list)
    {
      Content.Request (list);
    }

    public void RequestContent (Collection<GadgetTest> collection)
    {
      Content.Request (collection);
    }

    public void RequestContent (GadgetRegistration gadget)
    {
      Content.Request (gadget);
    }

    public void CopyFrom (GadgetResult alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);

        Date = alias.Date;

        Content.CopyFrom (alias.Content);
      }
    }

    public void Change (GadgetResult alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);
      }
    }

    public void SetDate (DateTime date)
    {
      if (date.NotNull ()) {
        Date = date;
      }
    }

    public GadgetResult Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }

    //public void RefreshModel (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Result)) {
    //      // update model action
    //      if (action.ModelAction.ComponentInfoModel.Id.NotEmpty ()) {
    //        CopyFrom (action.ModelAction); // my self
    //        action.ModelAction.GadgetResultModel.CopyFrom (this);
    //      }

    //      foreach (var modelAction in action.CollectionAction.ModelCollection) {
    //        if (modelAction.Value.ComponentInfoModel.Id.NotEmpty ()) {
    //          var gadget = GadgetResult.CreateDefault;
    //          gadget.CopyFrom (modelAction.Value);

    //          action.CollectionAction.GadgetResultCollection.Add (gadget);
    //        }
    //      }

    //      // relation
    //      if (action.CollectionAction.IsComponentOperation (TComponentOperation.TInternalOperation.Category)) {
    //        foreach (var gadget in action.CollectionAction.GadgetResultCollection) {
    //          foreach (var item in action.CollectionAction.ComponentOperation.ParentCategoryCollection) {
    //            foreach (var relation in item.Value) {
    //              if (relation.ParentId.Equals (gadget.Id)) {
    //                gadget.AddContentId (relation.ChildId, relation.ChildCategory);
    //              }
    //            }
    //          }
    //        }
    //      }
    //    }
    //  }
    //}

    //public void Refresh (TEntityAction action)
    //{
    //  //TODO: what for????
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Result)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      action.ModelAction.GadgetResultModel.CopyFrom (this);
    //    }
    //  }
    //}

    public bool IsRegistrationContent (Guid id)
    {
      return (Content.IsRegistration (id));
    }
    #endregion

    #region Property
    TContent Content
    {
      get;
    }
    #endregion

    #region Static
    public static GadgetResult CreateDefault => (new GadgetResult ());
    #endregion
  };
  //---------------------------//

}  // namespace