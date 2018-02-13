using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleClearMessages: StateHandler
    {
        private Timer timer;
        private int index;

        public long Delay { get; set; }
        public uint vButton { get; set; }

        AutoResetEvent finishedEvent = new AutoResetEvent(true);

        private TmThrottleController tmThrottleController;

        public TmThrottleController TmThrottleController
        {
            get { return tmThrottleController; }
            set
            {
                tmThrottleController = value;
                if (null != tmThrottleController)
                {
                    tmThrottleController.Controller.SwitchState += async (s, e) =>
                        await Task.Run(() => ControllerSwitchState(s, e));
                }
            }
        }

        private void ControllerSwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
            var button14 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button14;
            if (((e.Buttons & button14) != 0) &&
                ((e.PreviousButtons & button14) == 0)) 
            {
                if (null == timer)
                    Activate();
                //tmThrottleController.VisualState.UpdateMessage("Clear Message Log: Running");
            }
        }

        public void Activate()
        {
            index = 0;
            timer = new Timer(new TimerCallback(Action), null, Delay, Timeout.Infinite);
        }

        public void Disable()
        {
            if (null != timer)
            {
                var temp = timer;
                timer = null;
                temp.Dispose();
                tmThrottleController.ReleaseAllKeys();
                index = 0;
            }
        }

        public virtual void Action(object o)
        {
            switch (index)
            {
                // Click to enter message window
                case 0:
                    tmThrottleController.SetJoystickButton(true, vButton, vJoyTypes.Virtual);
                    break;
                case 1:
                    tmThrottleController.SetJoystickButton(false, vButton, vJoyTypes.Virtual);
                    break;
                case 2:
                    tmThrottleController.DepressKey(0x2f); // "/"
                    break;
                case 3:
                    tmThrottleController.ReleaseKey(0x2f);
                    break;
                case 4:
                    tmThrottleController.DepressKey(0x63); // "c"
                    break;
                case 5:
                    tmThrottleController.ReleaseKey(0x63);
                    break;
                case 6:
                    tmThrottleController.DepressKey(0x6C); // "l"
                    break;
                case 7:
                    tmThrottleController.ReleaseKey(0x6C);
                    break;
                case 8:
                    tmThrottleController.DepressKey(0x65); // "e"
                    break;
                case 9:
                    tmThrottleController.ReleaseKey(0x65);
                    break;
                case 10:
                    tmThrottleController.DepressKey(0x61); // "a"
                    break;
                case 11:
                    tmThrottleController.ReleaseKey(0x61);
                    break;
                case 12:
                    tmThrottleController.DepressKey(0x72); // "r"
                    break;
                case 13:
                    tmThrottleController.ReleaseKey(0x72);
                    break;
                case 14:
                    tmThrottleController.DepressKey(0xB0); // "enter"
                    break;
                case 15:
                    tmThrottleController.ReleaseKey(0xB0);
                    break;
                case 16:
                    Disable();
                    //tmThrottleController.VisualState.UpdateMessage("Clear Message Log: Done");
                    return;
            }
            index++;
            timer.Change(Delay, Timeout.Infinite);
        }
    }
}
