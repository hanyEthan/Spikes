using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.Core.Models.Relations
{
    public class ActorPrivilege
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public int PrivilegeId { get; set; }
        public Privilege Privilege { get; set; }
    }
}
