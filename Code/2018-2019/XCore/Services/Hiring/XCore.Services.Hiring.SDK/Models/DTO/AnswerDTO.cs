using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.SDK.Models.DTO;

namespace XCore.Services.Hiring.SDK.Models.DTO
{
    public class AnswerDTO : Entity<int>
    {
        #region props
        public int QuestionId { get; set; }
        public QuestionDTO Question { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationDTO Application { get; set; }

        #endregion       
    }
}
