using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy
{
    [Serializable]
    public class Policy : IDynamicValuesProvider , ISerializable , IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public bool SystemPolicy { get; set; }

        public Guid PolicyTypeId { get; set; }
        public PolicyType PolicyType { get; set; }

        public IList<PolicyFieldValue> Values { get; set; }
      
        #region cst ...

        public Policy()
        {
            Values = new List<PolicyFieldValue>();
        }
        public Policy( Guid id , string name, string nameCultureVariant , string code , bool active , Guid policyTypeId ) : this()
        {
            Id = id;
            Name = name;
            NameCultureVarient = nameCultureVariant;
            Code = code;
            Active = active;
            PolicyTypeId = policyTypeId;
        }
        
        #endregion
        #region ISerializable
        public Policy( SerializationInfo info , StreamingContext ctxt )
        {
            this.Id = ( Guid ) info.GetValue ( "Id" , typeof ( Guid ) );
            this.Name = info.GetString ( "Name" );
            this.NameCultureVarient = info.GetString ( "NameCultureVarient" );
            this.Code = info.GetString ( "Code" );
            this.Active = info.GetBoolean ( "Active" );
            this.PolicyTypeId = ( Guid ) info.GetValue ( "PolicyTypeId" , typeof ( Guid ) );
            this.PolicyType = ( PolicyType ) info.GetValue ( "PolicyType" , typeof ( PolicyType ) );
            this.Values = ( IList<PolicyFieldValue> ) info.GetValue ( "Values" , typeof ( IList<PolicyFieldValue> ) );

        }
        public void GetObjectData( SerializationInfo info , StreamingContext context )
        {
            info.AddValue ( "Id" , this.Id );
            info.AddValue ( "Name" , this.Name );
            info.AddValue ( "NameCultureVarient" , this.NameCultureVarient );
            info.AddValue ( "Code" , this.Code );
            info.AddValue ( "Active" , this.Active );
            info.AddValue ( "PolicyTypeId" , this.PolicyTypeId );
            info.AddValue ( "PolicyType" , this.PolicyType );
            info.AddValue ( "Values" , this.Values );
        } 
        #endregion
    }
}
