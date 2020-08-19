using Topshelf;

namespace XCore.Services.Config.API.Infrastructure.Host
{
    public static class ServiceHostWin
    {
        public static void Config()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<ServiceHostWeb>(x =>
                {
                    x.ConstructUsing(s => new ServiceHostWeb(ServiceConfig.ServiceSettings.Port));
                    x.WhenStarted(s => s.Start());
                    x.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();
                configure.SetServiceName(ServiceConfig.ServiceSettings.Name ?? "XCore.Service");
                configure.SetDisplayName(ServiceConfig.ServiceSettings.Display ?? "XCore.Service");
                configure.SetDescription(ServiceConfig.ServiceSettings.Description ?? "XCore.Service");
            });
        }
    }
}
