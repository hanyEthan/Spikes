using System;
using System.Collections.Generic;
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
    public class CandidatesHandler : ICandidatesHandler
    {
        #region props.

        private readonly IHiringDataUnity DataLayer;
        private readonly IModelValidator<Candidate> CandidateValidator;
        private string Candidate_IncludeProperties_Basic { get; set; }
        private string Candidate_IncludeProperties_Search { get; set; }
        private string Candidate_IncludeProperties_Full { get; set; }

        #endregion
        #region cst.

        public CandidatesHandler(IHiringDataUnity dataLayer, IModelValidator<Candidate> CandidateValidator)
        {
            this.DataLayer = dataLayer;
            this.CandidateValidator = CandidateValidator;
            this.Initialized = Initialize();
        }

        #endregion
        
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.HiringHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region ICandidateHandler

        public async Task<ExecutionResponse<Candidate>> Create(Candidate request, RequestContext requestContext)
        {
            var context = new ExecutionContext<Candidate>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await this.DataLayer.Candidates.CreateAsync(request);
                await this.DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, request);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Candidate>(this.CandidateValidator, request, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }     
        public async Task<ExecutionResponse<SearchResults<Candidate>>> Get(CandidatesSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Candidate>>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var results = await this.DataLayer.Candidates.Get(criteria, GetIncludes(criteria.SearchIncludes));
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

                var existing = await DataLayer.Candidates.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.CandidateValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Candidates.SetActivationAsync(existing.Id, true);
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

                var existing = await DataLayer.Candidates.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.CandidateValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Candidates.SetActivationAsync(existing.Id, false);
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
        public async Task<ExecutionResponse<Candidate>> Edit(Candidate candidate, RequestContext requestContext)
        {
            var context = new ExecutionContext<Candidate>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var existing = await DataLayer.Candidates.GetFirstAsync(x => x.Id == candidate.Id || x.Code == candidate.Code, null, this.Candidate_IncludeProperties_Full);
                MapUpdate(existing, candidate);

                DataLayer.Candidates.Update(existing);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Candidate>(this.CandidateValidator, candidate, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Candidate candidate, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await DataLayer.Candidates.DeleteAsync(candidate.Id);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Candidate>(this.CandidateValidator, candidate, ValidationMode.Delete),
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

                var candidate = await DataLayer.Candidates.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.CandidateValidator.ValidateAsync(candidate, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Candidates.DeleteAsync(candidate.Id);
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

                var candidate = await DataLayer.Candidates.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.CandidateValidator.ValidateAsync(candidate, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Candidates.DeleteAsync(candidate.Id);
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

                var venue = await DataLayer.Candidates.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.CandidateValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Candidates.SetActivationAsync(venue.Id, true);
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

                var venue = await DataLayer.Candidates.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.CandidateValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Candidates.SetActivationAsync(venue.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsExists(CandidatesSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.Candidates.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Candidate candidate, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.Candidates.AnyAsync(x => ((candidate.Name != null && x.Name == candidate.Name.Trim()) ||
                                                                           (candidate.NameCultured != null && x.NameCultured == candidate.NameCultured.Trim()))
                                                                            &&
                                                                           (x.Code != candidate.Code)
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
            isValid = isValid && CandidateValidator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Candidate : basic

                this.Candidate_IncludeProperties_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Candidate : search

                this.Candidate_IncludeProperties_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Candidate : full

                this.Candidate_IncludeProperties_Full = string.Join(",", new List<string>()
                {

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
        private bool MapUpdate(Candidate existing, Candidate updated)
        {
            if (updated == null || existing == null) return false;

            #region Common

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.AppId = updated.AppId;
            existing.ModuleId = updated.ModuleId;
            existing.CandidateReferenceId = updated.CandidateReferenceId;

            #endregion

            return true;
        }

        #endregion
        #region includes
        private string GetIncludes(SearchIncludes searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludes.Search:
                    return this.Candidate_IncludeProperties_Search;
                case SearchIncludes.Full:
                    return this.Candidate_IncludeProperties_Full;
                case SearchIncludes.Basic:
                default:
                    return this.Candidate_IncludeProperties_Basic;
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
