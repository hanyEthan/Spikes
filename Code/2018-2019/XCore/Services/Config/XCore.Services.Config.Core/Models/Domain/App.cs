using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Config.Core.Models.Domain
{
    public class App : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public List<Module> modulelist { get; set; }

        #endregion
    }
}
