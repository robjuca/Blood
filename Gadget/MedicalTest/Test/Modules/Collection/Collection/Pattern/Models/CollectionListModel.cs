/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TModelItemInfo> ItemsSource
    {
      get; 
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public TComponentModelItem Current
    {
      get;
    }

    public string TestCount
    {
      get
      {
        return ($"[ {ItemsSource.Count} ]");
      }
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      SelectedIndex = -1;

      ItemsSource = new ObservableCollection<TModelItemInfo> ();
      Current = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      ItemsSource.Clear ();

      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        action.ModelAction.CopyFrom (modelAction.Value);
        
        var model = action.ModelAction.GadgetTestModel;
        model.CopyFrom (action); // set gadget model

        ItemsSource.Add (TModelItemInfo.Create (action));
      }

      if (ItemsSource.Count > 0) {
        SelectedIndex = 0;
      }
    }
    #endregion
  };
  //---------------------------//

  //----- TModelItemInfo
  public class TModelItemInfo
  {
    #region Property
    public TComponentModelItem ModelItem
    {
      get;
    }

    public Guid Id
    {
      get
      {
        return (ModelItem.Id);
      }
    }

    public string Name
    {
      get
      {
        return (ModelItem.Name);
      }
    }

    public int TargetCount
    {
      get
      {
        return (ModelItem.GadgetTestModel.ContentCount);
      }
    }

    public Server.Models.Infrastructure.TCategory RelationCategory
    {
      get
      {
        return (ModelItem.GadgetTestModel.RequestCategory ());
      }
    }

    public Visibility TestRelationVisibility
    {
      get
      {
        return (HasTarget ? RelationCategory.Equals (Server.Models.Infrastructure.TCategory.Test) ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed);
      }
    }

    public Visibility TargetRelationVisibility
    {
      get
      {
        return (HasTarget ? RelationCategory.Equals (Server.Models.Infrastructure.TCategory.Target) ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed);
      }
    }
    #endregion

    #region Constructor
    TModelItemInfo (Server.Models.Component.TEntityAction action)
      : this ()
    {
      if (action.NotNull ()) {
        ModelItem = TComponentModelItem.Create (action);
      }
    }

    TModelItemInfo ()
    {
      ModelItem = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region property
    public bool HasTarget
    {
      get
      {
        return (TargetCount.Equals (0).IsFalse ());
      }
    } 
    #endregion

    #region Static
    public static TModelItemInfo Create (Server.Models.Component.TEntityAction action) => new TModelItemInfo (action); 
    #endregion
  };
  //---------------------------//

}  // namespace
