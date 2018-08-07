using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.GameVoice
{
    public class SwGameLandingGearHandler : StateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SwGvController swGvController;
        public SwGvController SwGvController
        {
            get { return swGvController; }
            set
            {
                swGvController = value;
                if (null != swGvController)
                {
                    swGvController.Controller.ButtonsChanged += async (s, e) =>
                        await Task.Run(() => ControllerButtonsChanged(s, e));
                }
            }
        }

        static private byte button1 = (byte)SwgvButton.Button1;

        private void ControllerButtonsChanged(object sender, Faz.SideWinderSC.Logic.SwgvButtonStateEventArgs e)
        {
            if (0 == (e.PreviousButtonsState & button1) && (e.ButtonsState & button1) == button1)
            {
                // On
                swGvController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200);
                log.Debug($"Virtual: Landing Gear: Deployed");
            }
            else if (button1 == (e.PreviousButtonsState & button1) && 0 == (e.ButtonsState & button1))
            {
                // Off
                swGvController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200);
                log.Debug($"Virtual: Landing Gear: Retracted");
            }
        }
    }
}
