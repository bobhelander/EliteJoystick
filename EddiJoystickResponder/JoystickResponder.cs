using Eddi;
using EddiEvents;
using Newtonsoft.Json;
using System;
using System.AddIn;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EddiJoystickResponder
{
    [AddIn("EliteJoystickResponder", Version = "1.0.0.0")]
    public class JoystickResponder : EDDIResponder
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ConfigurationWindow configWindow;

        public EliteJoystickClient.Client Client { get; set; }

        public UserControl ConfigurationTabItem()
        {
            configWindow = new ConfigurationWindow { JoystickResponder = this };
            return configWindow;
        }

        public void Handle(Event theEvent)
        {
            //log.Debug($"Received event {JsonConvert.SerializeObject(theEvent)}");

            if (theEvent is JoystickCommandEvent joystickCommandEvent)
            {
                Client.HandleCommand(joystickCommandEvent.command, JoystickCommandEvent.VARIABLES).Wait();
            }

            if (theEvent is JoystickActionEvent joystickActionEvent)
            {
                EliteEventHandlers.EddiAction(Client, joystickActionEvent, JoystickActionEvent.VARIABLES);
            }

            if (theEvent is DockedEvent dockedEvent)
            {
                EliteEventHandlers.DockedEvent(Client, dockedEvent);
            }

            if (theEvent is JumpedEvent jumpedEvent)
            {
                EliteEventHandlers.JumpedEvent(Client, jumpedEvent);
            }
        }

        public string LocalizedResponderName()
        {
            return "Elite Joystick Responder";
        }

        public void Reload()
        {
            //throw new NotImplementedException();
        }

        public string ResponderDescription()
        {
            return "Joystick Responder";
        }

        public string ResponderName()
        {
            return "Joystick Responder";
        }

        public string ResponderVersion()
        {
            return "1.0";
        }

        public bool Start()
        {
            Client = new EliteJoystickClient.Client { Name = "elite_joystick_client" };
            Client.Initialize().Wait();
            return true;
        }

        public void Stop()
        {
            Client.DisconnectArduino().Wait();
            Client.DisconnectJoysticks().Wait();
        }
    }
}
