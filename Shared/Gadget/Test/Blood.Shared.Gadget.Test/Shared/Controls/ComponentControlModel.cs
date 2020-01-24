/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Infrastructure;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Shared.Gadget.Test
{
  public class TComponentControlModel
  {
    #region Property
    public GadgetTest ControlModel
    {
      get;
    }

    public Guid ControlModelId
    {
      get
      {
        return (ControlModel.Id);
      }
    }

    public Collection<TComponentControlModel> ComponentControlModels
    {
      get;
    }

    public bool HasComponentControlModels
    {
      get
      {
        return (ComponentControlModels.Any ());
      }
    }

    public Collection<byte> MaterialImage
    {
      get;
      private set;
    }

    public bool HasImage
    {
      get
      {
        return (MaterialImage.IsNull ().IsFalse ());
      }
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = GadgetTest.CreateDefault;

      ComponentControlModels = new Collection<TComponentControlModel> ();

      MaterialImage = null;
    }

    TComponentControlModel (GadgetTest gadget)
      : this ()
    {
      if (gadget.NotNull ()) {
        ControlModel.CopyFrom (gadget);
      }
    }
    #endregion

    #region Members
    public void SelectModel (GadgetTest gadget)
    {
      /*
       action.ModelAction.GadgetTestModel
       action.CollectionAction.EntityCollection
      */

      if (gadget.NotNull ()) {
        if (gadget.ValidateId) {
          ComponentControlModels.Clear ();

          ControlModel.CopyFrom (gadget);

          if (ControlModel.IsContentTest) {
            var contents = new Collection<GadgetTest> ();
            ControlModel.RequestContent (contents);

            foreach (var gadgetTest in contents) {
              var controlModel = TComponentControlModel.CreateDefault;
              controlModel.ControlModel.CopyFrom (gadgetTest.Clone ());

              ComponentControlModels.Add (controlModel);
            }
          }
        }

        // Change only
        else {
          ControlModel.GadgetName = gadget.GadgetName;
          ControlModel.Description = gadget.Description;
          ControlModel.ExternalLink = gadget.ExternalLink;
        }
      }
    }

    public void SelectModel (TActionComponent component)
    {
      if (component.NotNull ()) {
        if (component.IsCategory (TCategory.Test)) {
          var gadget = component.Models.GadgetTestModel;

          SelectModel (gadget);

          if (gadget.ValidateId) {
            if (HasComponentControlModels) {
              foreach (var controlModel in ComponentControlModels) {
                controlModel.SelectImage (component.Models.GadgetMaterialModel.GetImage ());
              }
            }

            else {
              SelectImage (component.Models.GadgetMaterialModel.GetImage ());
            }
          }
        }
      }
    }

    public TCategory RequestCategory ()
    {
      return (ControlModel.RequestCategory ());
    }

    //public void RequestComponents (Server.Models.Component.TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    var contents = new Collection<Server.Models.Component.GadgetTest> ();

    //    foreach (var item in ComponentControlModels) {
    //      contents.Add (item.ControlModel);
    //    }

    //    action.Param2 = contents;
    //  }
    //}

    public void AddComponent (TActionComponent component)
    {
      if (component.NotNull ()) {
        switch (component.Category) {
          case TCategory.Test: {
              ControlModel.AddContent (component.Models.GadgetTestModel);

              var contents = new Collection<GadgetTest> ();
              ControlModel.RequestContent (contents);

              ComponentControlModels.Clear ();

              foreach (var gadgetTest in contents) {
                var componentControlModel = TComponentControlModel.CreateDefault;
                componentControlModel.ControlModel.CopyFrom (gadgetTest);

                if (componentControlModel.ControlModel.IsContentTest) {
                  var internalContents = new Collection<GadgetTest> ();
                  componentControlModel.ControlModel.RequestContent (internalContents);

                  foreach (var gadgetTestInternal in internalContents) {
                    var internalComponentControlModel = TComponentControlModel.CreateDefault;
                    internalComponentControlModel.ControlModel.CopyFrom (gadgetTestInternal);
                    internalComponentControlModel.SelectImage (component.Models.GadgetMaterialModel.GetImage ());

                    componentControlModel.ComponentControlModels.Add (internalComponentControlModel);
                  }
                }

                if (componentControlModel.ControlModel.IsContentTarget) {
                  componentControlModel.SelectImage (component.Models.GadgetMaterialModel.GetImage ());
                }

                ComponentControlModels.Add (componentControlModel);
              }
            }
            break;

          case TCategory.Target: {
              ControlModel.CopyFrom (component.Models.GadgetTestModel);
              ControlModel.AddContent (component.Models.GadgetTargetModel);  // for sure

              SelectImage (component.Models.GadgetMaterialModel.GetImage ());
            }
            break;
        }
      }
    }

    public void RemoveComponent (TActionComponent component)
    {
      if (component.NotNull ()) {
        if (component.Models.GadgetTestModel.Contains (ControlModelId)) {
          Cleanup ();
        }

        else {
          switch (component.Category) {
            case TCategory.Test: {
                ControlModel.RemoveContent (component.Models.GadgetTestModel);

                if (HasComponentControlModels) {
                  foreach (var controlModelItem in ComponentControlModels) {
                    if (component.Models.GadgetTestModel.Contains (controlModelItem.ControlModelId)) {
                      ComponentControlModels.Remove (controlModelItem);
                      break;
                    }
                  }
                }
              }
              break;

            case TCategory.Target:
              ControlModel.RemoveContent (component.Models.GadgetTargetModel);
              break;
          }
        }
      }
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        Cleanup ();

        ControlModel.CopyFrom (alias.ControlModel);

        foreach (var item in alias.ComponentControlModels) {
          ComponentControlModels.Add (item);
        }
      }
    }

    internal void SelectImage (byte [] image)
    {
      if (image.NotNull ()) {
        MaterialImage = new Collection<byte> (image);
      }
    }

    public void Cleanup ()
    {
      ControlModel.CopyFrom (GadgetTest.CreateDefault);

      ComponentControlModels.Clear ();

      MaterialImage = null;
    }
    #endregion

    #region Static
    public static TComponentControlModel Create (GadgetTest gadget) => new TComponentControlModel (gadget);

    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace