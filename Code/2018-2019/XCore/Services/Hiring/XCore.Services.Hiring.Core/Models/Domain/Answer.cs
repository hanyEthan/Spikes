using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Answer : Entity<int>
    {
        #region props
        public virtual int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual int ApplicationId { get; set; }
        public virtual Application Application { get; set; }

        #endregion       
    }
}
