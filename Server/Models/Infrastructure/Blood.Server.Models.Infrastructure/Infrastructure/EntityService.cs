/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Threading.Tasks;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TEntityService<TDataContextType>
    where TDataContextType : IEntityDataContext
  {
    #region Property
    public TDataContextType DataContext
    {
      get; 
    }
    #endregion

    #region Constructor
    public TEntityService (TDataContextType dataContext)
    {
      DataContext = dataContext;
    }
    #endregion

    #region Interface
    public async Task<IEntityAction> OperationAsync (IEntityAction entityAction)
    {
      return (await DataContext.OperationAsync (entityAction).ConfigureAwait (false));
    }
    #endregion
  };
  //---------------------------//

}  // namespace