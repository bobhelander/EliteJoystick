using EliteGameStatus.Services;
using Microsoft.Extensions.DependencyInjection;

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
