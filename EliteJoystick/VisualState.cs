using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EliteJoystick
{
    public class VisualState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public String Name { get; set; }
        public bool Initialized { get; set; }
        private String message;
        public String Message
        {
            get { return message; }
            set
            {
                message = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Message"));
            }
        }

        private UInt32 buttonStates;
        public UInt32 ButtonStates
        {
            get { return buttonStates; }
            set
            {
                buttonStates = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ButtonStates"));
            }
        }

        private UInt32 axis1;
        public UInt32 Axis1
        {
            get { return axis1; }
            set
            {
                axis1 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Axis1"));
            }
        }

        private UInt32 axis2;
        public UInt32 Axis2
        {
            get { return axis2; }
            set
            {
                axis2 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Axis2"));
            }
        }

        private UInt32 axis3;
        public UInt32 Axis3
        {
            get { return axis3; }
            set
            {
                axis3 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Axis3"));
            }
        }

        public void UpdateMessage(String message)
        {
            Dispatcher.CurrentDispatcher.Invoke((Action)delegate () { Message = message; });
        }

        public void UpdateButtons(UInt32 buttons)
        {
            Dispatcher.CurrentDispatcher.Invoke((Action)delegate () { ButtonStates = buttons; });
        }

        public void UpdateAxis1(UInt32 value)
        {
            Dispatcher.CurrentDispatcher.Invoke((Action)delegate () { Axis1 = value; });
        }

        public void UpdateAxis2(UInt32 value)
        {
            Dispatcher.CurrentDispatcher.Invoke((Action)delegate () { Axis2 = value; });
        }

        public void UpdateAxis3(UInt32 value)
        {
            Dispatcher.CurrentDispatcher.Invoke((Action)delegate () { Axis3 = value; });
        }
    }
}
