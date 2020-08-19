using ADS.Common.Contracts;
using System;
using System.Runtime.Serialization;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy
{
    [Serializable]
    public class PolicyRule : ISerializable , IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public Guid PolicyTypeId { get; set; }
        public PolicyType PolicyType { get; set; }

        public Guid FieldId { get; set; }
        public PolicyField Field { get; set; }

        public PolicyRulesConditions Condition { get; set; }
        public string Meta { get; set; }

        #region Sub Models

        public enum PolicyRulesConditions
        {
            Exclusive = 477 ,
        }

        #endregion
        #region cst ...

        public PolicyRule()
        {
        }
        public PolicyRule( Guid policyTypeId , Guid fieldId , PolicyRulesConditions condition , string meta )
        {
            PolicyTypeId = policyTypeId;
            FieldId = fieldId;
            Condition = condition;
            Meta = meta;
        }
        public PolicyRule( Guid id , string code , Guid policyTypeId , Guid fieldId , PolicyRulesConditions condition , string meta )
            : this ( policyTypeId , fieldId , condition , meta )
        {
            Id = id;
            Code = code;
        }

        #endregion

        #region ISerializable
        public PolicyRule( SerializationInfo info , StreamingContext ctxt )
        {
            this.Id = ( Guid ) info.GetValue ( "Id" , typeof ( Guid ) );
            this.Code = info.GetString ( "Code" );
            this.PolicyTypeId = ( Guid ) info.GetValue ( "PolicyTypeId" , typeof ( Guid ) );
            this.FieldId = ( Guid ) info.GetValue ( "FieldId" , typeof ( Guid ) );
            this.PolicyType = ( PolicyType ) info.GetValue ( "PolicyType" , typeof ( PolicyType ) );
            this.Field = ( PolicyField ) info.GetValue ( "Field" , typeof ( PolicyField ) );
            this.Condition = ( PolicyRulesConditions ) info.GetInt32 ( "Condition" );
            this.Meta = info.GetString ( "Meta" );
            

        }
        public void GetObjectData( SerializationInfo info , StreamingContext context )
        {
            info.AddValue ( "Id" , this.Id );
            info.AddValue ( "Code" , this.Code );
            info.AddValue ( "PolicyTypeId" , this.PolicyTypeId );
            info.AddValue ( "FieldId" , this.FieldId );
            info.AddValue ( "PolicyType" , this.PolicyType );
            info.AddValue ( "Field" , this.Field );
            info.AddValue ( "Condition" , this.Condition );
            info.AddValue ( "Meta" , this.Meta );

        }
        #endregion
    }
}