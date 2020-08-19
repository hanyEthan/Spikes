using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;
using XCore.Services.Organizations.Core.Utilities;

namespace XCore.Services.Organizations.Core.Handlers
{
    public class DepartmentsHandler : IDepartmentHandler
    {
        #region props.
        private string Department_DataInclude_Basic { get; set; }
        private string Department_DataInclude_Full { get; set; }
        private string Department_DataInclude_Search { get; set; }
        private string Department_DataInclude_SearchRecursive { get; set; }




        private readonly IOrganizationDataUnity dataHandler;
        private readonly IModelValidator<Department> DepartmentValidator;
        #endregion
        #region cst.

        public DepartmentsHandler(IOrganizationDataUnity DataHandler,
                                  IModelValidator<Department> departmentValidators)
        {
            this.dataHandler = DataHandler;
            this.DepartmentValidator = departmentValidators;
            this.Initialized = Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.DepartmentHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IDepartmentHandler

        public async Task<ExecutionResponse<SearchResults<Department>>> Get(DepartmentSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Department>>();
            await context.Process(async () =>
            {
                #region Logic
                #region validate.

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, null);
                }

                #endregion
                #region DL

                var includes = criteria.IncludeRecursive
                                ? this.Department_DataInclude_SearchRecursive
                                : this.Department_DataInclude_Search;

                var Departments = await dataHandler.Department.GetAsync(criteria, includes);
                return context.Response.Set(ResponseState.Success, Departments);


                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Department>> Create(Department Department, RequestContext requestContext)
        {
            var context = new ExecutionContext<Department>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Department.CreateAsync(Department);
                await dataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, Department);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Department>(this.DepartmentValidator, Department, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Department>> Edit(Department Department, RequestContext requestContext)
        {
            var context = new ExecutionContext<Department>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                // var existing = await dataHandler.Department.GetFirstAsync(x => x.Id == Department.Id || x.Code == Department.Code, null, this.Department_DataInclude_Full);
                var existing = await dataHandler.Department.GetFirstAsync(x => x.Id == Department.Id || x.Code == Department.Code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, null);

                OrganizationsHelpers.MapUpdate(existing, Department);

                dataHandler.Department.Update(existing);
                await dataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, existing);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Department>(this.DepartmentValidator, Department, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(string DepartmentCode, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await dataHandler.Department.GetFirstAsync(x => x.Code == DepartmentCode);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);
                #region validate.
                var validationResponse = await this.DepartmentValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                await dataHandler.Department.DeleteAsync(existing);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await dataHandler.Department.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);
                #region validate.
                var validationResponse = await this.DepartmentValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                await dataHandler.Department.DeleteAsync(existing.Id);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,

            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Department department, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Department.DeleteAsync(department.Id);
                await dataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Department>(this.DepartmentValidator, department, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 #region validate.

                 var existing = await dataHandler.Department.GetFirstAsync(x => x.Code == code);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.DepartmentValidator.ValidateAsync(existing, ValidationMode.Activate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Department.SetActivationAsync(existing.Id, true);
                 await dataHandler.SaveAsync();

                 return context.Response.Set(ResponseState.Success, true);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,

             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 #region validate.

                 var existing = await dataHandler.Department.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.DepartmentValidator.ValidateAsync(existing, ValidationMode.Activate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Department.SetActivationAsync(id, true);
                 await dataHandler.SaveAsync();

                 #endregion

                 return context.Response.Set(ResponseState.Success, true);

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,

             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 #region validate.

                 var existing = await dataHandler.Department.GetFirstAsync(x => x.Code == code);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.DepartmentValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Department.SetActivationAsync(existing.Id, false);
                 await dataHandler.SaveAsync();

                 #endregion

                 return context.Response.Set(ResponseState.Success, true);

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 #region validate.

                 var existing = await dataHandler.Department.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.DepartmentValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Department.SetActivationAsync(id, false);
                 await dataHandler.SaveAsync();

                 #endregion

                 return context.Response.Set(ResponseState.Success, true);

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> IsUnique(Department department, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Department.AnyAsync(x => ((department.Name != null && x.Name == department.Name.Trim()) ||
                                                                              (department.NameCultured != null && x.NameCultured == department.NameCultured.Trim()))
                                                                              &&
                                                                              (x.Code != department.Code)
                                                                              &&
                                                                              (x.IsActive == true));

                 return context.Response.Set(ResponseState.Success, isExisting);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> IsExists(DepartmentSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Department.AnyAsync(criteria);
                 return context.Response.Set(ResponseState.Success, isExisting);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
             });
            return context.Response;

            #endregion
        }

        #endregion
        #region Helpers.

        private bool Initialize()
        {
            bool isValid = true;
            try
            {
                isValid = isValid && (dataHandler?.Initialized ?? false) && this.DepartmentValidator != null;
                isValid = isValid && InitializeIncludes();

                return isValid;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }
        private bool InitializeIncludes()
        {
            #region Department : basic

            this.Department_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region Department : search

            this.Department_DataInclude_Search = string.Join(",", new List<string>()
            {
                //"SubDepartments"
            });

            this.Department_DataInclude_SearchRecursive = string.Join(",", new List<string>()
                {
                    "SubDepartments.SubDepartments"
                });

            #endregion
            #region Department : full

            this.Department_DataInclude_Full = string.Join(",", new List<string>()
            {
                "Organization",
                "ParentDepartment",
                "SubDepartments",
            });

            #endregion

            return true;
        }

        #endregion
    }
}
