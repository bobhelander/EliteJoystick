using EliteGameStatus.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EliteGameStatus.Extensions
{
    public static class GameStatusExtensions
    {
        public static IServiceCollection AddGameServices(this IServiceCollection services)
        {
            services.AddSingleton<GameService>();
            services.AddSingleton<ExplorationService>();
            return services;
        }
    }
}
