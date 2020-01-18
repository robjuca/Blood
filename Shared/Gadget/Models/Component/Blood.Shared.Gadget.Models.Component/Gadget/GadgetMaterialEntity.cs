/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetMaterial : TGadgetBase
  {
    #region Property
    public Collection<byte> Image
    {
      get; 
      private set;
    }

    public bool HasImage
    {
      get
      {
        return (Image.Any ());
      }
    }
    #endregion

    #region Constructor
    public GadgetMaterial ()
      : base ()
    {
      Image = new Collection<byte>();
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
        base.CopyFrom (alias);

        SetImage (alias.Image);
      }
    }

    public void Change (GadgetMaterial alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);

        SetImage (alias.Image);
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