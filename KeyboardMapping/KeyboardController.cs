using EliteJoystick.Common.Messages;
using System;
using System.Collections.Generic;

namespace KeyboardMapping
{
    public partial class KeyboardController : IObservable<KeyboardMessage>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<IObserver<KeyboardMessage>> observers = new List<IObserver<KeyboardMessage>>();

        public IDisposable Subscribe(IObserver<KeyboardMessage> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        public void Notify(KeyboardMessage message)
        {
            foreach (var observer in observers)
                observer.OnNext(message);
        }
    }
}
