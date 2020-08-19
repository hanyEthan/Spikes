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
    public class SkillsHandler : ISkillsHandler
    {
        #region props.

        private readonly IHiringDataUnity DataLayer;
        private readonly IModelValidator<Skill> SkillValidator;
        private string Skill_IncludeProperties_Basic { get; set; }
        private string Skill_IncludeProperties_Search { get; set; }
        private string Skill_IncludeProperties_Full { get; set; }
        #endregion
        #region cst.

        public SkillsHandler(IHiringDataUnity dataLayer, IModelValidator<Skill> SkillValidator)
        {
            this.DataLayer = dataLayer;
            this.SkillValidator = SkillValidator;
            this.Initialized = Initialize();
        }

        #endregion
        
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.HiringHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region ISkillsHandler

        public async Task<ExecutionResponse<Skill>> Create(Skill request, RequestContext requestContext)
        {
            var context = new ExecutionContext<Skill>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await this.DataLayer.Skills.CreateAsync(request);
                await this.DataLayer.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, request);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Skill>(this.SkillValidator, request, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }       
        public async Task<ExecutionResponse<SearchResults<Skill>>> Get(SkillsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Skill>>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var results = await this.DataLayer.Skills.Get(criteria, GetIncludes(criteria.SearchIncludes));
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
                Check();

                #region Logic

                #region DL

                #region validate.

                var existing = await DataLayer.Skills.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SkillValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Skills.SetActivationAsync(existing.Id, true);
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

                var existing = await DataLayer.Skills.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SkillValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Skills.SetActivationAsync(existing.Id, false);
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
        public async Task<ExecutionResponse<Skill>> Edit(Skill skill, RequestContext requestContext)
        {
            var context = new ExecutionContext<Skill>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var existing = await DataLayer.Skills.GetFirstAsync(x => x.Id == skill.Id || x.Code == skill.Code, null, this.Skill_IncludeProperties_Full);
                MapUpdate(existing, skill);

                DataLayer.Skills.Update(existing);
                await DataLayer.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, existing);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Skill>(this.SkillValidator, skill, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Skill skill, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await DataLayer.Skills.DeleteAsync(skill.Id);
                await DataLayer.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Skill>(this.SkillValidator, skill, ValidationMode.Delete),
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

                var Skill = await DataLayer.Skills.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.SkillValidator.ValidateAsync(Skill, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Skills.DeleteAsync(Skill.Id);
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
        public async Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var skill = await DataLayer.Skills.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.SkillValidator.ValidateAsync(skill, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Skills.DeleteAsync(skill.Id);
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
        public async Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var venue = await DataLayer.Skills.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.SkillValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Skills.SetActivationAsync(venue.Id, true);
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
        public async Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var venue = await DataLayer.Skills.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.SkillValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Skills.SetActivationAsync(venue.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsExists(SkillsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await DataLayer.Skills.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Skill skill, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await DataLayer.Skills.AnyAsync(x => ((skill.Name != null && x.Name == skill.Name.Trim()) ||
                                                                       (skill.NameCultured != null && x.NameCultured == skill.NameCultured.Trim()))
                                                                        &&
                                                                       (x.Code != skill.Code)
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
            isValid = isValid && SkillValidator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Skill : basic

                this.Skill_IncludeProperties_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Skill : search

                this.Skill_IncludeProperties_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Skill : full

                this.Skill_IncludeProperties_Full = string.Join(",", new List<string>()
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
        private bool MapUpdate(Skill existing, Skill updated)
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

            return true;
        }

        #endregion
        #region includes

        private string GetIncludes(SearchIncludes searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludes.Search:
                    return this.Skill_IncludeProperties_Search;
                case SearchIncludes.Full:
                    return this.Skill_IncludeProperties_Full;
                case SearchIncludes.Basic:
                default:
                    return this.Skill_IncludeProperties_Basic;
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
