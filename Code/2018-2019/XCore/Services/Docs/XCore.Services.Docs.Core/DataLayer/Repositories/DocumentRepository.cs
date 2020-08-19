using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Docs.Core.DataLayer.Context;
using XCore.Services.Docs.Core.DataLayer.Contracts;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Config.Core.DataLayer.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public DocumentRepository(DocumentDataContext dataContext) : base(dataContext)
        {
            this.Initialized = Initialize();
        }

        #endregion

        #region IDocumentRepository

        public async Task<SearchResults<Document>> GetAsync(DocumentSearchCriteria criteria)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);
           
            // query ...
            return new SearchResults<Document>()
            {
                Results =await  query.ToListAsync(),
                TotalCount =await query.CountAsync(),
            };
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        private ExpressionStarter<Document> GetQuery(DocumentSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Document>(true);

            #region Id

            if (criteria.Id != null && criteria.Id.All(x => x!= 0))
            {
                predicate = predicate.And(x => criteria.Id.Contains(x.Id));
            }

            #endregion

            #region UserIds

            if (criteria.UserIds != null)
            {
                predicate = predicate.And(x => criteria.UserIds.Contains(x.UserId));
            }

            #endregion
            #region UserNames

            if (criteria.UserNames != null)
            {
                predicate = predicate.And(x => criteria.UserNames.Contains(x.UserName));
            }

            #endregion
            #region Apps

            if (criteria.Apps != null)
            {
                predicate = predicate.And(x => criteria.Apps.Contains(x.App));
            }

            #endregion
            #region Modules

            if (criteria.Modules != null)
            {
                predicate = predicate.And(x => criteria.Modules.Contains(x.Module));
            }

            #endregion
            #region Actions

            if (criteria.Actions != null)
            {
                predicate = predicate.And(x => criteria.Actions.Contains(x.Action));
            }

            #endregion
            #region Entities

            if (criteria.Entities != null)
            {
                predicate = predicate.And(x => criteria.Entities.Contains(x.Entity));
            }

            #endregion

            #region AttachId

            if (criteria.AttachId != null)
            {
                predicate = predicate.And(x => criteria.AttachId.Contains(x.AttachId));
            }

            #endregion
            #region Category

            if (criteria.Category != null)
            {
                predicate = predicate.And(x => criteria.Category.Contains(x.Category));
            }

            #endregion
            
            return predicate;
        }
      
        #endregion
    }
}
