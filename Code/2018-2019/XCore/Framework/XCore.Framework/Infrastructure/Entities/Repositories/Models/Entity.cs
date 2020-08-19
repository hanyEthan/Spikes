using System;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Models
{
    [Serializable]
    public abstract class Entity<T> : IEntity<T>
    {
        #region props.

        public virtual T Id { get; set; }
        object IEntity.Id { get { return this.Id; } set { this.Id = ( T ) Convert.ChangeType( value , typeof( T ) ); } }

        public virtual string Code { get; set; } = Guid.NewGuid().ToString();

        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }

        public virtual bool IsActive { get; set; } = true;

        private DateTime? createdDate;
        public virtual DateTime? CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }
        public virtual DateTime? ModifiedDate { get; set; }

        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual string MetaData { get; set; }

        #endregion
        #region cst.

        public Entity()
        {
            
        }

        #endregion
    }
}
