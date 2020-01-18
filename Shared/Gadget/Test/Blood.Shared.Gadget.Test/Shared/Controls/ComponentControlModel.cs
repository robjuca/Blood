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
    public TGadgetTestModel ControlModel
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
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ControlModel = TGadgetTestModel.CreateDefault;

      ComponentControlModels = new Collection<TComponentControlModel> ();
    }

    TComponentControlModel (TGadgetTestModel gadget)
      : this ()
    {
      if (gadget.NotNull ()) {
        ControlModel.CopyFrom (gadget);
      }
    }
    #endregion

    #region Members
    public void SelectModel (TGadgetTestModel gadget)
    {
      /*
       action.ModelAction.GadgetTestModel
       action.CollectionAction.EntityCollection
      */

      if (gadget.NotNull ()) {
        // Change only
        if (gadget.Id.IsEmpty ()) {
          ControlModel.Model.Test = gadget.Model.Test;
          ControlModel.Model.Description = gadget.Model.Description;
          ControlModel.Model.ExternalLink = gadget.Model.ExternalLink;
        }

        else {
          ComponentControlModels.Clear ();

          ControlModel.CopyFrom (gadget);

          if (ControlModel.RelationCategory.Equals (TCategory.Test)) {
            var contents = new Collection<GadgetTest> ();
            ControlModel.Model.RequestContent (contents);

            foreach (var gadgetTest in contents) {
              var controlModel = TComponentControlModel.CreateDefault;
              controlModel.ControlModel.CopyFrom (TGadgetTestModel.Create (gadgetTest));

              ComponentControlModels.Add (controlModel);
            }
          }
        }
      }
    }

    //public void SelectComponents (Server.Models.Component.TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.Param2 is Collection<TComponentControlModel> relations) {
    //      ComponentControlModels.Clear ();

    //      foreach (var item in relations) {
    //        ComponentControlModels.Add (item);
    //      }
    //    }
    //  }
    //}

    public Server.Models.Infrastructure.TCategory RequestCategory ()
    {
      return (ControlModel.Model.RequestCategory ());
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

    public void AddComponent (TGadgetTestComponent gadgetComponent)
    {
      if (gadgetComponent.NotNull ()) {
        switch (gadgetComponent.CurrentGadgetCategory) {
          case TCategory.Test: {
              ControlModel.Model.AddContent (gadgetComponent.GadgetTestModel);

              var contents = new Collection<GadgetTest> ();
              ControlModel.Model.RequestContent (contents);

              ComponentControlModels.Clear ();

              foreach (var gadgetContent in contents) {
                var componentControlModel = TComponentControlModel.CreateDefault;
                componentControlModel.ControlModel.CopyFrom (gadgetContent);

                if (componentControlModel.ControlModel.RelationCategory.Equals (TCategory.Test)) {
                  var internalContents = new Collection<GadgetTest> ();
                  componentControlModel.ControlModel.Model.RequestContent (internalContents);

                  foreach (var gadgetInternalContent in internalContents) {
                    var internalComponentControlModel = TComponentControlModel.CreateDefault;
                    internalComponentControlModel.ControlModel.CopyFrom (gadgetInternalContent);

                    componentControlModel.ComponentControlModels.Add (internalComponentControlModel);
                  }
                }

                ComponentControlModels.Add (componentControlModel);
              }
            }
            break;

          case TCategory.Target:
            ControlModel.Model.AddContent (gadgetComponent.GadgetTargetModel);
            break;
        }
      }
    }

    public void RemoveComponent (TGadgetTestComponent gadgetComponent)
    {
      if (gadgetComponent.NotNull ()) {
        if (gadgetComponent.Contains (ControlModelId)) {
          Cleanup ();
        }

        else {
          switch (gadgetComponent.CurrentGadgetCategory) {
            case TCategory.Test: {
                ControlModel.Model.RemoveContent (gadgetComponent.GadgetTestModel);

                if (HasComponentControlModels) {
                  foreach (var controlModelItem in ComponentControlModels) {
                    if (gadgetComponent.Contains (controlModelItem.ControlModelId)) {
                      ComponentControlModels.Remove (controlModelItem);
                      break;
                    }
                  }
                }
              }
              break;

            case TCategory.Target:
              ControlModel.Model.RemoveContent (gadgetComponent.GadgetTargetModel);
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

    public void Cleanup ()
    {
      ControlModel.CopyFrom (TGadgetTestModel.CreateDefault);

      ComponentControlModels.Clear ();
    }
    #endregion

    #region Static
    public static TComponentControlModel Create (TGadgetTestModel gadget) => new TComponentControlModel (gadget);

    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace