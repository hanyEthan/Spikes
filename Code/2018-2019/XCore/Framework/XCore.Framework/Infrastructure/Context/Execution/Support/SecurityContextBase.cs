namespace XCore.Framework.Infrastructure.Context.Execution.Support
{
    public class SecurityContextBase
    {
        public bool? Authenticated { get; set; }
        public bool? Authorized { get; set; }
    }
}
