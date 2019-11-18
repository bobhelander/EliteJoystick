using EliteAPI;
using EliteJoystick.Common.EliteGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text;

namespace EliteGameStatus
{
    public class Status : IObservable<EliteAPI.Events.StatusEvent>
    {
        private List<IObserver<EliteAPI.Events.StatusEvent>> observers = new List<IObserver<EliteAPI.Events.StatusEvent>>();

        public EliteDangerousAPI EliteAPI { get; }

        public Status()
        {
            EliteAPI = new EliteDangerousAPI();
            EliteAPI.Start();
            EliteAPI.Events.AllEvent += Events_AllEvent;
        }

        private static void Process(EliteAPI.Events.StatusEvent e, List<IObserver<EliteAPI.Events.StatusEvent>> observers)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_AllEvent(object sender, dynamic e)
        {
            if (e is EliteAPI.Events.StatusEvent)
            {
                Process(e, observers);
            }
        }

        public IDisposable Subscribe(IObserver<EliteAPI.Events.StatusEvent> observer)
        {
            lock (observers)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);
            }

            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<EliteAPI.Events.StatusEvent>> _observers;
            private IObserver<EliteAPI.Events.StatusEvent> _observer;

            public Unsubscriber(List<IObserver<EliteAPI.Events.StatusEvent>> observers, IObserver<EliteAPI.Events.StatusEvent> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                lock (_observers)
                {
                    if (_observer != null && _observers.Contains(_observer))
                        _observers.Remove(_observer);
                }
            }
        }
    }
}
