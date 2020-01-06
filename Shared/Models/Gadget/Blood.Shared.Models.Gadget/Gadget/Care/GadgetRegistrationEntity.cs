/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Shared.Models.Gadget
{
  public class GadgetRegistration
  {
    #region Property
    public Guid Id
    {
      get; 
      set;
    }

    public string Name
    {
      get; 
      set;
    }

    public string Description
    {
      get; 
      set;
    }

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

    public bool Enabled
    {
      get; 
      set;
    }
    #endregion

    #region Constructor
    public GadgetRegistration ()
    {
      Id = Guid.Empty;

      Name = string.Empty;
      Description = string.Empty;
      Image = new Collection<byte>();
      Date = DateTime.Now;
      Enabled = false;
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
        Id = alias.Id;
        Name = alias.Name;
        Description = alias.Description;
        SetImage (alias.Image);
        Date = alias.Date;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetRegistration alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        Description = alias.Description;
        SetImage (alias.Image);
        Date = alias.Date;
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

    #region Support
    //void CopyFrom (TModelAction modelAction)
    //{
    //  Id = modelAction.ComponentInfoModel.Id;
    //  Name = modelAction.ExtensionTextModel.Text;
    //  Description = modelAction.ExtensionTextModel.Description;
    //  Enabled = modelAction.ComponentInfoModel.Enabled;
    //  SetImage (modelAction.ExtensionImageModel.Image);
    //  Date = modelAction.ExtensionTextModel.Date;
    //} 
    #endregion
  };
  //---------------------------//

}  // namespace