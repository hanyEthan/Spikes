using ADS.Common.Contracts;
using System;
using System.Runtime.Serialization;
using ADS.Common.Handlers;
using ADS.Common.Models.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy
{
    [Serializable]
    public class PolicyFieldValue : ISerializable, IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Guid PolicyId { get; set; }
        public Policy Policy { get; set; }

        public Guid PolicyFieldId { get; set; }
        public PolicyField PolicyField { get; set; }

        #region cst ...

        public PolicyFieldValue()
        {
        }

        public PolicyFieldValue(PolicyField policyField, string value)
        {
            if (policyField == null) return;

            Name = policyField.Name;
            Value = value;
            PolicyField = policyField;
            PolicyFieldId = policyField.Id;
        }

        #endregion
        #region ISerializable

        public PolicyFieldValue(SerializationInfo info, StreamingContext ctxt)
        {
            this.Id = (Guid) info.GetValue("Id", typeof (Guid));
            this.Name = info.GetString("Name");
            this.Value = info.GetString("Value");
            this.PolicyId = (Guid) info.GetValue("PolicyId", typeof (Guid));
            this.Policy = (Policy) info.GetValue("Policy", typeof (Policy));
            this.PolicyFieldId = (Guid) info.GetValue("PolicyFieldId", typeof (Guid));
            this.PolicyField = (PolicyField) info.GetValue("PolicyField", typeof (PolicyField));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", this.Id);
            info.AddValue("Name", this.Name);
            info.AddValue("Value", this.Value);
            info.AddValue("PolicyId", this.PolicyId);
            info.AddValue("Policy", this.Policy);
            info.AddValue("PolicyFieldId", this.PolicyFieldId);
            info.AddValue("PolicyField", this.PolicyField);
        }

        #endregion
    }
}
