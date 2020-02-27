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
            var category = TCategoryType.FromValue (categoryValue);

            if (category.Equals (TCategory.Registration)) {
              Registration.Id = id;

              res = true;
            }

            if (category.Equals (TCategory.Test)) {
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

      internal void Update (GadgetRegistration gadget)
      {
        if (gadget.NotNull ()) {
          if (Contains (gadget.Id)) {
            IdCollection.Remove (gadget.Id);
            Add (gadget);
          }

          else {
            if (Registration.ValidateId.IsFalse ()) {
              Add (gadget); // registration must exist
            }
          }
        }
      }

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
    public int ContentCount
    {
      get
      {
        return (Content.Count);
      }
    }

    public bool CanEdit
    {
      get
      {
        return (IsEditEnabled);
      }
    }

    public bool CanRemove
    {
      get
      {
        return (IsRemoveEnabled && Content.IsEmpty);
      }
    }
    #endregion

    #region Constructor
    public GadgetResult ()
      : base ()
    {
      Content = TContent.CreateDefault;
    }

    public GadgetResult (GadgetResult alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void AddContent (GadgetRegistration gadget)
    {
      Content.Add (gadget);
    }

    public void AddContent (GadgetTest gadget)
    {
      Content.Add (gadget);
    }

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

    public void UpdateContent (GadgetRegistration gadget)
    {
      Content.Update (gadget);
    }

    public void UpdateContent (Collection<GadgetTest> list)
    {
      Content.Update (list);
    }

    public void CopyFrom (GadgetResult alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);

        Content.CopyFrom (alias.Content);
      }
    }

    public void Change (GadgetResult alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);
      }
    }

    public GadgetResult Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }

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