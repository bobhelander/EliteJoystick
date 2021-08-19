using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            // Read in appsettings.json.  Not used, just don't want to forget how to do this.
            var config = new ConfigurationBuilder()
             .SetBasePath(System.IO.Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();

            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options => {options.SingleLine = true; options.TimestampFormat = "hh:mm:ss ";}).SetMinimumLevel(LogLevel.Debug)
                    .AddFile(
                        pathFormat: "%LOCALAPPDATA%/EliteJoystick/log/InGame2{Date:yyyy}.log",
                        outputTemplate: "{Message}{NewLine}",
                        minimumLevel: LogLevel.Information).SetMinimumLevel(LogLevel.Debug));

            ILogger<JoystickService> log = loggerFactory.CreateLogger<JoystickService>();

            log.LogInformation("Testing");

            // Are we running as a service or command line
            if (Environment.UserInteractive)
            {
                var service1 = new JoystickService(log, log);
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun = new ServiceBase[]
                {
                    new JoystickService(log, log)
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
