using System;

namespace ADS.Common.Models.Domain
{
    public class SMSMessage
    {
        public Guid Id { get; set; }
        public string CellNumber { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
        public DateTime CreationTime { get; set; }

        #region cst ...

        public SMSMessage()
        {

        }
        public SMSMessage( string cellNumber , string message , string language , DateTime creationTime )
        {
            this.Id = Guid.NewGuid();
            this.CellNumber = cellNumber;
            this.Message = message;
            this.Language = language;
            this.CreationTime = creationTime;
        }
        
        #endregion
    }
}
