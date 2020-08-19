using System.Threading.Tasks;
using XCore.Services.Hiring.Core.DataLayer.Contracts;

namespace XCore.Services.Attachments.Core.DataLayer.Contracts
{
    public interface IHiringDataUnity
    {
        bool? Initialized { get; }
        IAdvertisementRepository Advertisements { get; }
        IApplicationRepository Applications { get; }
        ICandidateRepository Candidates { get; }
        ISkillRepository Skills { get; }
        IHiringProcessRepository HiringProcesses { get; }
        IOrganizationRepository Organizations { get; }
        Task SaveAsync();
        void Save();
    }
}