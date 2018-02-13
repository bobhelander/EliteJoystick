using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.GameVoice
{
    public class SwGameResetHandler : StateHandler
    {
        private Timer timer;
        private byte oldState;

        public long Delay { get; set; }
        public uint vButton { get; set; }

        AutoResetEvent finishedEvent = new AutoResetEvent(true);

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

        private void ControllerButtonsChanged(object sender, Faz.SideWinderSC.Logic.SwgvButtonStateEventArgs e)
        {
            var muteButton = (uint)Faz.SideWinderSC.Logic.SwgvButton.Button1;
            if ((e.ButtonsState & muteButton) == muteButton &&
                (e.PreviousButtonsState & muteButton) == 0)
            {
                if (null == timer)
                {
                    oldState = e.PreviousButtonsState;
                    Activate();
                }
            }
        }

        public void Activate()
        {
            timer = new Timer(new TimerCallback(Action), null, Delay, Timeout.Infinite);
        }

        public void Disable()
        {
            if (null != timer)
            {
                var temp = timer;
                timer = null;
                temp.Dispose();
            }
        }

        public virtual void Action(object o)
        {
            SwGvController.Controller.Lights = oldState;
            Disable();
            return;
        }
    }
}
