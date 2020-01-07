/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetReport
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
    public GadgetReport ()
    {
      Id = Guid.Empty;

      Name = string.Empty;
      Description = string.Empty;
      Date = DateTime.Now;
      Enabled = false;
    }

    public GadgetReport (GadgetReport alias)
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

    public void CopyFrom (GadgetReport alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Name = alias.Name;
        Description = alias.Description;
        Date = alias.Date;
        Enabled = alias.Enabled;
      }
    }

    public void Change (GadgetReport alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        Description = alias.Description;
        Enabled = alias.Enabled;
      }
    }

    public GadgetReport Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }

    //public void RefreshModel (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Report)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      action.ModelAction.GadgetReportModel.CopyFrom (this); 

    //      // update model collection
    //      action.CollectionAction.GadgetMaterialCollection.Clear ();

    //      foreach (var modelAction in action.CollectionAction.ModelCollection) {
    //        var gadget = GadgetReport.CreateDefault;
    //        gadget.CopyFrom (modelAction.Value);

    //        action.CollectionAction.GadgetReportCollection.Add (gadget);
    //      }
    //    }
    //  }
    //}

    //public void Refresh (TEntityAction action)
    //{
    //  if (action.NotNull ()) {
    //    if (action.CategoryType.IsCategory (Infrastructure.TCategory.Report)) {
    //      // update model action
    //      CopyFrom (action.ModelAction); // my self
    //      action.ModelAction.GadgetReportModel.CopyFrom (this);
    //    }
    //  }
    //}
    #endregion

    #region Static
    public static GadgetReport CreateDefault => (new GadgetReport ());
    #endregion

    #region Support
    //void CopyFrom (TModelAction modelAction)
    //{
    //  Id = modelAction.ComponentInfoModel.Id;
    //  Name = modelAction.ExtensionTextModel.Text;
    //  Description = modelAction.ExtensionTextModel.Description;
    //  Enabled = modelAction.ComponentInfoModel.Enabled;
    //  Date = modelAction.ExtensionTextModel.Date;
    //} 
    #endregion
  };
  //---------------------------//

}  // namespace