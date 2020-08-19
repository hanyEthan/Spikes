namespace XCore.Utilities.Infrastructure.Context.Services.Contracts
{
    public interface IModelMapper<TModelA, TModelB>
    {
        TModelB Map( TModelA from );
        TModelA Map( TModelB from );
    }
}
