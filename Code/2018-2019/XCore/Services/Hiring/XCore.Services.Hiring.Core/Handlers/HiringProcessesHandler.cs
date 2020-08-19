using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Handlers
{
    public class HiringProcessesHandler : IHiringProcessesHandler
    {
        #region props.

        private readonly IHiringDataUnity DataLayer;
        private readonly IModelValidator<HiringProcess> HiringProcessValidator;
        private string HiringProcess_IncludeProperties_Basic { get; set; }
        private string HiringProcess_IncludeProperties_Search { get; set; }
        private string HiringProcess_IncludeProperties_Full { get; set; }
        #endregion
        #region cst.

        public HiringProcessesHandler(IHiringDataUnity dataLayer, IModelValidator<HiringProcess> HiringProcessValidator)
        {
            this.DataLayer = dataLayer;
            this.HiringProcessValidator = HiringProcessValidator;
            this.Initialized = Initialize();
        }

        #endregion
        
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.HiringHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IHiringProcessesHandler

        public async Task<ExecutionResponse<HiringProcess>> Create(HiringProcess request, RequestContext requestContext)
        {
            var context = new ExecutionContext<HiringProcess>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await this.DataLayer.HiringProcesses.CreateAsync(request);
                await this.DataLayer.HiringProcesses.SaveAsync();

                return context.Response.Set(ResponseState.Success, request);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<HiringProcess>(this.HiringProcessValidator, request, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }       
        public async Task<ExecutionResponse<SearchResults<HiringProcess>>> Get(HiringProcessesSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<HiringProcess>>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var results = await this.DataLayer.HiringProcesses.Get(criteria, GetIncludes(criteria.SearchIncludes));
                return context.Response.Set(ResponseState.Success, results);

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

                Check();

                #region DL

                #region validate.

                var existing = await DataLayer.HiringProcesses.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.HiringProcessValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.HiringProcesses.SetActivationAsync(existing.Id, true);
                await DataLayer.SaveAsync();

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

                Check();

                #region DL

                #region validate.

                var existing = await DataLayer.HiringProcesses.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.HiringProcessValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.HiringProcesses.SetActivationAsync(existing.Id, false);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<HiringProcess>> Edit(HiringProcess hiringProcess, RequestContext requestContext)
        {
            var context = new ExecutionContext<HiringProcess>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var existing = await DataLayer.HiringProcesses.GetFirstAsync(x => x.Id == hiringProcess.Id || x.Code == hiringProcess.Code, null, this.HiringProcess_IncludeProperties_Full);
                MapUpdate(existing, hiringProcess);

                DataLayer.HiringProcesses.Update(existing);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<HiringProcess>(this.HiringProcessValidator, hiringProcess, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(HiringProcess hiringProcess, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await DataLayer.HiringProcesses.DeleteAsync(hiringProcess.Id);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<HiringProcess>(this.HiringProcessValidator, hiringProcess, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                Check();

                #region Logic

                #region validate.

                var hiringProcess = await DataLayer.HiringProcesses.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.HiringProcessValidator.ValidateAsync(hiringProcess, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.HiringProcesses.DeleteAsync(hiringProcess.Id);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var hiringProcess = await DataLayer.HiringProcesses.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.HiringProcessValidator.ValidateAsync(hiringProcess, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.HiringProcesses.DeleteAsync(hiringProcess.Id);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var venue = await DataLayer.HiringProcesses.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.HiringProcessValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.HiringProcesses.SetActivationAsync(venue.Id, true);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var venue = await DataLayer.HiringProcesses.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.HiringProcessValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.HiringProcesses.SetActivationAsync(venue.Id, false);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> IsExists(HiringProcessesSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.HiringProcesses.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(HiringProcess hiringProcess, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.HiringProcesses.AnyAsync(x => ((hiringProcess.Name != null && x.Name == hiringProcess.Name.Trim()) ||
                                                                                (hiringProcess.NameCultured != null && x.NameCultured == hiringProcess.NameCultured.Trim()))
                                                                                &&
                                                                                (x.Code != hiringProcess.Code)
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

        #endregion

        #region helpers.

        #region initialize
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.DataLayer?.Initialized ?? false);
            isValid = isValid && HiringProcessValidator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region HiringProcess : basic

                this.HiringProcess_IncludeProperties_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region HiringProcess : search

                this.HiringProcess_IncludeProperties_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region HiringProcess : full

                this.HiringProcess_IncludeProperties_Full = string.Join(",", new List<string>()
                {
                    "HiringSteps",
                    "Organization"
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
        #endregion
        #region mapUpdate
        private bool MapUpdate(HiringProcess existing, HiringProcess updated)
        {
            if (updated == null || existing == null) return false;

            #region Common

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;

            #endregion

            existing.OrganizationId = updated.OrganizationId;
            existing.HiringSteps = updated.HiringSteps;

            //MapUpdate(existing.HiringSteps, updated.HiringSteps);
            //existing.Organization = updated.Organization;

            return true;
        }
        #endregion
        #region includes
        private string GetIncludes(SearchIncludes searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludes.Search:
                    return this.HiringProcess_IncludeProperties_Search;
                case SearchIncludes.Full:
                    return this.HiringProcess_IncludeProperties_Full;
                case SearchIncludes.Basic:
                default:
                    return this.HiringProcess_IncludeProperties_Basic;
            }
        }

        #endregion

        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("handler is not initialized properly.");
            }
        }

        #endregion
    }
}
