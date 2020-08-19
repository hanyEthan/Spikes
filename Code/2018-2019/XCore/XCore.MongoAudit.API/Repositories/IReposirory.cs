using System.Collections.Generic;
using XCore.MongoAudit.API.model;

namespace XCore.MongoSample.API.Repositories
{
    public interface IReposirory
    {
        IEnumerable<Department> Get();
        Department create(Department department);
        bool Update(string id, Department department);
        bool Remove(string id);
    }
}