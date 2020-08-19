using MongoDB.Driver;
using XCore.MongoAudit.API.model;

namespace XCore.MongoSample.API.context
{
    public interface ICompanyContext
    {
      void  Initialize();
      IMongoCollection<Employe> EmployeCollection { get; set; }
      IMongoCollection<Department> DepartmentCollection { get; set; }
    }
}