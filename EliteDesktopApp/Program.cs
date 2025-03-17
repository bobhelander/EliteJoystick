using EliteDesktopApp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EliteDesktopApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var host = Host.CreateDefaultBuilder()
                 .ConfigureServices((context, service) =>
                 {
                     service.AddScoped<Form1>();
                     service.AddCustomLogging();
                     service.AddJoystickService();
                     service.AddMessagingServices();
                     service.AddEliteGameServices();
                     service.AddInputControllers();
                     service.AddOutputControllers();
                     service.AddEliteApi();
                 })
                 .Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    Console.WriteLine("Starting");

                    var form1 = services.GetRequiredService<Form1>();
                    Application.Run(form1);

                    Console.WriteLine("Success");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Occured");
                }
            }

            /*
            var service = host.Services.GetRequiredService<JoystickService>();
            
            Application.Run(new Form1(service));
            service.OnStop();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var form1 = services.GetRequiredService<Form1>();
                    Application.Run(form1);

                    Console.WriteLine("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured");
                }
            }
            */
        }
    }
}