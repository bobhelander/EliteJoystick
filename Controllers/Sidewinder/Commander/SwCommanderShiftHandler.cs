using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.Commander
{
    public class SwCommanderShiftHandler: StateHandler
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
                    scController.Controller.ButtonUp += Controller_ButtonUp;
                    scController.Controller.ButtonDown += Controller_ButtonDown;
                }
            }
        }

        private void Controller_ButtonDown(object sender, Faz.SideWinderSC.Logic.ButtonEventArgs e)
        {
            SetShiftMode(e.Button);
        }

        private void SetShiftMode(Faz.SideWinderSC.Logic.SwscButton button)
        {
            if (button == Faz.SideWinderSC.Logic.SwscButton.Shift1)
            {
                scController.SharedState.ShiftStateValue = EliteSharedState.ShiftState.Shift1;
            }

            if (button == Faz.SideWinderSC.Logic.SwscButton.Shift2)
            {
                scController.SharedState.ShiftStateValue = EliteSharedState.ShiftState.Shift2;
            }

            if (button == Faz.SideWinderSC.Logic.SwscButton.Shift3)
            {
                scController.SharedState.ShiftStateValue = EliteSharedState.ShiftState.Shift3;
            }
        }

        static HashSet<Faz.SideWinderSC.Logic.SwscButton> ShiftButtons = new HashSet<Faz.SideWinderSC.Logic.SwscButton>
        {
            Faz.SideWinderSC.Logic.SwscButton.Shift1,
            Faz.SideWinderSC.Logic.SwscButton.Shift2,
            Faz.SideWinderSC.Logic.SwscButton.Shift3
        };

        private void Controller_ButtonUp(object sender, Faz.SideWinderSC.Logic.ButtonEventArgs e)
        {
            if (e.Button == Faz.SideWinderSC.Logic.SwscButton.Shift1 ||
                e.Button == Faz.SideWinderSC.Logic.SwscButton.Shift2 ||
                e.Button == Faz.SideWinderSC.Logic.SwscButton.Shift3)
            {
                var stillPressed = ShiftButtons.Intersect(scController.Controller.CurrentStatus.DownButtons);
                if (stillPressed.Any())
                {
                    foreach (var button in stillPressed)
                        SetShiftMode(button);
                }
                else
                {
                    scController.SharedState.ShiftStateValue = EliteSharedState.ShiftState.None;
                }
            }
        }
    }
}
