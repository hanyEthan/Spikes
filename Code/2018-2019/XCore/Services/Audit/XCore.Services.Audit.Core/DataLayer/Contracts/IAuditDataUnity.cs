using System.Threading.Tasks;

namespace XCore.Services.Audit.Core.DataLayer.Contracts
{
    public interface IAuditDataUnity
    {
        bool? Initialized { get; }
        IAuditRepository Audit { get; }
        IAuditReadRepository AuditRead { get; }
        Task SaveAsync();
    }
}