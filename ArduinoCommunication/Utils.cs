using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ArduinoCommunication
{
    public static class Utils
    {
        public static void TypeFullString(Arduino arduino, String text, EventWaitHandle finishedEvent)
        {
            new TextBuffer
            {
                Delay = 40,
                Text = String.IsNullOrEmpty(text) ? Utils.CallClipboard() : text,
                Arduino = arduino,
                FinishedEvent = finishedEvent,
                Newline = true,
            }.Start();
        }

        public static void KeyCombo(Arduino arduino, byte[] modifier, byte key)
        {
            new KeyCombo
            {
                Delay = 150,
                Modifier = modifier,
                Key = key,
                Arduino = arduino
            }.Start();
        }

        static public string CallClipboard()
        {
            object returnValue = null;  
            var t = new Thread((ThreadStart)(() =>
            {
                returnValue = System.Windows.Clipboard.GetText();
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return returnValue as String;
        }
    }
}
