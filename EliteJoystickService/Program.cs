using System;
using System.ServiceProcess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using EliteJoystickService.Extensions;
using EliteAPI;

namespace EliteJoystickService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            // Inject the logger and EliteApi services into host
            var host = Host.CreateDefaultBuilder()
                 .ConfigureServices((context, service) =>
                 {
                     service.AddCustomLogging();
                     service.AddJoystickService();
                     service.AddMessagingServices();
                     service.AddEliteGameServices();
                     service.AddInputControllers();
                     service.AddOutputControllers();
                     service.AddEliteAPI();
                 })
                 .Build();

            // Are we running as a service or command line?
            if (Environment.UserInteractive)
            {
                var service = host.Services.GetRequiredService<JoystickService>();
                service.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun = new ServiceBase[]
                {
                    host.Services.GetRequiredService<JoystickService>(),
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
