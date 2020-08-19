using System;

namespace ADS.Common.Contracts.Cache
{
    public interface ICacheHandler
    {
        bool Add<T>( Guid clusterId , string key , T value );
        T Get<T>( Guid clusterId , string key );
        bool Contains( Guid clusterId , string key );
        bool Invalidate( Guid clusterId , bool invalidateGraph );
    }
}
