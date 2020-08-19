namespace XCore.Framework.Infrastructure.Context.Services.Contracts
{
    public interface IModelMapper<TModelA, TModelB>
    {
        TModelB Map( TModelA from , object metadata = null);
        TModelA Map( TModelB from , object metadata = null);
    }
}
