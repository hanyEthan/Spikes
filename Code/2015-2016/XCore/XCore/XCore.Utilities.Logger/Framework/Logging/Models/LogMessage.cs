using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Entities.Repositories.Models;

namespace XCore.Utilities.Logger.Framework.Logging.Models
{
    public class LogMessage : Entity<Guid>
    {
        public string Thread { get; set; }
        public string Level { get; set; }
        public int LevelCode { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string AppId { get; set; }
        public string ModuleId { get; set; }
        public string Exception { get; set; }
        public string Context { get; set; }
        public DateTime LogDate { get; set; }
    }
}
