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
using XCore.Services.Personnel.Core.Contracts.Departments;
using XCore.Services.Personnel.DataLayer.Contracts;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Core.Handlers.Departments
{

    public class DepartmentHandler : IDepartmentHandler
    {
        #region props.
        private string Department_DataInclude_Basic { get; set; }
        private string Department_DataInclude_Search { get; set; }
        private string Department_DataInclude_Full { get; set; }
        private readonly IModelValidator<Department> DepartmentModelValidator;
        private readonly IPersonnelDataUnity dataHandler;
        #endregion
        #region cst.
        public DepartmentHandler(IPersonnelDataUnity DataHandler, IModelValidator<Department> DepartmentModelValidator)
        {
            this.dataHandler = DataHandler;
            this.DepartmentModelValidator = DepartmentModelValidator;

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
            try
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

                    var DepartmentModels = await dataHandler.Department.GetAsync(criteria, GetIncludes(criteria.SearchIncludes));
                    return context.Response.Set(ResponseState.Success, DepartmentModels);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<Department>> Create(Department Department, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Department>();
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

                    await dataHandler.Department.CreateAsync(Department);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, Department);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Department>(this.DepartmentModelValidator, Department, ValidationMode.Create),
                });
                return context.Response;

                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<Department>> Edit(Department Department, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Department>();
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

                    var existing = await dataHandler.Department.GetFirstAsync(x => x.Id == Department.Id || x.Code == Department.Code, null, this.Department_DataInclude_Full);
                    MapUpdate(existing, Department);

                    dataHandler.Department.Update(existing);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Department>(this.DepartmentModelValidator, Department, ValidationMode.Edit),
                });
                return context.Response;

                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> Delete(Department Department, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region validate.

                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, false);
                    }

                    #endregion
                    #region DL

                    await dataHandler.Department.DeleteAsync(Department);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Department>(this.DepartmentModelValidator, Department, ValidationMode.Delete),
                });
                return context.Response;

                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, false);
                    }

                    #endregion
                    #region validate.

                    var app = await dataHandler.Department.GetFirstAsync(x => x.Id == id);
                    var validationResponse = await this.DepartmentModelValidator.ValidateAsync(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    #region DL

                    await dataHandler.Department.DeleteAsync(app);
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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> IsUnique(Department Department, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region validate.

                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, false);
                    }

                    #endregion
                    #region DL

                    var isExisting = await dataHandler.Department.AnyAsync(x => (x.Name == Department.Name.Trim() ||
                                                                           x.NameCultured == Department.NameCultured.Trim())
                                                                           &&
                                                                           (x.Code != Department.Code)
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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> IsExists(DepartmentSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region validate.

                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, false);
                    }

                    #endregion
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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (dataHandler?.Initialized ?? false);
            isValid = isValid && (DepartmentModelValidator != null);
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Department : basic

                this.Department_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Department : Search

                this.Department_DataInclude_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Department : full

                this.Department_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "HeadDepartment",
                });

                #endregion

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }
        private bool MapUpdate(Department existing, Department updated)
        {
            if (updated == null || existing == null) return false;
            existing.DepartmentReferenceId = updated.DepartmentReferenceId;
            existing.HeadDepartmentId = updated.HeadDepartmentId;

            #region Common
            existing.Code = updated.Code;
            existing.IsActive = updated.IsActive;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            #endregion
            return true;
        }
        private string GetIncludes(SearchIncludesEnum searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludesEnum.Search:
                    return this.Department_DataInclude_Search;
                case SearchIncludesEnum.Full:
                    return this.Department_DataInclude_Full;
                case SearchIncludesEnum.Basic:
                default:
                    return this.Department_DataInclude_Basic;
            }
        }
        #endregion
    }
}
