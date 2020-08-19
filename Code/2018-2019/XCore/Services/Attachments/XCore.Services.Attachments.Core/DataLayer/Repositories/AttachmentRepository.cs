using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Attachments.Core.DataLayer.Context;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Enums;
using XCore.Services.Attachments.Core.Models.Support;

namespace XCore.Services.Config.Core.DataLayer.Repositories
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public AttachmentRepository(AttachmentDataContext dataContext) : base(dataContext)
        {
            this.Initialized = Initialize();
        }

        #endregion

        #region IAttachmentsRepository
        public async Task<bool> AnyAsync(AttachmentSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<Attachment>> GetAsync(AttachmentSearchCriteria criteria)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            // query ...
            return new SearchResults<Attachment>()
            {
                Results = await query.ToListAsync(),
                TotalCount = await query.CountAsync(),
            };

        }
        //public  void ConfirmStatus(Attachment attachment)
        //{
        //    //await Task.Run(() => { attachment.Status = AttachmentStatus.; });
        //    attachment.ModifiedDate = DateTime.UtcNow;
        //    context.Entry(attachment).State = EntityState.Modified;
        //}
        //public void ConfirmStatus(List<Attachment> attachments)
        //{
        //    // context.UpdateRange(attachment);
        //    //context.Entry(attachment).State = EntityState.Modified;

        //    foreach (var attachment in attachments)
        //    {
        //        attachment.ModifiedDate = DateTime.Now;
        //    }

        //    context.Set<Attachment>().UpdateRange(attachments);

        //}
        public void CreateConfirm (List<Attachment> attachments)
        {
          ConfirmStatus(attachments,new AttachmentConfirmationAction() 
              { ConfirmationAction = AttachmentConfirmationStatus.ConfirmAdd });
        }
        public void DeleteSoft(Attachment attachment)
        {
            ConfirmStatus(new List<Attachment>() { attachment }, new AttachmentConfirmationAction()
            { ConfirmationAction = AttachmentConfirmationStatus.RequestDelete });
        }
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        private ExpressionStarter<Attachment> GetQuery(AttachmentSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Attachment>(true);

            #region Id

            if (criteria.Id != null)
            {
                predicate = predicate.And(x => criteria.Id.Contains(x.Id));
            }

            #endregion

            return predicate;
        }

        public void ConfirmStatus(List<Attachment> Attachments, AttachmentConfirmationAction confirmationRequest)
        {
                #region Logic
                #region status comparisons.

                bool updateStore = false;
                foreach (var Attachment in Attachments)
                {
                    switch (confirmationRequest.ConfirmationAction)
                    {
                        case AttachmentConfirmationStatus.ConfirmAdd:
                            {
                                switch (Attachment.Status)
                                {
                                    case AttachmentStatus.MarkedForAddtion:
                                        {
                                            Attachment.Status = AttachmentStatus.Permanent;
                                            updateStore = true;
                                        }
                                        break;
                                    case AttachmentStatus.MarkedForDeletion:
                                    case AttachmentStatus.Permanent:
                                    default:
                                         break;
                            }
                            }
                            break;
                        case AttachmentConfirmationStatus.RequestDelete:
                            {
                                switch (Attachment.Status)
                                {
                                    case AttachmentStatus.Permanent:
                                        {
                                            Attachment.Status = AttachmentStatus.MarkedForDeletion;
                                            updateStore = true;
                                        }
                                        break;
                                    case AttachmentStatus.MarkedForAddtion:
                                    case AttachmentStatus.MarkedForDeletion:
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                if (updateStore)
                {
                    context.Set<Attachment>().UpdateRange(Attachments);
                }
            #endregion
        }
        #endregion
    }
}
