/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
//---------------------------//

namespace Shared.Gadget.Models.Action
{
  public class TActionModelBase<TGadgetModel>
  {
    #region Property
    public TGadgetModel Model
    {
      get;
    }

    public string Name
    {
      get;
      private set;
    }

    public bool Busy
    {
      get;
      private set;
    }

    public Visibility BusyVisibility
    {
      get
      {
        return (Busy ? Visibility.Visible : Visibility.Collapsed);
      }
    }
    #endregion

    #region Constructor
    protected TActionModelBase (TGadgetModel gadgetModel)
    {
      Model = gadgetModel;
      Name = string.Empty;
      Busy = false;
    }
    #endregion

    #region Members
    public void Select (string name, bool busy)
    {
      Name = name;
      Busy = busy;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace