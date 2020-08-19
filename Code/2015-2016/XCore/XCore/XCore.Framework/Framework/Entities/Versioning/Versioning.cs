using System;

namespace XCore.Framework.Framework.Entities.Versioning
{
    public class Versioning : IVersionable
    {
        #region props.

        public string VersionNumber
        {
            get
            {
                var version = string.Format( "{0}{1}" , !string.IsNullOrWhiteSpace( VersionNumberMajor ) ? VersionNumberMajor : "" , VersionNumberMinor != 0 ? "." + VersionNumberMinor : "" );
                return !string.IsNullOrEmpty( version ) ? version : "1";
            }
            set
            {
            }
        }
        public string VersionNumberMajor { get; set; }
        public int VersionNumberMinor { get; set; }

        private string _VersionCode;
        public string VersionCode { get { return _VersionCode ?? VersionNumber; } set { _VersionCode = value; } }

        public int VersionSubCount { get; set; }

        private string _VersionBranchSource;
        public string VersionBranchSource
        {
            get { return this._VersionBranchSource ?? this.VersionNumber; }
            set { _VersionBranchSource = value; }
        }

        public string VersionCorrelationBase { get; set; }
        public string VersionCorrelationBranch { get; set; }

        public bool VersionIsLatestInBranch { get; set; } = true;
        public bool VersionIsLatestInBase { get; set; } = true;

        private string _VersionLatestVersion;
        public string VersionLatestVersion
        {
            get { return this._VersionLatestVersion ?? this.VersionNumber; }
            set { _VersionLatestVersion = value; }
        }

        public string VersionMetadata { get; set; }

        #endregion
        #region cst.

        public Versioning()
        {
            this.VersionNumberMajor = "1";
            this.VersionNumberMinor = 0;
            this.VersionSubCount = 0;
            this.VersionLatestVersion = this.VersionNumber;
            this.VersionIsLatestInBranch = true;
            this.VersionCorrelationBase = Guid.NewGuid().ToString();
            this.VersionCorrelationBranch = this.VersionCorrelationBase;
        }

        #endregion
        #region publics.

        public IVersionable Branch()
        {
            return Branch( null );
        }
        public IVersionable Branch( string label )
        {
            if ( this.VersionIsLatestInBranch )
            {
                return Extend( label );
            }
            else
            {
                return Fork( label );
            }
        }

        #endregion
        #region helpers.

        private IVersionable Extend( string label )
        {
            var from = this;
            var to = Clone();

            BaseExtend( from , to , label );
            from.VersionLatestVersion = to.VersionNumber;

            return to;
        }
        private IVersionable Fork( string label )
        {
            var from = this;
            var to = Clone();

            BaseExtend( from , to , label );
            to.VersionCorrelationBranch = Guid.NewGuid().ToString();

            return to;
        }

        private IVersionable Clone()
        {
            var to = new Versioning();

            to.VersionNumberMinor = this.VersionNumberMinor;
            to.VersionNumberMajor = this.VersionNumberMajor;
            to._VersionBranchSource = this._VersionBranchSource;
            to.VersionCorrelationBase = this.VersionCorrelationBase;
            to.VersionCorrelationBranch = this.VersionCorrelationBranch;
            to.VersionIsLatestInBranch = this.VersionIsLatestInBranch;
            to.VersionIsLatestInBase = this.VersionIsLatestInBase;
            to._VersionLatestVersion = this._VersionLatestVersion;
            to.VersionMetadata = this.VersionMetadata;

            return to;
        }
        private void BaseExtend( IVersionable from , IVersionable to , string label )
        {
            NewVersion( from , to );

            to.VersionLatestVersion = to.VersionNumber;
            to.VersionIsLatestInBranch = true;
            to.VersionIsLatestInBase = true;
            from.VersionIsLatestInBranch = false;
            from.VersionIsLatestInBase = false;
            to.VersionBranchSource = from.VersionNumber;
            to.VersionCode = label;
        }
        private void NewVersion( IVersionable from , IVersionable to )
        {
            to.VersionNumberMajor = from.VersionNumber;
            to.VersionNumberMinor = from.VersionSubCount + 1;

            from.VersionSubCount += 1;
        }

        #endregion
    }
}
