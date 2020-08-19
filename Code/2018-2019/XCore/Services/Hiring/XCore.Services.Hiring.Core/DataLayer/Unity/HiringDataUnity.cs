using System.Threading.Tasks;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Hiring.Core.DataLayer.Context;
using XCore.Services.Hiring.Core.DataLayer.Contracts;

namespace XCore.Services.Hirings.Core.DataLayer.Unity
{
    public class HiringDataUnity : IHiringDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }

        private readonly HiringDataContext DataContext;

        public IAdvertisementRepository Advertisements { get; protected set; }

        public IApplicationRepository Applications { get; protected set; }

        public ICandidateRepository Candidates { get; protected set; }

        public ISkillRepository Skills { get; protected set; }

        public IHiringProcessRepository HiringProcesses { get; protected set; }

        public IOrganizationRepository Organizations { get; protected set; }

        #endregion
        #region cst.

        public HiringDataUnity(HiringDataContext dataContext,
                               IAdvertisementRepository advertisementRepository,
                               IApplicationRepository applicationRepository,
                               ICandidateRepository candidateRepository,
                               ISkillRepository skillRepository,
                               IHiringProcessRepository processRepository,
                               IOrganizationRepository organizationRepository
            )
        {
            this.DataContext = dataContext;
            this.Organizations = organizationRepository;
            this.HiringProcesses = processRepository;
            this.Skills = skillRepository;
            this.Candidates = candidateRepository;
            this.Applications = applicationRepository;
            this.Advertisements = advertisementRepository;

            this.Initialized = Initialize();
        }

        #endregion

        #region publics

        public void Save()
        {
            this.DataContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await this.DataContext.SaveChangesAsync();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && DataContext != null;
            isValid = isValid &&
                (this.Advertisements?.Initialized.GetValueOrDefault() ?? false) &&
                                 (this.Applications?.Initialized.GetValueOrDefault() ?? false) &&
                                 (this.Candidates?.Initialized.GetValueOrDefault() ?? false) &&
                                 (this.Organizations?.Initialized.GetValueOrDefault() ?? false) &&
                                 (this.HiringProcesses?.Initialized.GetValueOrDefault() ?? false) &&
                                 (this.Skills?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion
    }
}
