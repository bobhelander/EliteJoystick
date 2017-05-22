using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.Commander
{
    public class SwCommanderProfileHandler: StateHandler
    {
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
                }
            }
        }

        private void Controller_ProfileChanged(object sender, Faz.SideWinderSC.Logic.ProfileChangedEventArgs e)
        {
            var test = e.Profile;     
        }


        static HashSet<Faz.SideWinderSC.Logic.SwscButton> ShiftButtons = new HashSet<Faz.SideWinderSC.Logic.SwscButton>
        {
            Faz.SideWinderSC.Logic.SwscButton.Shift1,
            Faz.SideWinderSC.Logic.SwscButton.Shift2,
            Faz.SideWinderSC.Logic.SwscButton.Shift3
        };
    }
}
