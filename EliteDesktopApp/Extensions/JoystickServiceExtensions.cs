﻿using EliteDesktopApp.Services;
using EliteGameStatus.Extensions;
using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using vJoyMapping.Common;

namespace EliteDesktopApp.Extensions
{
    public static class JoystickServiceExtensions
    {
        /// <summary>
        /// Add the Windows Service and settings
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJoystickService(this IServiceCollection services)
        {
            services.AddSingleton<JoystickService>();
            services.AddSingleton<ISettings>(serviceProvider => Settings.Load());
            return services;
        }

        /// <summary>
        /// Add all of the controllers that are currently connected to the simpit and are used as inputs into the service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInputControllers(this IServiceCollection services)
        {
            services.AddScoped<vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Controller>();      // msffb2: Joystick controller connection
            services.AddScoped<vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Controller>();
            services.AddScoped<vJoyMapping.Thrustmaster.Warthog.Throttle.Controller>();
            services.AddScoped<vJoyMapping.CHProducts.ProPedals.Controller>();
            services.AddScoped<vJoyMapping.LeoBodnar.BBI32.Controller>();
            services.AddScoped<vJoyMapping.Pioneer.ddjsb2.Controller>();

            // The complete set of controllers service
            services.AddTransient<ControllersService>();

            return services;
        }

        /// <summary>
        /// Add the controllers that will output to the games or other services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOutputControllers(this IServiceCollection services)
        {
            services.AddScoped<IForceFeedbackController, ForceFeedBackController.Controller>();    // msffb2: Force feedback connection
            services.AddScoped<IVirtualJoysticks, EliteVirtualJoysticks>();
            //services.AddScoped<IKeyboard, ArduinoCommunication.Arduino>();
            services.AddScoped<IKeyboard, vKeyboard.Keyboard>();
            services.AddScoped<IVoiceMeeterService, VoiceMeeterService>();
            return services;
        }

        /// <summary>
        /// Add the services that help with the game's state
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEliteGameServices(this IServiceCollection services)
        {
            services.AddGameServices();
            services.AddSingleton<IEdsmConnector, EdsmConnector.Connector>();
            services.AddSingleton<EliteSharedState>();
            return services;
        }

        private static void AddGameServices()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The services that help with the IPC messaging
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMessagingServices(this IServiceCollection services)
        {
            services.AddSingleton<MessageHandlingService>();
            services.AddSingleton<IpcService>();
            return services;
        }
    }
}
