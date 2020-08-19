using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Caching.Custom.Contracts
{
    [ServiceContract]
    public interface ICacheService
    {
        [OperationContract] void AddCluster(Guid id);
        [OperationContract] void AddClusterWithParents(Guid id, List<Guid> parents);
        [OperationContract] bool Add(Guid clusterId, string key, object value);
        [OperationContract] object Get(Guid clusterId, string key);
        [OperationContract] bool Remove(Guid clusterId, string key);
        [OperationContract] bool Contains(Guid clusterId, string key);
        [OperationContract] bool Invalidate(Guid clusterId, bool invalidateGraph);
        [OperationContract] CacheServiceStats GetStatistics();
    }

    [DataContract]
    public class CacheServiceStats
    {
        [DataMember]
        public int TotalClusters { get; set; }
        [DataMember]
        public int TotalKeys { get; set; }
        [DataMember]
        public long MemoryUsage { get; set; }
        [DataMember]
        public ServiceHealth Health { get; set; }
        public enum ServiceHealth
        {
            Healthy,
            Unkown
        }
    }
}
