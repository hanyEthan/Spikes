using System;
using ADS.Common.Contracts;

namespace ADS.Common.Models.Domain.Authorization
{
    [Serializable]
    public class RoleSearchCriteria : IXSerializable
    {
        #region props ...

        public string Name { get; set; }
        public string Code { get; set; }
        
        #endregion
        #region cst ...

        public RoleSearchCriteria( string name )
        {
            this.Name = name;
        }
        public RoleSearchCriteria( string name , string code ) : this( name )
        {
            this.Code = code;
        }
        
        #endregion
        #region Helpers

        public override string ToString()
        {
            return string.Format( "RoleSearchCriteria_{0}_{1}" , Name , Code );
        }

        #endregion
    }
}