using System;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;
using ADS.Common.Models.Domain.Authorization;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IPersonnelHandler : IBaseHandler
    {
        ExecutionResponse<bool> CreatePerson( Person person , List<PolicyFieldValue> customFields , RequestContext requestContext );
        ExecutionResponse<bool> EditPerson( Person person , List<PolicyFieldValue> customFields , RequestContext requestContext );
        ExecutionResponse<bool> EditPersonPassword(Person person, RequestContext requestContext);
        ExecutionResponse<bool> EditPersonSecurity(Actor actor, RequestContext requestContext);
        
        ExecutionResponse<bool> EditPersonStatus( Guid personId , bool activeStatus , RequestContext requestContext );
        ExecutionResponse<Person> GetPerson( Guid personId , RequestContext requestContext );
        ExecutionResponse<Person> GetPerson( string username , RequestContext requestContext );
        ExecutionResponse<Person> GetPersonByIdentifier( string identifier , RequestContext requestContext );

        ExecutionResponse<PersonSearchResult> GetPersonnel( PersonSearchCriteria criteria , RequestContext requestContext );
        ExecutionResponse<object> GetPersonnelByRoot( Guid rootId , RequestContext requestContext );
        ExecutionResponse<List<Guid>> GetPersonnelWithUnTransferredCredits();
        ExecutionResponse<PersonnelDelegatesSearchResult> GetPersonnelDelegates(PersonnelDelegatesSearchCriteria criteria, RequestContext requestContext);
        ExecutionResponse<bool> CanActFor(Guid delegateId, Guid personId, RequestContext requestContext);
        ExecutionResponse<bool> CreateDelegate(PersonDelegate personDelegate, RequestContext requestContext);
        ExecutionResponse<bool> EditDelegate(PersonDelegate personDelegate, RequestContext requestContext);
    }
}