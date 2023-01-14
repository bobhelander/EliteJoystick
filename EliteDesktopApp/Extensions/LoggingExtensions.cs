using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickService.Extensions
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// I am configuring two different console loggers. One is the standard console, and the other, is
        /// outputting to an explorer log that will output the valueable systems to scan.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            services.AddLogging(builder =>
                        builder                            
                            //.AddSimpleConsole()
                            .AddFile(
                                minimumLevel: LogLevel.Debug,
                                pathFormat: "%LOCALAPPDATA%/EliteJoystick/log/EliteJoystick.log",
                                levelOverrides: new Dictionary<string, LogLevel> {
                                    { "EliteGameStatus.Services.ExplorationService", LogLevel.None },
                                })
                            .AddFile(
                                minimumLevel: LogLevel.None,
                                pathFormat: "%LOCALAPPDATA%/EliteJoystick/log/InGame.log",
                                outputTemplate: "{Message}{NewLine}",
                                levelOverrides: new Dictionary<string, LogLevel> {
                                    { "EliteJoystickService.JoystickService", LogLevel.None },
                                    { "EliteGameStatus.Services.ExplorationService", LogLevel.Information },
                                })
                            .AddSimpleConsole(options => { options.SingleLine = true; options.TimestampFormat = "hh:mm:ss "; })
                            .SetMinimumLevel(LogLevel.Debug));

            return services;
        }
    }
}