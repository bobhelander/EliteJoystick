using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping
{
    public class SwCommanderProfileHandler : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(States value)
        {
            var current = value.Current as State;
            //current.Profile;
        }

        //private void Controller_ProfileChanged(object sender, Faz.SideWinderSC.Logic.ProfileChangedEventArgs e)
        //{
        //    scController.Profile = e.Profile;
        //    log.Debug($"Profile changed to {scController.Profile}");

        //    if (1 == scController.Profile)
        //    {
        //        log.Debug("Entering Program Mode");
        //        scController.ProgramLights();
        //    }
        //    else
        //    {
        //        scController.vJoyMapper.Save();
        //    }
        //}
    }
}
