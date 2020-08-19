using System.Collections.Generic;
using System.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Messaging.Queues.Contracts;
using XCore.Framework.Infrastructure.Messaging.Queues.Models;
using XCore.Framework.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Repositories
{
    public class QueueRepository : Repository<MQMessage>, IQueueRepository
    {
        #region cst.
        public QueueRepository( DbContext context ) : base( context )
        {
        }

        #endregion
        #region Publics

        public virtual MQMessage Get( int Id )
        {
            return context.Set<MQMessage>().FirstOrDefault( m => m.Id == Id );
        }
        public virtual MQMessage GetNext()
        {
            return context.Set<MQMessage>().Where( n => n.Status == MQMessageStatus.UnProcessed ).OrderBy( n => n.CreatedDate ).FirstOrDefault();
        }
        public virtual List<MQMessage> GetAll()
        {
            return context.Set<MQMessage>().Where( n => n.Status == MQMessageStatus.UnProcessed ).ToList();
        }
        public virtual int GetCount()
        {
            return context.Set<MQMessage>().Where( n => n.Status == MQMessageStatus.UnProcessed ).Count();
        }
        public virtual MQMessage GetNext( MQCriteria criteria )
        {
            if ( criteria == null )
            {
                return context.Set<MQMessage>().Where( n => n.Status == MQMessageStatus.UnProcessed ).OrderBy( n => n.CreatedDate ).FirstOrDefault();
            }
            var predicate = PredicateBuilder.New<MQMessage>( true );
            if ( criteria.MaxRetialsCount.HasValue )
            {
                predicate = predicate.And( p => p.RetrialsCounter == 0 || p.RetrialsCounter < criteria.MaxRetialsCount.Value );
            }
            if ( criteria.Statuses != null )
            {
                predicate = predicate.And( p => criteria.Statuses.Any( c => c == p.Status ) );
            }
            if ( !string.IsNullOrWhiteSpace( criteria.Type ) )
            {
                predicate = predicate.And( p => p.Type == criteria.Type );
            }
            var message = base.GetQueryable( true , predicate , x => x.OrderBy( q => q.CreatedDate ) ).FirstOrDefault();

            return message;
        }
        public virtual List<MQMessage> GetNext( MQCriteria criteria , int messagesCount )
        {
            if ( criteria == null )
            {
                return context.Set<MQMessage>().Where( n => n.Status == MQMessageStatus.UnProcessed ).OrderBy( n => n.CreatedDate ).Take( messagesCount ).ToList();
            }
            var predicate = PredicateBuilder.New<MQMessage>( true );
            if ( criteria.MaxRetialsCount.HasValue )
            {
                predicate = predicate.And( p => p.RetrialsCounter == 0 || p.RetrialsCounter < criteria.MaxRetialsCount.Value );
            }
            if ( criteria.Statuses != null )
            {
                predicate = predicate.And( p => criteria.Statuses.Any( c => c == p.Status ) );
            }
            if ( !string.IsNullOrWhiteSpace( criteria.Type ) )
            {
                predicate = predicate.And( p => p.Type == criteria.Type );
            }
            var messages = base.GetQueryable( true , predicate , x => x.OrderBy( q => q.CreatedDate ) ).Take( messagesCount ).ToList();

            return messages;
        }
        public virtual bool Send( MQMessage message )
        {
            base.CreateAsync( message );

            return true;
        }
        public virtual bool Send( IList<MQMessage> messages )
        {
            foreach ( var message in messages )
            {
                base.CreateAsync( message );
            }

            return true;
        }

        #endregion
    }
}
