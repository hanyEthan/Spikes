using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.Core.Models.Relations
{
    public class ActorRole
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
