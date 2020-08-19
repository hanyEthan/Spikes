using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XCore.MongoAudit.API.model;

namespace XCore.MongoSample.API.context
{
    public class CompanyContext : ICompanyContext
    {
        #region props.
        private MongoClient MongoClient;
        public IMongoCollection<Employe> EmployeCollection { get; set; }
        public IMongoCollection<Department> DepartmentCollection { get; set; }
        #endregion

        #region ctr.
        public CompanyContext()
        {
            Initialize();
        }

        public void Initialize()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configration = builder.Build();
            MongoClient = new MongoClient(configration["ConnectionString:DefaultConnection"]);
            var db = MongoClient.GetDatabase(configration["ConnectionString:DatabaseName"]);
            EmployeCollection = db.GetCollection<Employe>("Employe");
            DepartmentCollection = db.GetCollection<Department>("Department");
        }
        #endregion

    }
}
