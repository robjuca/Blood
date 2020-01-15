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
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetTest
  {
    #region Data
    //----- TContent
    class TContent
    {
      #region Property
      public TCategory Category
      {
        get;
        private set;
      }

      public Collection<Guid> IdCollection
      {
        get; 
        private set;
      }

      public Collection<GadgetTest> TestCollection
      {
        get; 
        private set;
      }

      public Collection<GadgetTarget> TargetCollection
      {
        get; 
        private set;
      }

      public bool IsCategoryTest
      {
        get
        {
          return (Category.Equals (TCategory.Test));
        }
      }

      public bool IsCategoryTarget
      {
        get
        {
          return (Category.Equals (TCategory.Target));
        }
      }

      public bool IsEmpty
      {
        get
        {
          return (IdCollection.Count.Equals (0));
        }
      }

      public int Count
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
        Category = TCategory.None;

        IdCollection = new Collection<Guid> ();
        TestCollection = new Collection<GadgetTest> ();
        TargetCollection = new Collection<GadgetTarget> ();
      }
      #endregion

      #region Members
      public bool Add (Guid id, TCategory category)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (Contains (id).IsFalse ()) {
            if (ValidateCategory (category)) {
              IdCollection.Add (id);
              UpdateCategory (category);

              res = true;
            }
          }
        }

        return (res);
      }

      public bool Add (GadgetTest gadget)
      {
        var res = false;

        if (gadget.NotNull ()) {
          if (Contains (gadget.Id).IsFalse ()) {
            IdCollection.Add (gadget.Id);
            UpdateCategory (TCategory.Test);

            TestCollection.Add (gadget);

            res = true;
          }

          else {
            if (IsCategoryTest) {
              if (ContainsTest (gadget.Id).IsFalse ()) {
                TestCollection.Add (gadget);
              }
            }
          }
        }

        return (res);
      }

      public bool Add (GadgetTarget gadget)
      {
        var res = false;

        if (gadget.NotNull ()) {
          if (Contains (gadget.Id).IsFalse ()) {
            IdCollection.Add (gadget.Id);
            UpdateCategory (TCategory.Target);

            TargetCollection.Add (gadget);

            res = true;
          }

          else {
            if (IsCategoryTarget) {
              if (ContainsTarget (gadget.Id).IsFalse ()) {
                TargetCollection.Add (gadget);
              }
            }
          }
        }

        return (res);
      }

      //public void Add (Shared.Gadget.Models.Component.TEntityAction action)
      //{
      //  /*
      //  action.CollectionAction.EntityCollection (contents to add)
      //  action.CollectionAction.EntityCollection [id] (ModelAction content to add Test or Target)
      //  */

      //  if (action.NotNull ()) {
      //    if (IsEmpty.IsFalse ()) {
      //      foreach (var id in IdCollection) {
      //        if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
      //          var contentAction = action.CollectionAction.EntityCollection [id];

      //          if (IsCategoryTest) {
      //            var gadget = contentAction.ModelAction.GadgetTestModel;

      //            if (ContainsTest (gadget.Id).IsFalse ()) {
      //              gadget.AddContent (contentAction);

      //              TestCollection.Add (gadget);
      //            }
      //          }

      //          if (IsCategoryTarget) {
      //            var gadget = contentAction.ModelAction.GadgetTargetModel;

      //            if (ContainsTarget (gadget.Id).IsFalse ()) {
      //              TargetCollection.Add (gadget);
      //            }
      //          }
      //        }
      //      }
      //    }
      //  }
      //}

      public bool Remove (Guid id)
      {
        return (RemoveFromCollection (id));
      }

      public bool Remove (GadgetTest gadget)
      {
        return (gadget.NotNull () ? Remove (gadget.Id) : false);
      }

      public bool Remove (GadgetTarget gadget)
      {
        return (gadget.NotNull () ? Remove (gadget.Id) : false);
      }

      public void Request (IList<GadgetTest> collection)
      {
        if (collection.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            if (IsCategoryTest) {
              collection.Clear ();

              var list = TestCollection
                .OrderBy (p => p.Test)
                .ToList ()
              ;

              foreach (var item in list) {
                collection.Add (item);
              }
            }
          }
        }
      }

      public void Request (IList<GadgetTarget> collection)
      {
        if (collection.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            if (IsCategoryTarget) {
              collection.Clear ();

              var list = TargetCollection
                .OrderBy (p => p.Target)
                .ToList ()
              ;

              foreach (var item in list) {
                collection.Add (item);
              }
            }
          }
        }
      }

      public void Request (IList<string> collection, bool useSeparator)
      {
        if (collection.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            var separator = useSeparator ? " - " : string.Empty;

            collection.Clear ();

            if (IsCategoryTest) {
              var list = TestCollection
                .OrderBy (p => p.Test)
                .ToList ()
              ;

              foreach (var item in list) {
                collection.Add (separator + item.Test);
              }
            }

            if (IsCategoryTarget) {
              var list = TargetCollection
                .OrderBy (p => p.Target)
                .ToList ()
              ;

              foreach (var item in list) {
                collection.Add (separator + item.Target);
              }
            }
          }
        }
      }

      public void Request (IList<Guid> collection)
      {
        if (collection.NotNull ()) {
          collection.Clear ();

          if (IsEmpty.IsFalse ()) {
            foreach (var id in IdCollection) {
              collection.Add (id);
            }
          }
        }
      }

      //public void Update (TEntityAction action)
      //{
      //  /*
      //   action.CollectionAction.EntityCollection[id]{ModelAction}
      //  */

      //  if (action.NotNull ()) {
      //    if (IsEmpty.IsFalse ()) {
      //      foreach (var id in IdCollection) {
      //        if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
      //          var contentAction = action.CollectionAction.EntityCollection [id];

      //          if (IsCategoryTest) {
      //            var gadget = contentAction.ModelAction.GadgetTestModel;
      //            gadget.AddContent (contentAction);

      //            if (ContainsTest (gadget.Id).IsFalse ()) {
      //              TestCollection.Add (gadget);
      //            }
      //          }

      //          if (IsCategoryTarget) {
      //            var gadget = contentAction.ModelAction.GadgetTargetModel;

      //            if (ContainsTarget (gadget.Id).IsFalse ()) {
      //              TargetCollection.Add (gadget);
      //            }
      //          }
      //        }
      //      }
      //    }
      //  }
      //}

      public void Update (Collection<GadgetTest> list)
      {
        if (list.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            if (IsCategoryTest) {
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
      }

      public void CopyFrom (TContent alias)
      {
        if (alias.NotNull ()) {
          Cleanup ();

          Category = alias.Category;

          foreach (var id in alias.IdCollection) {
            IdCollection.Add (id);
          }

          foreach (var gadgetTest in alias.TestCollection) {
            TestCollection.Add (gadgetTest.Clone ());
          }

          foreach (var gadgetTarget in alias.TargetCollection) {
            TargetCollection.Add (gadgetTarget.Clone ());
          }
        }
      }

      public TContent Clone ()
      {
        var model = CreateDefault;
        model.CopyFrom (this);

        return (model);
      }

      public void Cleanup ()
      {
        Category = TCategory.None;

        IdCollection = new Collection<Guid> ();
        TestCollection = new Collection<GadgetTest> ();
        TargetCollection = new Collection<GadgetTarget> ();
      }
      #endregion

      #region Property
      bool IsCategoryEmpty
      {
        get
        {
          return (Category.Equals (TCategory.None));
        }
      } 
      #endregion

      #region Static
      public static TContent CreateDefault => new TContent ();
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

      bool ContainsTarget (Guid id)
      {
        foreach (var gadget in TargetCollection) {
          if (gadget.Id.Equals (id)) {
            return (true);
          }
        }

        return (false);
      }

      bool ValidateCategory (TCategory category)
      {
        if (IsCategoryEmpty) {
          return (category.Equals (TCategory.Test) || category.Equals (TCategory.Target));
        }

        return (category.Equals (Category));
      }

      void UpdateCategory (TCategory category)
      {
        if (IsCategoryEmpty) {
          Category = category;
        }
      }

      bool RemoveFromCollection (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (Contains (id)) {
            IdCollection.Remove (id);

            RemoveTest (id);
            RemoveTarget (id);

            if (IsEmpty) {
              Category = TCategory.None;
            }

            res = true;
          }
        }

        return (res);
      }

      bool RemoveTest (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (IsCategoryTest) {
            foreach (var item in TestCollection) {
              if (item.Id.Equals (id)) {
                TestCollection.Remove (item);
                res = true;
                break;
              }
            }
          }
        }

        return (res);
      }

      bool RemoveTarget (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (IsCategoryTarget) {
            foreach (var item in TargetCollection) {
              if (item.Id.Equals (id)) {
                TargetCollection.Remove (item);
                res = true;
                break;
              }
            }
          }
        }

        return (res);
      }
      #endregion
    };
    #endregion

    #region Property
    public Guid Id
    {
      get; 
      set;
    }

    public string Test
    {
      get; 
      set;
    }

    public string Material
    {
      get; 
      set;
    }

    public string Description
    {
      get; 
      set;
    }

    public string ExternalLink
    {
      get; 
      set;
    }

    public bool Enabled
    {
      get; 
      set;
    }

    public int ContentCount
    {
      get
      {
        return (Content.Count);
      }
    }

    public bool HasContent
    {
      get
      {
        return (IsContentEmpty.IsFalse ());
      }
    }

    public bool IsContentEmpty
    {
      get
      {
        return (ContentCount.Equals (0));
      }
    }

    public bool IsContentTarget
    {
      get
      {
        return (RequestCategory ().Equals (TCategory.Target));
      }
    }

    public bool IsContentTest
    {
      get
      {
        return (RequestCategory ().Equals (TCategory.Test));
      }
    }

    public bool CanRemove
    {
      get
      {
        return (Enabled.IsFalse () && Content.IsEmpty);
      }
    }

    public bool ValidateId
    {
      get
      {
        return (Id.NotEmpty ());
      }
    }
    #endregion

    #region Constructor
    public GadgetTest ()
    {
      Id = Guid.Empty;

      Test = string.Empty;
      Material = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      Enabled = false;
      
      Content = TContent.CreateDefault;
    }

    public GadgetTest (GadgetTest alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public bool AddContentId (Guid id, TCategory category)
    {
      return (Content.Add (id, category));
    }

    public bool AddContent (GadgetTest gadget)
    {
      return (Content.Add (gadget));
    }

    public bool AddContent (GadgetTarget gadget)
    {
      return (Content.Add (gadget));
    }

    public bool RemoveContentId (Guid id)
    {
      return (Content.Remove (id));
    }

    public bool RemoveContent (GadgetTest gadget)
    {
      return (Content.Remove (gadget));
    }

    public bool RemoveContent (GadgetTarget gadget)
    {
      return (Content.Remove (gadget));
    }

    public void CopyFrom (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;

        Test = alias.Test;
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;

        Content.CopyFrom (alias.Content.Clone ());
      }
    }

    public void Change (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Test = alias.Test;
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;

        Content.CopyFrom (alias.Content);
      }
    }
    
    public TCategory RequestCategory ()
    {
      return (Content.Category);
    }

    public void RequestContent (IList<GadgetTest> collection)
    {
      Content.Request (collection);
    }

    public void RequestContent (IList<GadgetTarget> collection)
    {
      Content.Request (collection);
    }

    public void RequestContentNames (IList<string> collection, bool useSeparator = true)
    {
      Content.Request (collection, useSeparator);
    }

    public void RequestContentId (IList<Guid> collection)
    {
      Content.Request (collection);
    }

    public void UpdateContents (Collection <GadgetTest> list)
    {
      Content.Update (list);
    }

    public GadgetTest Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }

    public bool Contains (Guid id)
    {
      return (Id.Equals (id));
    }
    #endregion

    #region Property
    TContent Content
    {
      get;
    } 
    #endregion

    #region Static
    public static GadgetTest CreateDefault => (new GadgetTest ());
    #endregion
  };
  //---------------------------//

}  // namespace