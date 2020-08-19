using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Context.Execution.Models;
using XCore.Utilities.Infrastructure.Messaging.Pools.Contracts;
using XCore.Utilities.Infrastructure.Messaging.Pools.Handlers;
using XCore.Utilities.Infrastructure.Messaging.Pools.Service.Contracts;
using XCore.Utilities.Infrastructure.Messaging.Pools.Service.Mappers;
using XCore.Utilities.Infrastructure.Messaging.Pools.Service.Models;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Service.Services
{
    public class MPoolService : IMPoolService
    {
        #region props.

        private IMessagePoolSender sender;
        private IMessagePoolReader listener;

        #endregion
        #region cst.

        public MPoolService()
        {
            var handler = new MessagePoolHandler();
            sender = handler;
            listener = handler;
        }

        #endregion

        #region IMPoolService

        public async Task<Response<int>> AddMessage( MPoolMessageDataContract message )
        {
            try
            {
                return await Task.Factory.StartNew( () =>
                 {
                     var result = sender.Create( Mapper.Map( message ) );
                     return new Response<int>() { state = ( int ) ResponseState.Success , result = result };
                 } );
            }
            catch ( Exception ex )
            {
                NLogger.Error( "Exception : " + ex );
                return await Task.Factory.StartNew( () =>
                 {
                     return new Response<int>() { state = ( int ) ResponseState.Error , result = 0 };
                 } );
            }
        }
        public async Task<Response<List<MPoolMessageDataContract>>> GetMessages( MPoolCriteriaDataContract criteria )
        {
            try
            {
                return await Task.Factory.StartNew( () =>
                 {
                     var result = listener.Get( Mapper.Map( criteria ) );
                     return new Response<List<MPoolMessageDataContract>>() { state = ( int ) ResponseState.Success , result = Mapper.Map( result ) };
                 } );
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return await Task.Factory.StartNew( () =>
                 {
                     return new Response<List<MPoolMessageDataContract>>() { state = ( int ) ResponseState.Error , result = null };
                 } );
            }
        }
        public async Task<Response<bool>> RestoreMessages( List<string> messages )
        {
            try
            {
                return await Task.Factory.StartNew( () =>
                 {
                     var status = listener.Restore( messages );

                     return new Response<bool>() { state = ( int ) ResponseState.Success , result = status };
                 } );
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return await Task.Factory.StartNew( () =>
                 {
                     return new Response<bool>() { state = ( int ) ResponseState.Success , result = false };
                 } );
            }
        }
        public async Task<Response<bool>> DeleteMessages( List<string> messages )
        {
            try
            {
                return await Task.Factory.StartNew( () =>
                 {
                     var status = listener.Delete( messages );

                     return new Response<bool>() { state = ( int ) ResponseState.Success , result = status };
                 } );
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return await Task.Factory.StartNew( () =>
                 {
                     return new Response<bool>() { state = ( int ) ResponseState.Success , result = false };
                 } );
            }
        }

        #endregion
    }
}
