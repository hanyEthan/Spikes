using System;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Models
{
    public interface IEntity
    {
        object Id { get; set; }
        string Code { get; set; }
        bool IsActive { get; set; }
        DateTime? CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        string MetaData { get; set; }
    }
    public interface IEntity<T> : IEntity
    {
        new T Id { get; set; }
    }
}
