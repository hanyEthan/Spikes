using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using XCore.MongoAudit.API.model;
using XCore.MongoSample.API.context;

namespace XCore.MongoSample.API.Repositories
{
    public class DepartmentRepository : IReposirory
    {
        #region props.
        private readonly ICompanyContext CompanyContext;
        #endregion
        #region ctr.
        public DepartmentRepository(ICompanyContext CompanyContext)
        {
            this.CompanyContext = CompanyContext;
        }

     
        #endregion
        public IEnumerable<Department> Get() 
            {
                var result = this.CompanyContext.DepartmentCollection.AsQueryable<Department>();
               return result;
            }
        public Department Get(string id)
        {
            var result= this.CompanyContext.DepartmentCollection.Find<Department>(d => d.id == id).FirstOrDefault();
            return result;

        }
        public Department create(Department department)
        {
            this.CompanyContext.DepartmentCollection.InsertOne(department);
            return department;
        }

        public bool Update(string id, Department department)
        {
            try
            {
                this.CompanyContext.DepartmentCollection.ReplaceOne(d => d.id == id, department);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool Remove(string id)
        {
            try
            {
                this.CompanyContext.DepartmentCollection.DeleteOne(d => d.id == id);
                return true;

            }
            catch (Exception)
            {

                return false;
            }       
        }
              

      
    }
}
