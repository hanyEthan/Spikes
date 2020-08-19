using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Repositories
{
    public static class DepartmentsRepository
    {
        #region fields

        private static List<Department> departments;

        #endregion

        #region props

        private static List<Department> Departments
        {
            get
            {
                if ( departments == null || departments.Count == 0 ) Reload();
                return departments;
            }
            set
            {
                departments = value;
            }
        }

        #endregion

        #region helpers

        private static List<Department> GetDepartments()
        {
            var TamamDepartmentsResponse = TamamServiceBroker.OrganizationHandler.GetDepartments(SystemRequestContext.Instance);
            if (TamamDepartmentsResponse.Type != ResponseState.Success) return null;
            return TamamDepartmentsResponse.Result;
        }

        #endregion

        #region publics

        public static Department GetDepartment(string code)
        {
            var department = Departments.SingleOrDefault(g => g.Code == code);
            return department;
        }

        public static Guid? Translate(string code)
        {
            var department = Departments.SingleOrDefault(g => g.Code == code);
            return department == null ? (Guid?)null : department.Id;
        }

        public static void Reload()
        {
            Departments = GetDepartments();
        }

        #endregion
    }
}
