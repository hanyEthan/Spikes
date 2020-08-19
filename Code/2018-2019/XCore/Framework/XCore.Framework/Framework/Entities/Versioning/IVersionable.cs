namespace XCore.Framework.Framework.Entities.Versioning
{
    public interface IVersionable : IVersionBase
    {
        #region props.

        string VersionCode { get; set; }
        string VersionNumber { get; set; }
        string VersionNumberMajor { get; set; }
        int VersionNumberMinor { get; set; }

        int VersionSubCount { get; set; }

        string VersionBranchSource { get; set; }
        bool VersionIsLatestInBranch { get; set; }
        bool VersionIsLatestInBase { get; set; }

        string VersionMetadata { get; set; }

        #endregion
        #region publics.

        IVersionable Branch();
        IVersionable Branch( string label );

        #endregion
    }
    public interface IVersionBase
    {
        string VersionCorrelationBase { get; set; }
        string VersionCorrelationBranch { get; set; }
        string VersionLatestVersion { get; set; }
    }
}
