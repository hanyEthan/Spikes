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
using XCore.Services.Hiring.Core.Models.Relations;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Handlers
{
    public class AdvertisementsHandler : IAdvertisementsHandler
    {
        #region props.

        private readonly IHiringDataUnity DataLayer;
        private readonly IModelValidator<Advertisement> AdvertisementValidator;
        private string Advertisement_IncludeProperties_Basic { get; set; }
        private string Advertisement_IncludeProperties_Search { get; set; }
        private string Advertisement_IncludeProperties_Full { get; set; }

        #endregion
        #region cst.

        public AdvertisementsHandler(IHiringDataUnity dataLayer, IModelValidator<Advertisement> AdvertisementValidator)
        {
            this.DataLayer = dataLayer;
            this.AdvertisementValidator = AdvertisementValidator;
            this.Initialized = Initialize();
        }

        #endregion
        
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.HiringHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IAdvertisementsHandler

        public async Task<ExecutionResponse<Advertisement>> Create(Advertisement request, RequestContext requestContext)
        {
            var context = new ExecutionContext<Advertisement>();
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

                await this.DataLayer.Advertisements.CreateAsync(request);
                await this.DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, request);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Advertisement>(this.AdvertisementValidator, request, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<SearchResults<Advertisement>>> Get(AdvertisementsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Advertisement>>();
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

                var results = await this.DataLayer.Advertisements.Get(criteria, GetIncludes(criteria.SearchIncludes));
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

                #region DL

                #region validate.

                var existing = await DataLayer.Advertisements.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.AdvertisementValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Advertisements.SetActivationAsync(existing.Id, true);
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

                #region DL

                #region validate.

                var existing = await DataLayer.Advertisements.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.AdvertisementValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Advertisements.SetActivationAsync(existing.Id, false);
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
        public async Task<ExecutionResponse<Advertisement>> Edit(Advertisement advertisement, RequestContext requestContext)
        {
            var context = new ExecutionContext<Advertisement>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await DataLayer.Advertisements.GetFirstAsync(x => x.Id == advertisement.Id || x.Code == advertisement.Code, null, this.Advertisement_IncludeProperties_Full);
                MapUpdate(existing, advertisement);

                DataLayer.Advertisements.Update(existing);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Advertisement>(this.AdvertisementValidator, advertisement, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Advertisement advertisement, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await DataLayer.Advertisements.DeleteAsync(advertisement.Id);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Advertisement>(this.AdvertisementValidator, advertisement, ValidationMode.Delete),
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

                #region validate.

                var advertisement = await DataLayer.Advertisements.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.AdvertisementValidator.ValidateAsync(advertisement, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Advertisements.DeleteAsync(advertisement.Id);
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

                #region validate.

                var advertisement = await DataLayer.Advertisements.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.AdvertisementValidator.ValidateAsync(advertisement, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Advertisements.DeleteAsync(advertisement);
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

                #region validate.

                var venue = await DataLayer.Advertisements.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.AdvertisementValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Advertisements.SetActivationAsync(venue.Id, true);
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

                #region validate.

                var venue = await DataLayer.Advertisements.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.AdvertisementValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Advertisements.SetActivationAsync(venue.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsExists(AdvertisementsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await DataLayer.Advertisements.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Advertisement advertisement, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await DataLayer.Advertisements.AnyAsync(x => (//(advertisement.Name != null && x.Name == advertisement.Name.Trim()) ||
                                                                               (advertisement.Title != null && x.Title == advertisement.Title.Trim()) ||
                                                                               (advertisement.NameCultured != null && x.NameCultured == advertisement.NameCultured.Trim()))
                                                                               &&
                                                                               (x.Code != advertisement.Code)
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
            isValid = isValid && AdvertisementValidator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Advertisement : basic

                this.Advertisement_IncludeProperties_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Advertisement : search

                this.Advertisement_IncludeProperties_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Advertisement : full

                this.Advertisement_IncludeProperties_Full = string.Join(",", new List<string>()
                {
                    //"HiringProcces",
                    "Positions",
                    "Organization",
                    "Questions",
                    "Skills.Skill",
                    "Role",
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
        private bool MapUpdate(Advertisement existing, Advertisement updated)
        {
            if (updated == null || existing == null) return false;

            #region Common

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;

            #endregion

            existing.HiringProccesId = updated.HiringProccesId;
            existing.OrganizationId = updated.OrganizationId;
            existing.RoleId = updated.RoleId;
            existing.Title = updated.Title;
            existing.AppId = updated.AppId;
            existing.ModuleId = updated.ModuleId;

            existing.Skills = updated.Skills;
            existing.Questions = updated.Questions;
            MapUpdate(existing.Positions, updated.Positions);
            MapUpdate(existing.Role, updated.Role);

            return true;
        }
        private bool MapUpdate(Position existing, Position updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.AdvertisementId = updated.AdvertisementId;

            return true;
        }
        private bool MapUpdate(Role existing, Role updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;

            return true;
        }
        private bool MapUpdate(IList<Position> existing, IList<Position> updated)
        {
            // ...
            if (existing == null) return false;
            updated = updated ?? new List<Position>();

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
                    return this.Advertisement_IncludeProperties_Search;
                case SearchIncludes.Full:
                    return this.Advertisement_IncludeProperties_Full;
                case SearchIncludes.Basic:
                default:
                    return this.Advertisement_IncludeProperties_Basic;
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
