/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.ObjectModel;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TSupportAction
  {
    #region Property
    public TSummaryInfo SummaryInfo
    {
      get;
    }

    public TSelectionInfo SelectionInfo
    {
      get;
    }

    public Collection<TSummaryInfo> SummaryCollection
    {
      get;
    }

    public Collection<TSelectionInfo> SelectionCollection
    {
      get;
    } 
    #endregion

    #region Constructor
    TSupportAction ()
    {
      SummaryInfo = TSummaryInfo.CreateDefault;
      SelectionInfo = TSelectionInfo.CreateDefault;

      SummaryCollection = new Collection<TSummaryInfo> ();
      SelectionCollection = new Collection<TSelectionInfo> ();
    }
    #endregion

    #region Static
    public static TSupportAction CreateDefault => new TSupportAction (); 
    #endregion
  };
  //---------------------------//

}  // namespace