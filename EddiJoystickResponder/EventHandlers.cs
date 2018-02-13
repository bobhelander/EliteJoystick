using EddiDataDefinitions;
using EddiDataProviderService;
using EddiEvents;
using EliteJoystickClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddiJoystickResponder
{
    public static class EventHandlers
    {
        public static void DockedEvent(Client client, DockedEvent actionEvent)
        {
            
        }

        public static void JumpedEvent(Client client, JumpedEvent actionEvent)
        {
            // We have entered a new system
            StarSystem starSystem = StarSystemSqLiteRepository.Instance.GetOrCreateStarSystem(actionEvent.system, true);
            var page = Exploration.Actions.OutputValuableSystems(client, starSystem);

            var local_files_base = "http://127.0.0.1:8080";
            var httpPage = $"{local_files_base}/{Path.GetFileName(page)}";

            client.Navigate(httpPage);
        }
    }
}
