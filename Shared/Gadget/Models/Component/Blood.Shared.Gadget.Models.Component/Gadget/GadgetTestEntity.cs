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
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetTest : TGadgetBase
  {
    #region Data
    //----- TContent
    class TContent
    {
      #region Property
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

      public bool HasContentTest
      {
        get
        {
          return (TestCollection.Any ());
        }
      }

      public bool HasContentTarget
      {
        get
        {
          return (TargetCollection.Any ());
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
        IdCollection = new Collection<Guid> ();
        TestCollection = new Collection<GadgetTest> ();
        TargetCollection = new Collection<GadgetTarget> ();
      }
      #endregion

      #region Members
      public bool Add (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (Contains (id).IsFalse ()) {
            IdCollection.Add (id);
            res = true;
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
            TestCollection.Add (gadget);

            res = true;
          }

          else {
            if (ContainsTest (gadget.Id).IsFalse ()) {
              TestCollection.Add (gadget);
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
            TargetCollection.Add (gadget);

            res = true;
          }

          else {
            if (ContainsTarget (gadget.Id).IsFalse ()) {
              TargetCollection.Add (gadget);
            }
          }
        }

        return (res);
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
            if (HasContentTest) {
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
      }

      public void Request (IList<GadgetTarget> collection)
      {
        if (collection.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            if (HasContentTarget) {
              collection.Clear ();

              var list = TargetCollection
                .OrderBy (p => p.GadgetName)
                .ToList ()
              ;

              foreach (var item in list) {
                collection.Add (item);
              }
            }
          }
        }
      }

      public void Request (IList<string> collection, bool useSeparator, bool full)
      {
        if (collection.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            var separator = useSeparator ? " - " : string.Empty;

            collection.Clear ();

            if (HasContentTest) {
              var testList = TestCollection
                .OrderBy (p => p.GadgetName)
                .ToList ()
              ;

              foreach (var gadgetTest in testList) {
                collection.Add (separator + gadgetTest.GadgetName);

                if (gadgetTest.HasContentTarget) {
                  var namesCollection = new Collection<string> ();

                  if (full) {
                    gadgetTest.RequestContentNamesFull (namesCollection);
                  }

                  else {
                    gadgetTest.RequestContentNames (namesCollection);
                  }

                  foreach (var itemName in namesCollection) {
                    collection.Add (separator + itemName);
                  }
                }
              }
            }

            if (HasContentTarget) {
              var list = TargetCollection
                .OrderBy (p => p.GadgetName)
                .ToList ()
              ;

              foreach (var item in list) {
                if (full) {
                  var str = $"{separator}{item.GadgetName}     :{item.Reference}" +
                            $"{Environment.NewLine}" +
                            $"    resultado: [ {item.Value} ]" +
                            $"{Environment.NewLine}";

                  collection.Add (str);
                }

                else {
                  collection.Add (separator + item.GadgetName);
                }
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

      public GadgetTest RequestTest (Guid id)
      {
        foreach (var gadget in TestCollection) {
          if (gadget.Id.Equals (id)) {
            return (gadget);
          }
        }

        return (null);
      }

      public GadgetTarget RequestTarget (Guid id)
      {
        foreach (var gadget in TargetCollection) {
          if (gadget.Id.Equals (id)) {
            return (gadget);
          }
        }

        return (null);
      }

      public Collection<string> ContentNames ()
      {
        var names = new Collection<string> ();

        if (HasContentTest) {
          var list = TestCollection
            .OrderBy (p => p.GadgetName)
            .ToList ()
          ;

          foreach (var item in list) {
            names.Add (" - " + item.GadgetName);
          }
        }

        if (HasContentTarget) {
          var list = TargetCollection
            .OrderBy (p => p.GadgetName)
            .ToList ()
          ;

          foreach (var item in list) {
            names.Add (" - " + item.GadgetName);
          }
        }

        return (names);
      }

      public void Update (Collection<GadgetTest> list)
      {
        if (list.NotNull ()) {
          if (IsEmpty.IsFalse ()) {
            if (HasContentTest) {
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

      public bool UpdateValue (GadgetTest gadgetContent)
      {
        bool res = false;

        if (gadgetContent.NotNull ()) {
          if (gadgetContent.ValidateId) {
            if (Contains (gadgetContent.Id)) {
              if (RequestTest (gadgetContent.Id) is GadgetTest gadgetItem) {
                if (gadgetItem.HasContentTarget) {
                  var list = new Collection<GadgetTarget> ();
                  gadgetItem.RequestContent (list);

                  foreach (var item in list) {
                    if (gadgetContent.RequestContentTarget (item.Id) is GadgetTarget gadget) {
                      item.ChangeValue (gadget.Value);
                      res = true;
                    }
                  }
                }

                if (gadgetItem.HasContentTest) {
                  var list = new Collection<GadgetTest> ();
                  gadgetItem.RequestContent (list);

                  foreach (var item in list) {
                    if (gadgetContent.RequestContentTest (item.Id) is GadgetTest gadget) {
                      item.ChangeValue (gadget.Value);
                      res = true;
                    }
                  }
                }
              }
            }
          }
        }

        return (res);
      }

      public bool UpdateFrom (GadgetTest gadgetContent)
      {
        bool res = false;

        if (gadgetContent.NotNull ()) {
          if (gadgetContent.ValidateId) {
            // Test
            if (HasContentTest && gadgetContent.HasContentTest) {
              if (Contains (gadgetContent.Id)) {
                if (RequestTest (gadgetContent.Id) is GadgetTest gadgetItem) {
                  gadgetItem.ChangeFrom (gadgetContent);

                  // Target
                  if (gadgetItem.HasContentTarget) {
                    var list = new Collection<GadgetTarget> ();
                    gadgetItem.RequestContent (list);

                    foreach (var item in list) {
                      if (gadgetContent.RequestContentTarget (item.Id) is GadgetTarget gadget) {
                        item.ChangeFrom (gadget);
                        res = true;
                      }
                    }
                  }

                  // Test
                  if (gadgetItem.HasContentTest) {
                    var list = new Collection<GadgetTest> ();
                    gadgetItem.RequestContent (list);

                    foreach (var item in list) {
                      if (gadgetContent.RequestContentTest (item.Id) is GadgetTest gadget) {
                        item.UpdateFrom (gadget);
                        res = true;
                      }
                    }
                  }
                }
              }
            }

            // Target
            if (HasContentTarget && gadgetContent.HasContentTarget) {
              if (gadgetContent.HasContent) {
                var gadgetTargetList = new Collection<GadgetTarget> ();
                gadgetContent.RequestContent (gadgetTargetList);

                foreach (var gadgetTarget in gadgetTargetList) {
                  if (RequestTarget (gadgetTarget.Id) is GadgetTarget gadgetItem) {
                    gadgetItem.ChangeFrom (gadgetTarget);
                    res = true;
                  }
                }
              }
            }
          }
        }

        return (res);
      }

      public void CopyFrom (TContent alias)
      {
        if (alias.NotNull ()) {
          Cleanup ();

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
        IdCollection = new Collection<Guid> ();
        TestCollection = new Collection<GadgetTest> ();
        TargetCollection = new Collection<GadgetTarget> ();
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

      bool RemoveFromCollection (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (Contains (id)) {
            IdCollection.Remove (id);

            RemoveTest (id);
            RemoveTarget (id);

            res = true;
          }
        }

        return (res);
      }

      bool RemoveTest (Guid id)
      {
        bool res = false;

        if (id.NotEmpty ()) {
          if (HasContentTest) {
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
          if (HasContentTarget) {
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

    public Collection<string> ContentNames
    {
      get
      {
        var names = new Collection<string> ();
        RequestContentNames (names);

        return (names);
      }
    }

    public Collection<string> ContentNamesFull
    {
      get
      {
        var names = new Collection<string> ();
        RequestContentNamesFull (names);

        return (names);
      }
    }

    public Collection<GadgetTest> ContentTestCollection
    {
      get
      {
        var contents = new Collection<GadgetTest> ();
        RequestContent (contents);

        return (contents);
      }
    }

    public Collection<GadgetTarget> ContentTargetCollection
    {
      get
      {
        var contents = new Collection<GadgetTarget> ();
        RequestContent (contents);

        return (contents);
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

    public bool HasContentTest 
    {
      get
      {
        return (Content.HasContentTest);
      }
    }

    public bool HasContentTarget 
    {
      get
      {
        return (Content.HasContentTarget);
      }
    }

    public bool CanRemove
    {
      get
      {
        return (Enabled.IsFalse () && Content.IsEmpty);
      }
    }

    public Visibility ContentTargetVisibility
    {
      get
      {
        return (HasContentTarget ? Visibility.Visible : Visibility.Collapsed);
      }
    }

    public Visibility ContentTestVisibility
    {
      get
      {
        return (HasContentTest ? Visibility.Visible : Visibility.Collapsed);
      }
    }
    #endregion

    #region Constructor
    public GadgetTest ()
      : base (TCategory.Test)
    {
      Content = TContent.CreateDefault;
    }

    public GadgetTest (GadgetTest alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public bool AddContentId (Guid id)
    {
      return (Content.Add (id));
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
        base.CopyFrom (alias);

        Content.CopyFrom (alias.Content.Clone ());
      }
    }

    public void Change (GadgetTest alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);

        Content.CopyFrom (alias.Content);
      }
    }

    public void RequestContent (IList<GadgetTest> collection)
    {
      if (collection.NotNull ()) {
        Content.Request (collection);
      }
    }

    public GadgetTest RequestContentTest (Guid id)
    {
      return (Content.RequestTest (id));
    }

    public GadgetTarget RequestContentTarget (Guid id)
    {
      return (Content.RequestTarget (id));
    }

    public void RequestContent (IList<GadgetTarget> collection)
    {
      if (collection.NotNull ()) {
        Content.Request (collection);
      }
    }

    public void RequestContentNames (IList<string> collection, bool useSeparator = true)
    {
      if (collection.NotNull ()) {
        Content.Request (collection, useSeparator, full: false);
      }
    }

    public void RequestContentNamesFull (IList<string> collection)
    {
      if (collection.NotNull ()) {
        Content.Request (collection, useSeparator: true, full: true);
      }
    }

    public void RequestContentId (IList<Guid> collection)
    {
      if (collection.NotNull ()) {
        Content.Request (collection);
      }
    }

    public void UpdateContents (Collection<GadgetTest> list)
    {
      if (list.NotNull ()) {
        Content.Update (list);
      }
    }

    public bool UpdateValue (GadgetTest gadget)
    {
      return (Content.UpdateValue (gadget));
    }

    public bool UpdateFrom (GadgetTest gadget)
    {
      if (gadget.NotNull ()) {
        if (Contains (gadget.Id)) {
          if (string.IsNullOrEmpty (Description)) {
            Description = gadget.Description;
          }

          if (HasMaterial.IsFalse ()) {
            MaterialId = gadget.MaterialId;
            Material = gadget.Material;
            SetImage (gadget.GetImage ());
          }
        }
      }

      return (Content.UpdateFrom (gadget));
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
  };
  //---------------------------//

}  // namespace