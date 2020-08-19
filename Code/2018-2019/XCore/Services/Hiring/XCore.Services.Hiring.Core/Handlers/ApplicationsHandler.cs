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
    public class ApplicationsHandler : IApplicationsHandler
    {
        #region props.

        private readonly IHiringDataUnity DataLayer;
        private readonly IModelValidator<Application> ApplicationValidator;
        private string Application_IncludeProperties_Basic { get; set; }
        private string Application_IncludeProperties_Search { get; set; }
        private string Application_IncludeProperties_Full { get; set; }

        #endregion
        #region cst.

        public ApplicationsHandler(IHiringDataUnity dataLayer, IModelValidator<Application> ApplicationValidator)
        {
            this.DataLayer = dataLayer;
            this.ApplicationValidator = ApplicationValidator;
            this.Initialized = Initialize();
        }

        #endregion
        
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.HiringHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IApplicationsHandler

        public async Task<ExecutionResponse<Application>> Create(Application request, RequestContext requestContext)
        {
            var context = new ExecutionContext<Application>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await this.DataLayer.Applications.CreateAsync(request);
                await this.DataLayer.Applications.SaveAsync();

                return context.Response.Set(ResponseState.Success, request);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Application>(this.ApplicationValidator, request, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }       
        public async Task<ExecutionResponse<SearchResults<Application>>> Get(ApplicationsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Application>>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var results = await this.DataLayer.Applications.Get(criteria, GetIncludes(criteria.SearchIncludes));
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

                var existing = await DataLayer.Applications.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.ApplicationValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Applications.SetActivationAsync(existing.Id, true);
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

                var existing = await DataLayer.Applications.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.ApplicationValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Applications.SetActivationAsync(existing.Id, false);
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
        public async Task<ExecutionResponse<Application>> Edit(Application application, RequestContext requestContext)
        {
            var context = new ExecutionContext<Application>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var existing = await DataLayer.Applications.GetFirstAsync(x => x.Id == application.Id || x.Code == application.Code, null, this.Application_IncludeProperties_Full);
                MapUpdate(existing, application);

                DataLayer.Applications.Update(existing);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Application>(this.ApplicationValidator, application, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Application application, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await DataLayer.Applications.DeleteAsync(application.Id);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Application>(this.ApplicationValidator, application, ValidationMode.Delete),
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

                Check();

                #region validate.

                var application = await DataLayer.Applications.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.ApplicationValidator.ValidateAsync(application, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Applications.DeleteAsync(application.Id);
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

                var application = await DataLayer.Applications.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ApplicationValidator.ValidateAsync(application, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Applications.DeleteAsync(application.Id);
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

                var venue = await DataLayer.Applications.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ApplicationValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Applications.SetActivationAsync(venue.Id, true);
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

                var venue = await DataLayer.Applications.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ApplicationValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Applications.SetActivationAsync(venue.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsExists(ApplicationsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.Applications.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Application application, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.Applications.AnyAsync(x => ((application.Name != null && x.Name == application.Name.Trim()) ||
                                                                             (application.NameCultured != null && x.NameCultured == application.NameCultured.Trim()))
                                                                             &&
                                                                             (x.Code != application.Code)
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
            isValid = isValid && ApplicationValidator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Application : basic

                this.Application_IncludeProperties_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Application : search

                this.Application_IncludeProperties_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Application : full

                this.Application_IncludeProperties_Full = string.Join(",", new List<string>()
                {
                    "Candidate",
                    "Answers",
                    "Advertisement",
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
        private bool MapUpdate(Application existing, Application updated)
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

            existing.AdvertisementId = updated.AdvertisementId;            
            existing.CandidateId = updated.CandidateId;
            existing.HiringStepId = updated.HiringStepId;
            existing.AppId = updated.AppId;
            existing.ModuleId = updated.ModuleId;

            MapUpdate(existing.Answers, updated.Answers);
          
            return true;
        }

        private bool MapUpdate(Answer existing, Answer updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;

            existing.QuestionId = updated.QuestionId;

            return true;
        }
        private bool MapUpdate(IList<Answer> existing, IList<Answer> updated)
        {
            // ...
            if (existing == null) return false;
            updated = updated ?? new List<Answer>();

            // get [deleted records] ...
            foreach (var existingItem in existing)
            {
                var isDeleted = !updated.Any(x => x.Id == existingItem.Id);

                // delete the [existingItem]
                if (isDeleted)
                {
                    existing.Remove(existingItem);
                }
            }

            // get [updated records] ...
            foreach (var updatedItem in updated)
            {
                var existingItem = existing.Where(x => x.Id == updatedItem.Id && updatedItem.Id != 0).FirstOrDefault();

                if (existingItem != null)
                {
                    MapUpdate(existingItem, updatedItem);
                }
            }

            // get [inserted records] ...
            foreach (var updatedItem in updated)
            {
                var isInserted = !existing.Any(x => x.Id == updatedItem.Id && x.Id != 0);

                // insert the [updatedItem]
                if (isInserted)
                {
                    existing.Add(updatedItem);
                }
            }

            return true;
        }

        #endregion
        #region includes
        private string GetIncludes(SearchIncludes searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludes.Search:
                    return this.Application_IncludeProperties_Search;
                case SearchIncludes.Full:
                    return this.Application_IncludeProperties_Full;
                case SearchIncludes.Basic:
                default:
                    return this.Application_IncludeProperties_Basic;
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
