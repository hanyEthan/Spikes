using MediatR;
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
using XCore.Services.Personnel.Core.Contracts.Events;
using XCore.Services.Personnel.Core.Contracts.Personnels;
using XCore.Services.Personnel.DataLayer.Contracts;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.Events.Domain;
using XCore.Services.Personnel.Models.Personnels;

namespace XCore.Services.Personnel.Core.Handlers.Personnels
{
    public class PersonnelHandler : IPersonnelHandler
    {
        #region props.
        private string Personnel_DataInclude_Basic { get; set; }
        private string Personnel_DataInclude_Search { get; set; }
        private string Personnel_DataInclude_Full { get; set; }
        private readonly IModelValidator<Person> PersonnelModelValidator;
        private readonly IPersonnelDataUnity dataHandler;
        private readonly IMediator _mediator;
        #endregion
        #region cst.
        public PersonnelHandler(IPersonnelDataUnity DataHandler, IModelValidator<Person> PersonnelModelValidator, IMediator mediator)
        {
            this.dataHandler = DataHandler;
            this.PersonnelModelValidator = PersonnelModelValidator;
            this._mediator = mediator;

            this.Initialized = Initialize();
        }
        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.PersonnelHandler.{Guid.NewGuid()}"; } }

        #endregion

        #region IPersonnelHandler

        public async Task<ExecutionResponse<SearchResults<Person>>> Get(PersonSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Person>>();
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

                    var PersonnelModels = await dataHandler.Personnel.GetAsync(criteria, GetIncludes(criteria.SearchIncludes));
                    return context.Response.Set(ResponseState.Success, PersonnelModels);

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
        public async Task<ExecutionResponse<Person>> Create(Person Personnel, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Person>();
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

                    await dataHandler.Personnel.CreateAsync(Personnel);
                    await dataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_PersonCreated(Personnel);

                    #endregion
                    return context.Response.Set(ResponseState.Success, Personnel);
                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Person>(this.PersonnelModelValidator, Personnel, ValidationMode.Create),
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
        public async Task<ExecutionResponse<Person>> Edit(Person Personnel, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Person>();
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

                    var existing = await dataHandler.Personnel.GetFirstAsync(x => x.Id == Personnel.Id || x.Code == Personnel.Code, null, this.Personnel_DataInclude_Full);
                    MapUpdate(existing, Personnel);

                    dataHandler.Personnel.Update(existing);
                    await dataHandler.SaveAsync();
                    #region events.

                    await RaiseEvent_PersonUpdated(Personnel);

                    #endregion

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Person>(this.PersonnelModelValidator, Personnel, ValidationMode.Edit),
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
        public async Task<ExecutionResponse<bool>> Delete(Person Personnel, RequestContext requestContext)
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

                    await dataHandler.Personnel.DeleteAsync(Personnel);
                    await dataHandler.SaveAsync();
                    #region events.

                    await RaiseEvent_PersonDeleted(Personnel);

                    #endregion

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Person>(this.PersonnelModelValidator, Personnel, ValidationMode.Delete),
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
                    #region validate.

                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, false);
                    }

                    #endregion
                    #region validate.

                    var app = await dataHandler.Personnel.GetFirstAsync(x => x.Id == id);
                    var validationResponse = await this.PersonnelModelValidator.ValidateAsync(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    #region DL

                    await dataHandler.Personnel.DeleteAsync(app.Id);
                    await dataHandler.SaveAsync();
                    await RaiseEvent_PersonDeleted(app);
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
        public async Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic

                    #region DL

                    #region validate.
                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, false);
                    }

                    var existing = await dataHandler.Personnel.GetFirstAsync(x => x.Id == id);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.PersonnelModelValidator.ValidateAsync(existing, ValidationMode.Activate);
                    if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                    #endregion

                    await dataHandler.Personnel.SetActivationAsync(existing.Id, true);
                    await dataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_PersonActivated(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
  
        }
        public async Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic

                    #region DL

                    #region validate.
                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, false);
                    }

                    var existing = await dataHandler.Personnel.GetFirstAsync(x => x.Id == id);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.PersonnelModelValidator.ValidateAsync(existing, ValidationMode.Activate);
                    if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                    #endregion

                    await dataHandler.Personnel.SetActivationAsync(existing.Id, false);
                    await dataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_PersonDeActivated(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> IsUnique(Person Personnel, RequestContext requestContext)
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

                    var isExisting = await dataHandler.Personnel.AnyAsync(x => ((Personnel.Name != null && x.Name == Personnel.Name.Trim()) ||
                                                                                (Personnel.NameCultured != null && x.NameCultured == Personnel.NameCultured.Trim()))
                                                                                &&
                                                                                (x.Code != Personnel.Code)
                                                                                &&
                                                                                (x.IsActive == true));

                    return context.Response.Set(ResponseState.Success, !isExisting);

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
        public async Task<ExecutionResponse<bool>> IsExists(PersonSearchCriteria criteria, RequestContext requestContext)
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

                    var isExisting = await dataHandler.Personnel.AnyAsync(criteria);
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
        #region events.

        private async Task RaiseEvent_PersonCreated(Person person)
        {
            if (person == null) return;
            if (_mediator == null) return;
            if ((person?.Id).GetValueOrDefault() == 0) return;

            await this._mediator.Publish(new PersonCreatedDomainEvent()
            {
                App = person.AppId,
                Module = person.ModuleId,
                PersonId = person.Id
            });
        }
        private async Task RaiseEvent_PersonActivated(Person person)
        {
            if (person == null) return;
            if (_mediator == null) return;
            if ((person?.Id).GetValueOrDefault() == 0) return;

            await this._mediator.Publish(new PersonActivatedDomainEvent()
            {
                App = person.AppId,
                Module = person.ModuleId,
                Code = person.Code
            });
        }
        private async Task RaiseEvent_PersonDeActivated(Person person)
        {
            if (person == null) return;
            if (_mediator == null) return;
            if ((person?.Id).GetValueOrDefault() == 0) return;

            await this._mediator.Publish(new PersonDeActivatedDomainEvent()
            {
                App = person.AppId,
                Module = person.ModuleId,
                Code = person.Code
            });
        }
        private async Task RaiseEvent_PersonUpdated(Person person)
        {
            if (person == null) return;
            if (_mediator == null) return;
            if ((person?.Id).GetValueOrDefault() == 0) return;

            await this._mediator.Publish(new PersonUpdatedDomainEvent()
            {
                App = person.AppId,
                Module = person.ModuleId,
                PersonId = person.Id
            });
        }
        private async Task RaiseEvent_PersonDeleted(Person person)
        {
            if (person == null) return;
            if (_mediator == null) return;
            if ((person?.Id).GetValueOrDefault() == 0) return;

            await this._mediator.Publish(new PersonDeletedDomainEvent()
            {
                App = person.AppId,
                Module = person.ModuleId,
                Code = person.Code
            });
        }
        #endregion
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (dataHandler?.Initialized ?? false);
            isValid = isValid && (PersonnelModelValidator != null);
            isValid = isValid && _mediator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Personnel : basic

                this.Personnel_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Personnel : Search

                this.Personnel_DataInclude_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Personnel : full

                this.Personnel_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "Manager",
                    "Department",
                    "Account",
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
        private bool MapUpdate(Person existing, Person updated)
        {
            if (updated == null || existing == null) return false;
            
            existing.ManagerId = updated.ManagerId;
            existing.DepartmentId = updated.DepartmentId;
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
                    return this.Personnel_DataInclude_Search;
                case SearchIncludesEnum.Full:
                    return this.Personnel_DataInclude_Full;
                case SearchIncludesEnum.Basic:
                default:
                    return this.Personnel_DataInclude_Basic;
            }
        }
        #endregion
    }
}
