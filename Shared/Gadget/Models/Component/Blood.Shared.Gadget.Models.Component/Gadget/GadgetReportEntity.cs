/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Gadget.Models.Component
{
  public class GadgetReport : TGadgetBase
  {
    #region Property
    public DateTime Date
    {
      get; 
      private set;
    }
    #endregion

    #region Constructor
    public GadgetReport ()
      : base ()
    {
      Date = DateTime.Now;
    }

    public GadgetReport (GadgetReport alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (GadgetReport alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);

        Date = alias.Date;
      }
    }

    public void Change (GadgetReport alias)
    {
      if (alias.NotNull ()) {
        base.Change (alias);

        Date = alias.Date;
      }
    }

    public void SetDate (DateTime date)
    {
      if (date.NotNull ()) {
        Date = date;
      }
    }

    public GadgetReport Clone ()
    {
      var model = CreateDefault;
      model.CopyFrom (this);

      return (model);
    }
    #endregion

    #region Static
    public static GadgetReport CreateDefault => (new GadgetReport ());
    #endregion
  };
  //---------------------------//

}  // namespace