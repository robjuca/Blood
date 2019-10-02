/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;
//---------------------------//

namespace Shared.Resources
{
  //----- TResource
  public static class TResource
  {
    #region Data
    public enum TModule
    {
      Shell,
      Collection,
      Factory,
      Services,
    };

    public enum TStyleMode
    {
      Horizontal,
      Vertical,
      None,
    };
    #endregion
  };
  //---------------------------//

    //----- TAlertsModel
  public class TAlertsModel : NotificationObject
  {
    #region Data
    public enum TKind
    {
      None,
      Primary,
      Secondary,
      Success,
      Danger,
      Warning,
      Info,
      Light,
      Dark,
    };
    #endregion

    public TKind Kind
    {
      get; 
      private set;
    }

    public string Caption
    {
      get;
      private set;
    }

    public string Message
    {
      get;
      private set;
    }

    public bool IsOpen
    {
      get;
      private set;
    }

    #region Constructor
    TAlertsModel ()
    {
      Kind = TKind.None;
      Caption = string.Empty;
      Message = string.Empty;
      IsOpen = false;
    }
    #endregion

    #region members
    public void Select (bool isOpen)
    {
      IsOpen = isOpen;
    }

    public void Select (TAlertsModel.TKind kind)
    {
      Kind = kind;
    }

    public void Select (string caption, string message)
    {
      Caption = caption;
      Message = message;
    }

    public void Refresh ()
    {
      RaisePropertyChanged (typeof (TAlertsModel).Name);
    }

    public void CopyFrom (TAlertsModel alias)
    {
      if (alias != null) {
        Kind = alias.Kind;
        Caption = alias.Caption;
        Message = alias.Message;
        IsOpen = alias.IsOpen;
      }
    } 
    #endregion

    #region Static
    public static TAlertsModel CreateDefault => new TAlertsModel (); 
    #endregion
  };
  //---------------------------//

}  // namespace