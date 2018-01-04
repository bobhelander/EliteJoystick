﻿using Eddi;
using EddiEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EddiJoystickResponder
{
    public class JoystickResponder : EDDIResponder
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ConfigurationWindow configWindow;

        public EliteJoystickClient.Client Client { get; set; }

        public UserControl ConfigurationTabItem()
        {
            configWindow = new ConfigurationWindow { JoystickResponder = this };
            return configWindow;
        }

        public void Handle(Event theEvent)
        {
            log.Debug($"Received event {JsonConvert.SerializeObject(theEvent)}");

            if (theEvent.type == "CONNECT_JOYSTICKS")
            {
                Client.ConnectArduino();
                Client.ConnectJoysticks();
            }

            if (theEvent is JoystickCommandEvent)
            {
                Client.HandleCommand(((JoystickCommandEvent)theEvent).command);
            }

            if (theEvent is DockedEvent)
            {
                EventHandlers.DockedEvent(Client, (DockedEvent)theEvent);
            }
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
            Client.Initialize();
            return true;
        }

        public void Stop()
        {
        }
    }
}
