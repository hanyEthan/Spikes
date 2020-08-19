using System;
using System.Collections.Generic;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Utilities;

namespace ADS.Common.Bases.MessageQueuing.Models
{
    [Serializable]
    public class MQMessage
    {
        #region props ...

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public MQMessageStatus Status { get; set; }
        public MQMessagePriority Priority { get; set; }
        public MQMessageComplexity Complexity { get; set; }
        public DateTime CreationTime { get; set; }

        public string TargetCode { get; set; }
        public string TargetType { get; set; }

        public byte[] ContentSerialized { get; set; }
        public string ContentType { get; set; }
        
        #endregion
        #region cst

        public MQMessage()
        {
            this.Id = Guid.NewGuid();
            this.Status = MQMessageStatus.UnProcessed;
            this.Priority = MQMessagePriority.Normal;
            this.Complexity = MQMessageComplexity.Normal;
            this.CreationTime = DateTime.Now;
        }
        
        #endregion
        #region publics ...

        public T ContentGet<T>() where T : IMQMessageContent
        {
            try
            {
                return ContentSerialized == null
                       ? default( T )
                       : XSerialize.Deserialize<T>( XSerialize.Mode.BinaryProtobuf, ContentSerialized );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return default( T );
            }
        }
        public bool ContentSet<T>( T content ) where T : IMQMessageContent
        {
            try
            {
                if ( content == null ) return false;

                byte[] modelSerialized;
                //XSerialize.Serialize<T>( XSerialize.Mode.BinaryProtobuf , content , new List<string>() { this.ContentType } , out modelSerialized );
                XSerialize.Serialize<T>( XSerialize.Mode.BinaryProtobuf, content, out modelSerialized );

                this.Type = typeof( T ).ToString();
                this.ContentType = content.ContentType;
                this.ContentSerialized = modelSerialized;

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
    }
}
