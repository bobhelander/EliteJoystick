using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.Commander
{
    public class SwCommanderButtonsStateHandler : StateHandler
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
                    scController.Controller.ButtonsChanged += async (s, e) =>
                        await Task.Run(() => ControllerButtonsChanged(s, e));
                }
            }
        }

        private void ControllerButtonsChanged(object sender, Faz.SideWinderSC.Logic.ButtonsEventArgs e)
        {
        }
    }
}
