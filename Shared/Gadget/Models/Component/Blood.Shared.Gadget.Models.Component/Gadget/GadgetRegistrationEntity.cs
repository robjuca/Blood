/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetRegistration : TGadgetBase
  {
    #region Property
    public Collection<byte> Image
    {
      get; 
      private set;
    }

    public DateTime Date
    {
      get; 
      private set;
    }
    #endregion

    #region Constructor
    public GadgetRegistration ()
      : base ()
    {
      Image = new Collection<byte>();
      Date = DateTime.Now;
    }

    public GadgetRegistration (GadgetRegistration alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    //public void CopyFrom (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    CopyFrom (action.ModelAction);
    //  }
    //}

    public void CopyFrom (GadgetRegistration alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);

        SetImage (alias.Image);
        Date = alias.Date;
      }
    }

    public void Change (GadgetRegistration alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);

        SetImage (alias.Image);
        Date = alias.Date;
      }
    }

    public byte [] GetImage ()
    {
      var image = new byte [Image.Count];
      Image.CopyTo (image, 0);

      return (image);
    }

    public void SetDate (DateTime date)
    {
      if (date.NotNull ()) {
        Date = date;
      }
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

    public GadgetRegistration Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }

    //public void RefreshModel (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Registration)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      action.ModelAction.GadgetRegistrationModel.CopyFrom (this); 

    //      // update model collection
    //      action.CollectionAction.GadgetRegistrationCollection.Clear ();

    //      foreach (var modelAction in action.CollectionAction.ModelCollection) {
    //        var gadget = GadgetRegistration.CreateDefault;
    //        gadget.CopyFrom (modelAction.Value);

    //        action.CollectionAction.GadgetRegistrationCollection.Add (gadget);
    //      }
    //    }
    //  }
    //}

    //public void Refresh (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Registration)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      action.ModelAction.GadgetRegistrationModel.CopyFrom (this);
    //    }
    //  }
    //}
    #endregion

    #region Static
    public static GadgetRegistration CreateDefault => (new GadgetRegistration ());
    #endregion
  };
  //---------------------------//

}  // namespace