using XCore.Framework.Infrastructure.Context.Services.Contracts;

namespace XCore.Framework.Infrastructure.Entities.Mappers
{
    public class NativeMapper<T> : IModelMapper<T, T>
    {
        #region props.

        public static NativeMapper<T> Instance { get; private set; }

        #endregion
        #region cst.

        static NativeMapper()
        {
            Instance = new NativeMapper<T>();
        }

        #endregion

        #region IModelMapper

        public T Map(T from, object metadata = null)
        {
            return from;
        }

        #endregion
    }
}
