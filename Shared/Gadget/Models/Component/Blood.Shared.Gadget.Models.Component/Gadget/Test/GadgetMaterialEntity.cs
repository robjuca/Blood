﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetMaterial
  {
    #region Property
    public Guid Id
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

    public Collection<byte> Image
    {
      get; 
      private set;
    }

    public bool Enabled
    {
      get; 
      set;
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
    public GadgetMaterial ()
    {
      Id = Guid.Empty;

      Material = string.Empty;
      Description = string.Empty;
      ExternalLink = string.Empty;
      Image = new Collection<byte>();
      Enabled = false;
    }

    public GadgetMaterial (GadgetMaterial alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (GadgetMaterial alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        SetImage (alias.Image);
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetMaterial alias)
    {
      if (alias.NotNull ()) {
        Material = alias.Material;
        Description = alias.Description;
        ExternalLink = alias.ExternalLink;
        SetImage (alias.Image);
        Enabled = alias.Enabled;
      }
    }

    public byte [] GetImage ()
    {
      var image = new byte [Image.Count];
      Image.CopyTo (image, 0);

      return (image);
    }

    public void SetImage (Collection<byte> image)
    {
      if (image.NotNull ()) {
        Image = new Collection<byte> (image);
      }
    }

    public void SetImage (byte [] image)
    {
      if (image.NotNull ()) {
        Image = new Collection<byte> (image);
      }
    }

    public GadgetMaterial Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }

    public bool Contains (Guid id)
    {
      return (Id.Equals (id));
    }

    //public void RefreshModel (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Material)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      action.ModelAction.GadgetMaterialModel.CopyFrom (this); 

    //      // update model collection
    //      action.CollectionAction.GadgetMaterialCollection.Clear ();

    //      foreach (var modelAction in action.CollectionAction.ModelCollection) {
    //        var gadget = GadgetMaterial.CreateDefault;
    //        gadget.CopyFrom (modelAction.Value);

    //        action.CollectionAction.GadgetMaterialCollection.Add (gadget);
    //      }
    //    }
    //  }
    //}

    //public void Refresh (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Material)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      action.ModelAction.GadgetMaterialModel.CopyFrom (this);
    //    }
    //  }
    //}
    #endregion

    #region Static
    public static GadgetMaterial CreateDefault => (new GadgetMaterial ());
    #endregion
  };
  //---------------------------//

}  // namespace