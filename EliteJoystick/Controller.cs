using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using vJoyInterfaceWrap;

namespace EliteJoystick
{
    public class Controller 
    {
        public virtual void Initialize() { }

        public vJoy vJoy { get; set; }
        public vJoyMapper vJoyMapper { get; set; }
        //public String vJoyType0 { get; set; }
        //public String vJoyType1 { get; set; }
        //public String vJoyType2 { get; set; }

        public VisualState VisualState { get; set; }

        public bool Use { get; set; }

        public ArduinoCommunication.Arduino Arduino { get; set; }

        public void SetJoystickButton(bool down, uint vButton, string vJoyType)
        {
            vJoy.SetBtn(down, vJoyMapper.GetJoystickId(vJoyType), vButton);
        }

        public void SetJoystickAxis(int value, HID_USAGES usage, string vJoyType)
        {
            vJoy.SetAxis(value, vJoyMapper.GetJoystickId(vJoyType), usage);
        }

        public void SetJoystickHat(int value, string vJoyType, uint hatNumber)
        {
            vJoy.SetDiscPov(value, vJoyMapper.GetJoystickId(vJoyType), hatNumber);
        }

        public void DepressKey(byte key)
        {
            Arduino?.DepressKey(key);
        }

        public void ReleaseKey(byte key)
        {
            Arduino?.ReleaseKey(key);
        }

        public void ReleaseAllKeys()
        {
            Arduino?.ReleaseAll();
        }

        public void TypeFullString(String text, System.Threading.EventWaitHandle finishedEvent)
        {
            ArduinoCommunication.Utils.TypeFullString(Arduino, text, finishedEvent);
        }

        public void SendKeyCombo(byte[] modifier, byte key)
        {
            ArduinoCommunication.Utils.KeyCombo(Arduino, modifier, key);
        }

        public class ActivateButtonClass
        {
            public Timer Timer { get; set; }
            public bool Pressed { get; set; }
            public vJoy vJoy { get; set; }
            public uint vJoyId { get; set; }            
            public uint vButton { get; set; }
            public long Delay { get; set; }
        }

        public void CallActivateButton(string vJoyType, uint vButton, long delay)
        {
            uint joyId = vJoyMapper.GetJoystickId(vJoyType);

            var activateButton = new ActivateButtonClass
            {
                vJoy = vJoy,
                vJoyId = joyId,
                vButton = vButton,
                Delay = delay
            };

            activateButton.Timer = new Timer(new TimerCallback(Action), activateButton, 0, Timeout.Infinite);
        }

        public void Disable(ActivateButtonClass activateButton)
        {
            if (null != activateButton.Timer)
            {
                var temp = activateButton.Timer;
                activateButton.Timer = null;
                temp.Dispose();
                activateButton.vJoy.SetBtn(false, activateButton.vJoyId, activateButton.vButton);
                activateButton.Pressed = false;
            }
        }

        public virtual void Action(object o)
        {
            var activateButton = o as ActivateButtonClass;
            activateButton.Pressed = !activateButton.Pressed;
            activateButton.vJoy.SetBtn(activateButton.Pressed, activateButton.vJoyId, activateButton.vButton);

            if (activateButton.Pressed)
            {
                activateButton.Timer.Change(activateButton.Delay, Timeout.Infinite);
                return;
            }

            Disable(activateButton);
        }
    }
}
