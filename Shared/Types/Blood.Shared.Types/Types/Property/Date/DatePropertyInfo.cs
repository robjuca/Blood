/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public class TDatePropertyInfo : NotificationObject
  {
    #region Property
    public DateTime TheDate
    {
      get
      {
        return (m_TheDate);
      }

      set
      {
        m_TheDate = value;
        RaisePropertyChanged ("DateProperty");
      }
    }
    #endregion

    #region Constructor
    TDatePropertyInfo ()
    {
      TheDate = DateTime.Now;
    }
    #endregion

    #region Overrides
    public override string ToString () => (TheDate.ToString ("dd-MMM-yyyy"));
    #endregion

    #region Fields
    DateTime                                m_TheDate; 
    #endregion

    #region Static
    public static TDatePropertyInfo CreateDefault => new TDatePropertyInfo ();
    #endregion
  }
  //---------------------------//

}  // namespace