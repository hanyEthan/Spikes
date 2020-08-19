using ADS.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy
{
    [Serializable]
    public class PolicyType : ISerializable , IXSerializable
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVariant { get; set; }
        public bool SupportMultiAssociation { get; set; }
        public string CustomValidatorType { get; set; }
        public string CustomRepresentation { get; set; }
        public bool Active { get; set; }

        public IList<Policy> Policies { get; set; }
        public IList<PolicyField> Fields { get; set; }
        public IList<PolicyRule> Rules { get; set; }

        #region ISerializable

        public PolicyType()
        {
            Policies = new List<Policy>();
            Fields = new List<PolicyField>();
            Rules = new List<PolicyRule>();
        }
        public PolicyType( SerializationInfo info , StreamingContext ctxt )
        {
            this.Id = ( Guid ) info.GetValue ( "Id" , typeof ( Guid ) );
            this.Code = info.GetString ( "Code" );
            this.Name = info.GetString ( "Name" );
            this.NameCultureVariant = info.GetString ( "NameCultureVariant" );
            this.SupportMultiAssociation = info.GetBoolean ( "SupportMultiAssociation" );
            this.CustomValidatorType = info.GetString ( "CustomValidatorType" );
            this.CustomRepresentation = info.GetString( "CustomRepresentation" );
            this.Active = info.GetBoolean ( "Active" );

            this.Policies = ( IList<Policy> ) info.GetValue ( "Policies" , typeof ( IList<Policy> ) );
            this.Fields = ( IList<PolicyField> ) info.GetValue ( "Fields" , typeof ( IList<PolicyField> ) );
            this.Rules = ( IList<PolicyRule> ) info.GetValue ( "Rules" , typeof ( IList<PolicyRule> ) );
        }
        public void GetObjectData( SerializationInfo info , StreamingContext context )
        {
            info.AddValue ( "Id" , this.Id );
            info.AddValue ( "Code" , this.Code );
            info.AddValue ( "Name" , this.Name );
            info.AddValue ( "NameCultureVariant" , this.NameCultureVariant );
            info.AddValue ( "SupportMultiAssociation" , this.SupportMultiAssociation );
            info.AddValue ( "CustomValidatorType" , this.CustomValidatorType );
            info.AddValue( "CustomRepresentation" , this.CustomRepresentation );
            info.AddValue ( "Active" , this.Active );
            info.AddValue ( "Policies" , this.Policies );
            info.AddValue ( "Fields" , this.Fields );
            info.AddValue ( "Rules" , this.Rules );
        }
        #endregion
    }
}
