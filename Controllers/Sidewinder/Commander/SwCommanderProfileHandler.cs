using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.Commander
{
    public class SwCommanderProfileHandler: StateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ScController scController;

        public ScController ScController
        {
            get { return scController; }
            set
            {
                scController = value;
                if (null != scController)
                {
                    scController.Controller.ProfileChanged += Controller_ProfileChanged;
                    log.Debug("connected");
                }
            }
        }

        private void Controller_ProfileChanged(object sender, Faz.SideWinderSC.Logic.ProfileChangedEventArgs e)
        {
            scController.Profile = e.Profile;
            log.Debug($"Profile changed to {scController.Profile}");

            if (1 == scController.Profile)
            {
                log.Debug("Entering Program Mode");
                scController.ProgramLights();
            }
            else
            {
                scController.vJoyMapper.Save();
            }
        }


        static HashSet<Faz.SideWinderSC.Logic.SwscButton> ShiftButtons = new HashSet<Faz.SideWinderSC.Logic.SwscButton>
        {
            Faz.SideWinderSC.Logic.SwscButton.Shift1,
            Faz.SideWinderSC.Logic.SwscButton.Shift2,
            Faz.SideWinderSC.Logic.SwscButton.Shift3
        };
    }
}
