using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EdsmConnector
{
    public static class Connector
    {
        private const string GET_SYSTEM_BY_NAME = "/api-system-v1/bodies?systemName=";

        private static readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://www.edsm.net") };

        public static async Task<System> GetSystem(string systemName)
        {
            try
            {
                var url = $"{GET_SYSTEM_BY_NAME}{systemName}";
                using (var response = await client.GetAsync(url).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var result = JsonConvert.DeserializeObject<System>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                    if (string.IsNullOrEmpty(result?.name))
                    {
                        result = new EdsmConnector.System
                        {
                            name = systemName,
                            bodyCount = -1,
                            bodies = new EdsmConnector.Body[0]
                        };
                    }

                    return result;
                }
            }
            catch(Exception ex)
            {
                return new EdsmConnector.System
                {
                    name = $"{systemName}  ERROR: {ex.Message}",
                    bodyCount = -1,
                    bodies = new EdsmConnector.Body[0]
                };
            }
        }
    }
}
