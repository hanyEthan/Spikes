namespace ADS.Common.Bases.MessageQueuing.Models
{
    public enum MQMessageStatus
    {
        UnProcessed = 0 ,
        InProcess = 1 ,
        MarkedForDeletion = 2 ,
    }
}
