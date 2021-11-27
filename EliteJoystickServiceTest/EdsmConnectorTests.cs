using EdsmConnector;
using EliteJoystick.Common.Interfaces;
using EliteJoystick.Common.Logic;
using EliteJoystickService.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vKeyboard;

namespace EliteJoystickServiceTest
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class EdsmConnectorTests
    {
        [TestMethod]
        public void TestServiceAvailable()
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((context, service) =>
             {
                 service.AddCustomLogging();
                 service.AddEliteGameServices();
             }).Build();

            Assert.IsNotNull(host.Services.GetService<IEdsmConnector>());
        }

        [TestMethod]
        public Task TestGetSystem_0() => TestGetSystem("Maia");

        [TestMethod]
        public Task TestGetSystem_1() => TestGetSystem("Sol");

        [TestMethod]
        public Task TestGetSystem_2() => TestGetSystem("Hypoae Aewsy CG-F d11-4");

        private async Task TestGetSystem(string systemName)
        {
            var edsmConnector = new Connector(new NullLogger<Connector>());

            var result = await edsmConnector.GetSystem(systemName);
            Assert.AreEqual(result.name, systemName);
        }
    }
}
