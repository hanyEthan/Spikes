using ADS.Common.Contracts;
using ADS.Common.Handlers;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using System;
using System.Runtime.Serialization;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy
{
    [Serializable]
    public class PolicyField : ISerializable , IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVariant { get; set; }

        public int DataTypeId { get; set; }
        public string DatasetReferenceTypeName { get; set; }
        public string DataCollectionReferenceId { get; set; }
        public string DataDefaultValue { get; set; }

        public Guid PolicyTypeId { get; set; }
        public PolicyType PolicyType { get; set; }

        public string ValidationRegularExpression { get; set; }

        public int Sequence { get; set; }
        public bool Active { get; set; }
        public bool Representative { get; set; }

        #region Helpers

        //public PolicyFieldDataType DataType { get { return ( PolicyFieldDataType ) DataTypeId; } }

        //MG,31-12-2014.Need DetailCode instance for this data type for localization reasons and system code is not a part of our ORM, so I sadly had to do this and this should be changed asap.
        private DetailCode _DataType = null;
        [XDontSerialize]
        public DetailCode DataType
        {
            get
            {
                try
                {
                    if ( _DataType == null )
                        _DataType = Broker.DetailCodeHandler.GetDetailCode ( DataTypeId );
                    return _DataType;
                }
                catch
                {
                    return null;
                }
            }
        }

        [XDontSerialize]
        public string RepresentativeStr
        {
            get
            { return Representative ? "Yes" : "No"; }
            set
            { Representative = value == "Yes" ? true : false; }
        }

        #endregion

        #region ISerializable
        public PolicyField()
        { }
        public PolicyField( SerializationInfo info , StreamingContext ctxt )
        {
            this.Id = ( Guid ) info.GetValue ( "Id" , typeof ( Guid ) );
            this.Name = info.GetString ( "Name" );

            this.NameCultureVariant = info.GetString ( "NameCultureVariant" );
            this.DataTypeId = info.GetInt32 ( "DataTypeId" );

            this.DatasetReferenceTypeName = info.GetString ( "DatasetReferenceTypeName" );
            this.DataCollectionReferenceId = info.GetString ( "DataCollectionReferenceId" );

            this.DataDefaultValue = info.GetString ( "DataDefaultValue" );
            this.PolicyTypeId = ( Guid ) info.GetValue ( "PolicyTypeId" , typeof ( Guid ) );

            this.PolicyType = ( PolicyType ) info.GetValue ( "PolicyType" , typeof ( PolicyType ) );
            this.ValidationRegularExpression = info.GetString ( "ValidationRegularExpression" );

            this.Sequence = info.GetInt32 ( "Sequence" );
            this.Active = info.GetBoolean ( "Active" );
            this.Representative = info.GetBoolean ( "Representative" );



        }
        public void GetObjectData( SerializationInfo info , StreamingContext context )
        {
            info.AddValue ( "Id" , this.Id );
            info.AddValue ( "Name" , this.Name );

            info.AddValue ( "NameCultureVariant" , this.NameCultureVariant );
            info.AddValue ( "DataTypeId" , this.DataTypeId );

            info.AddValue ( "DatasetReferenceTypeName" , this.DatasetReferenceTypeName );
            info.AddValue ( "DataCollectionReferenceId" , this.DataCollectionReferenceId );

            info.AddValue ( "DataDefaultValue" , this.DataDefaultValue );
            info.AddValue ( "PolicyTypeId" , this.PolicyTypeId );

            info.AddValue ( "PolicyType" , this.PolicyType );
            info.AddValue ( "ValidationRegularExpression" , this.ValidationRegularExpression );

            info.AddValue ( "Sequence" , this.Sequence );
            info.AddValue ( "Active" , this.Active );

            info.AddValue ( "Representative" , this.Representative );
        }
        #endregion
    }
}
