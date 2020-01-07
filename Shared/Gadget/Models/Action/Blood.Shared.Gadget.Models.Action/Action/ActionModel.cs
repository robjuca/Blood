/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Linq;

using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  //----- TGadgetMaterialModel
  public sealed class TGadgetMaterialModel : TActionModelBase<GadgetMaterial>
  {
    #region Property
    public Guid Id
    {
      get
      {
        return (Model.Id);
      }
    }

    public bool ValidateId
    {
      get
      {
        return (Id.NotEmpty ());
      }
    }

    public bool ContainsImage
    {
      get
      {
        if (Model.Image.NotNull ()) {
          return (Model.Image.Any ());
        }

        return (false);
      }
    }

    public Visibility DisableVisibility
    {
      get
      {
        return (Model.Enabled ? Visibility.Collapsed : Visibility.Visible);
      }
    }
    #endregion

    #region Constructor
    TGadgetMaterialModel (string name, bool busy)
      : base (GadgetMaterial.CreateDefault)
    {
      Select (name, busy);
    }

    TGadgetMaterialModel ()
      : base (GadgetMaterial.CreateDefault)
    {
    }
    #endregion

    #region Members
    public void CopyFrom (TGadgetMaterialModel alias)
    {
      if (alias.NotNull ()) {
        Select (alias.Name, alias.Busy);

        Model.Id = alias.Model.Id;
        Model.Material = alias.Model.Material;
        Model.Description = alias.Model.Description;
        Model.ExternalLink = alias.Model.ExternalLink;
        Model.SetImage (alias.Model.Image);
        Model.Enabled = alias.Model.Enabled;
      }
    } 
    #endregion

    #region Static
    public static TGadgetMaterialModel Create (string name, bool busy) => new TGadgetMaterialModel (name, busy);

    public static TGadgetMaterialModel CreateDefault => new TGadgetMaterialModel ();
    #endregion
  };
  //---------------------------//

}  // namespace