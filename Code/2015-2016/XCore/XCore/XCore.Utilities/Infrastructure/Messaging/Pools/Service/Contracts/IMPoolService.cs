using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Pools.Service.Models;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Service.Contracts
{
    [ServiceContract]
    public interface IMPoolService
    {
        #region AddMessage

        [OperationContract(AsyncPattern = true, IsOneWay = false)]
        Task<Response<int>> AddMessage(MPoolMessageDataContract message);

        #endregion
        #region GetMessages

        [OperationContract(AsyncPattern = true, IsOneWay = false)]
        Task<Response<List<MPoolMessageDataContract>>> GetMessages(MPoolCriteriaDataContract criteria);

        #endregion
        #region RestoreMessages

        [OperationContract(AsyncPattern = true, IsOneWay = false)]
        Task<Response<bool>> RestoreMessages(List<string> messages);

        #endregion
        #region DeleteMessages

        [OperationContract(AsyncPattern = true, IsOneWay = false)]
        Task<Response<bool>> DeleteMessages(List<string> messages);

        #endregion
    }
}
