namespace ADS.Common.Bases.Services.Context.Contracts
{
    public interface IModelMapper<TModelA, TModelB>
    {
        TModelB Map(TModelA from);
        TModelA Map(TModelB from);
    }
}
