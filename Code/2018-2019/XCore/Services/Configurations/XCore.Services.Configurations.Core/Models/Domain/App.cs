using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Configurations.Core.Models.Domain
{
    public class App : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public virtual IList<Module> Modules { get; set; }

        #endregion
        #region cst.

        public App()
        {
            this.Modules = new List<Module>();
        }

        #endregion
    }
}
