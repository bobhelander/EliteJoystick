using EliteJoystick.Common.Interfaces;
using EliteJoystick.Common.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EdsmConnector
{
    public class Connector : IEdsmConnector
    {
        private const string GET_SYSTEM_BY_NAME = "/api-system-v1/bodies?systemName=";

        private readonly HttpClient client;

        private ILogger<Connector> logger;

        public Connector(ILogger<Connector> logger)
        {
            this.logger = logger;
            this.client = new HttpClient() { BaseAddress = new Uri("https://www.edsm.net") };
        }

        public async Task<StarSystem> GetSystem(string systemName)
        {
            try
            {
                var url = $"{GET_SYSTEM_BY_NAME}{systemName}";
                using (var response = await client.GetAsync(url).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var result = JsonConvert.DeserializeObject<StarSystem>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                    if (string.IsNullOrEmpty(result?.name))
                    {
                        result = new StarSystem
                        {
                            name = systemName,
                            bodyCount = -1,
                            bodies = new EliteJoystick.Common.Models.Body[0]
                        };
                    }

                    return result;
                }
            }
            catch(Exception ex)
            {
                logger.LogError("EdsmConnector error: {ex}", ex);
                return new StarSystem
                {
                    name = $"{systemName}  ERROR: {ex.Message}",
                    bodyCount = -1,
                    bodies = new EliteJoystick.Common.Models.Body[0]
                };
            }
        }
    }
}
