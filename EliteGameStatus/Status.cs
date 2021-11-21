using EliteAPI.Abstractions;
using EliteAPI.Event.Models.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EliteGameStatus
{
    public class Status : IObservable<IEvent>, IDisposable
    {
        private ILogger logger { get; }
        private bool Initialized { get; }

        private readonly List<IObserver<IEvent>> observers = new List<IObserver<IEvent>>();

        public IEliteDangerousApi EliteAPI { get; set; }

        public Status(IEliteDangerousApi eliteApi, ILogger logger)
        {
            this.logger = logger;
            EliteAPI = eliteApi;
            try
            {
                Initialized = true;
                EliteAPI.Events.AllEvent += Events_AllEvent;

                EliteAPI.Events.StartJumpEvent += Events_StartJumpEvent;
                EliteAPI.Events.FssAllBodiesFoundEvent += Events_FSSAllBodiesFoundEvent;
                EliteAPI.Events.ScanEvent += Events_ScanEvent;
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.Message);
            }
        }

        private void Events_ScanEvent(object sender, EliteAPI.Event.Models.ScanEvent e)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_FSSAllBodiesFoundEvent(object sender, EliteAPI.Event.Models.FssAllBodiesFoundEvent e)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_StartJumpEvent(object sender, EliteAPI.Event.Models.StartJumpEvent e)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private static void Process(IEvent e, List<IObserver<IEvent>> observers)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_AllEvent(object sender, dynamic e)
        {
            if (e is IEvent)
                Process(e, observers);
        }

        public IDisposable Subscribe(IObserver<IEvent> observer)
        {
            lock (observers)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);
            }

            return new Unsubscriber(observers, observer);
        }

        public void Dispose()
        {
            if (Initialized)
            {
                EliteAPI?.StopAsync().Wait();
                EliteAPI = null;
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<IEvent>> _observers;
            private readonly IObserver<IEvent> _observer;

            public Unsubscriber(List<IObserver<IEvent>> observers, IObserver<IEvent> observer)
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
