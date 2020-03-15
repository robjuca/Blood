/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Message
{
  public class TArgumentInternal<T> : TArgumentTypesDefault
  {
    #region Property
    public T Item
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TArgumentInternal (T item)
    {
      Item = item;
    }
    #endregion
  };
  //---------------------------//

}  // namespace