using System.Collections.Generic;

namespace domain.Support
{
    public class BaseResponse
    {
        public bool IsValidRequest { get; set; } = true;
        public List<string> Messages { get; set; } = new List<string>();

        public object Content { get; set; }
    }
}
