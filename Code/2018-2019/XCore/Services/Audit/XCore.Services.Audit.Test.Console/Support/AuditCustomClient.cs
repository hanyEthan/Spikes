using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Framework.ServiceBus.MST.Support;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Audit.SDK.Client;
using XCore.Services.Audit.SDK.Contracts;
using XCore.Services.Audit.SDK.Handlers;
using XCore.Services.Audit.SDK.Models;
using XCore.Services.Configurations.SDK.Client;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Framework.Framework.ServiceBus.Handlers;
namespace XCore.Services.Audit.Test.Console.Support
{
    public class AuditCustomClient
    {
        public IAuditClient SDK { get; protected set; }

        public AuditCustomClient()
        {
            Initialize();
        }

        private void Initialize()
        {
            IConfigClient configClient = new ConfigClient(null);
            IConfigurationRoot builder = new ConfigurationBuilder()
                                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            var ServiceProvider = new ServiceBusLocalConfigProvider(builder);
            var ServiceBus = new ServiceBus(ServiceProvider);

            SDK = new AuditClient(null, ServiceBus, null);
        }
    }
}
