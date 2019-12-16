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

namespace Server.Models.Component
{
  public partial class GadgetTest
  {
    #region Data
    //----- TContent
    class TContent
    {
      #region Property
      public Server.Models.Infrastructure.TCategory Category
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
          return (Category.Equals (Server.Models.Infrastructure.TCategory.Test));
        }
      }

      public bool IsCategoryTarget
      {
        get
        {
          return (Category.Equals (Server.Models.Infrastructure.TCategory.Target));
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
        Category = Server.Models.Infrastructure.TCategory.None;

        IdCollection = new Collection<Guid> ();
        TestCollection = new Collection<GadgetTest> ();
        TargetCollection = new Collection<GadgetTarget> ();
      }
      #endregion

      #region Members
      public bool Add (Guid id, Server.Models.Infrastructure.TCategory category)
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
            UpdateCategory (Server.Models.Infrastructure.TCategory.Test);

            TestCollection.Add (gadget);

            res = true;
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
            UpdateCategory (Server.Models.Infrastructure.TCategory.Target);

            TargetCollection.Add (gadget);

            res = true;
          }
        }

        return (res);
      }

      public void Add (Server.Models.Component.TEntityAction action)
      {
        /*
        action.CollectionAction.EntityCollection (contents to add)
        action.CollectionAction.EntityCollection [id] (ModelAction content to add Test or Target)
        */

        if (action.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            foreach (var id in IdCollection) {
              if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
                var contentAction = action.CollectionAction.EntityCollection [id];

                if (IsCategoryTest) {
                  var gadget = contentAction.ModelAction.GadgetTestModel;

                  if (ContainsTest (gadget.Id).IsFalse ()) {
                    gadget.AddContent (contentAction);

                    TestCollection.Add (gadget);
                  }
                }

                if (IsCategoryTarget) {
                  var gadget = contentAction.ModelAction.GadgetTargetModel;

                  if (ContainsTarget (gadget.Id).IsFalse ()) {
                    TargetCollection.Add (gadget);
                  }
                }
              }
            }
          }
        }
      }

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

      public void Update (TEntityAction action)
      {
        /*
         action.CollectionAction.EntityCollection[id]{ModelAction}
        */

        if (action.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            foreach (var id in IdCollection) {
              if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
                var contentAction = action.CollectionAction.EntityCollection [id];

                if (IsCategoryTest) {
                  var gadget = contentAction.ModelAction.GadgetTestModel;
                  gadget.AddContent (contentAction);

                  if (ContainsTest (gadget.Id).IsFalse ()) {
                    TestCollection.Add (gadget);
                  }
                }

                if (IsCategoryTarget) {
                  var gadget = contentAction.ModelAction.GadgetTargetModel;

                  if (ContainsTarget (gadget.Id).IsFalse ()) {
                    TargetCollection.Add (gadget);
                  }
                }
              }
            }
          }
        }
      }

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
        Category = Server.Models.Infrastructure.TCategory.None;

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
          return (Category.Equals (Server.Models.Infrastructure.TCategory.None));
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

      bool ValidateCategory (Server.Models.Infrastructure.TCategory category)
      {
        if (IsCategoryEmpty) {
          return (category.Equals (Server.Models.Infrastructure.TCategory.Test) || category.Equals (Server.Models.Infrastructure.TCategory.Target));
        }

        return (category.Equals (Category));
      }

      void UpdateCategory (Server.Models.Infrastructure.TCategory category)
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
              Category = Server.Models.Infrastructure.TCategory.None;
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
    public GadgetTest ()
    {
      Id = Guid.Empty;

      Test = string.Empty;
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
    public bool AddContentId (Guid id, Server.Models.Infrastructure.TCategory category)
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

    public void AddContent (TEntityAction action)
    {
      Content.Add (action);
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
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;

        Content.CopyFrom (alias.Content.Clone ());
      }
    }

    public void CopyFrom (TEntityAction action)
    {
      /*
       action.ModelAction
       action.CollectionAction.ComponentRelationCollection
      */

      if (action.NotNull ()) {
        Content.Cleanup ();

        CopyFrom (action.ModelAction);

        // update
        if (action.CollectionAction.ComponentRelationCollection.Count.Equals (0)) {
          foreach (var item in action.CollectionAction.ComponentOperation.ParentCategoryCollection) {
            foreach (var relation in item.Value) {
              action.CollectionAction.ComponentRelationCollection.Add (relation);
            }
          }
        }

        foreach (var item in action.CollectionAction.ComponentRelationCollection) {
          if (item.ParentId.IsEmpty ()) {
            Content.Add (item.ChildId, Server.Models.Infrastructure.TCategoryType.FromValue (item.ChildCategory));
          }

          else {
            if (item.ParentId.Equals (Id)) {
              Content.Add (item.ChildId, Server.Models.Infrastructure.TCategoryType.FromValue (item.ChildCategory));
            }
          }
        }
      }
    }

    public void Change (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        Test = alias.Test;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        Enabled = alias.Enabled;

        Content.CopyFrom (alias.Content);
      }
    }

    public void RefreshModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        if (action.CategoryType.IsCategory (Infrastructure.TCategory.Test)) {
          if (action.ModelAction.ComponentInfoModel.Id.IsEmpty ().IsFalse ()) {
            // update model action
            CopyFrom (action.ModelAction); // my self

            // content list
            foreach (var item in action.ComponentOperation.ParentIdCollection) {
              foreach (var relation in item.Value) {
                Content.Add (relation.ChildId, Server.Models.Infrastructure.TCategoryType.FromValue (relation.ChildCategory));
              }
            }

            // update
            action.ModelAction.GadgetTestModel.CopyFrom (this);

            action.CollectionAction.GadgetTestCollection.Clear ();

            // update model collection
            foreach (var modelAction in action.CollectionAction.ModelCollection) {
              action.ModelAction.CopyFrom (modelAction.Value);

              var gadget = GadgetTest.CreateDefault;
              gadget.CopyFrom (action);

              modelAction.Value.GadgetTestModel.CopyFrom (gadget); // update colection

              action.CollectionAction.GadgetTestCollection.Add (gadget);
            }

            // update contents
            Content.Update (action);
          }
        }
      }
    }

    public Server.Models.Infrastructure.TCategory RequestCategory ()
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

    // TODO: review what for??
    public void UpdateModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        if (action.CategoryType.IsCategory (Server.Models.Infrastructure.TCategory.Test)) {
          Content.Update (action);
        }
      }

      

      //if (relCategory.Equals (Server.Models.Infrastructure.TCategory.None) || relCategory.Equals (Server.Models.Infrastructure.TCategory.Test)) {
      //  if (ContentIdCollectionCount.Equals (0).IsFalse ()) {
      //    var targetId = ContentIdCollection [0];

      //    foreach (var item in action.ComponentOperation.ParentCategoryCollection) {
      //      foreach (var relation in item.Value) {
      //        if (relation.ParentId.Equals (Id) && relation.ChildId.Equals (targetId)) {
      //          ContentCategoryValue = relation.ChildCategory;

      //          if (IsRelationTest) {
      //            foreach (var gadget in action.CollectionAction.GadgetTestCollection) {
      //              if (ContainsContentId (gadget.Id)) {
      //                RelationsTest.Add (gadget);
      //              }
      //            }
      //          }

      //          return;
      //        }
      //      }
      //    }
      //  }
      //}
    }

    public void UpdateContents (TEntityAction action)
    {
      Content.Update (action);
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

    #region Support
    void CopyFrom (TModelAction modelAction)
    {
      Id = modelAction.ComponentInfoModel.Id;

      Test = modelAction.ExtensionTextModel.Text;
      Description = modelAction.ExtensionTextModel.Description;
      ExternalLink = modelAction.ExtensionTextModel.ExternalLink;
      Enabled = modelAction.ComponentInfoModel.Enabled;
    }
    #endregion
  };
  //---------------------------//

}  // namespace