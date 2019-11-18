using EliteJoystick.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeyboardMapping
{
    public partial class KeyboardController
    {
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<KeyboardMessage>> _observers;
            private IObserver<KeyboardMessage> _observer;

            public Unsubscriber(List<IObserver<KeyboardMessage>> observers, IObserver<KeyboardMessage> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
