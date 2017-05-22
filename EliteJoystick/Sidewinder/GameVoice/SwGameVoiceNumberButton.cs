using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.GameVoice
{
    class SwGameVoiceNumberButton : SwGameVoiceButton
    {
        public uint vButtonId { get; set; }

        public override void Controller_ButtonUp(object sender, SwgvButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                State = false;
                SwGvController.SetJoystickButton(State, vButtonId, vJoyTypes.Voice);
            }
        }

        public override void Controller_ButtonDown(object sender, SwgvButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                State = true;
                SwGvController.SetJoystickButton(State, vButtonId, vJoyTypes.Voice);
            }
        }
    }
}
