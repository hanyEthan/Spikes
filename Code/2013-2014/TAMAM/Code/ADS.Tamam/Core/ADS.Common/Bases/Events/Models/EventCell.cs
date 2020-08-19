using System;
using ADS.Common.Bases.Events.Contracts;
using ADS.Common.Contracts;

namespace ADS.Common.Bases.Events.Models
{
    [Serializable]
    public abstract class EventCell : IEventCell , IXSerializable
    {
        #region props ...

        public Guid Id { get; set; }
        
        #endregion
        #region cst ...

        public EventCell()
        {
            //Id = Guid.NewGuid();
        }
        
        #endregion

        public abstract bool Process();

        [XDontSerialize] public abstract string ContentType { get; }
        [XDontSerialize] public abstract string TargetId { get; }
        [XDontSerialize] public abstract string TargetType { get; }
    }
}
