using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCore.MongoAudit.API.model;
using XCore.MongoSample.API.Repositories;

namespace XCore.MongoSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region props.
        private readonly IReposirory DepartmentRepository;
        #endregion
        #region ctr.
        public DepartmentController(IReposirory DepartmentRepository)
        {
            this.DepartmentRepository = DepartmentRepository;
        }
        #endregion
        [Route("Get")]
        [HttpGet]
        public IEnumerable<Department> Get()
        {
          
            
            var response=  this.DepartmentRepository.Get();
            return response;

        }
        [Route("Create")]
        [HttpPost]
    public Department create([FromBody]Department department)
        {

            var response = this.DepartmentRepository.create(department);
            return response;

        }
        [Route("Edit")]
        [HttpPut]
        public bool Edit(string id,Department department)
        {

            var response = this.DepartmentRepository.Update(id,department);
            return response;

        }
      
        [Route("Delete")]
        [HttpPost]
        public bool Delete([FromBody]string  id)
        {

            var response = this.DepartmentRepository.Remove(id);
            return response;

        }
    }
}