using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ADS.Comon.Services.CentralizedCacheWCF
{
    [ServiceContract]
    public interface ICentralizedCacheService
    {
        [OperationContract]
        void CreateCacheCluster( Guid id );
        [OperationContract]
        void CreateCacheClusterWithParents( Guid id , List<Guid> parents );
        [OperationContract]
        bool Add( Guid clusterId , string key , object value );
        [OperationContract]
        object Get( Guid clusterId , string key );
        [OperationContract]
        bool Contains( Guid clusterId , string key );
        [OperationContract]
        bool Invalidate( Guid clusterId , bool invalidateGraph );
    }

}
