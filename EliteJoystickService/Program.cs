using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                    }).SetMinimumLevel(LogLevel.Debug));

            ILogger<JoystickService> log = loggerFactory.CreateLogger<JoystickService>();

            if (Environment.UserInteractive)
            {
                var service1 = new JoystickService(log, log);
                service1.TestStartupAndStop(args);
            }
            else
            {
                // Put the body of your old Main method here.  
                ServiceBase[] ServicesToRun = new ServiceBase[]
                {
                    new JoystickService(log, log)
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
