using EliteAPI;
using EliteJoystick.Common.EliteGame;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text;

namespace EliteGameStatus
{
    public class Status : IObservable<EliteAPI.Events.IEvent>, IDisposable
    {
        private ILogger logger { get; }
        private bool Initialized { get; } = false;
        private readonly List<IObserver<EliteAPI.Events.IEvent>> observers = new List<IObserver<EliteAPI.Events.IEvent>>();

        public EliteDangerousAPI EliteAPI { get; set; }

        public Status(ILogger logger)
        {
            this.logger = logger;
            EliteAPI = new EliteDangerousAPI();
            try
            {
                EliteAPI.Start();
                Initialized = true;
                EliteAPI.Events.AllEvent += Events_AllEvent;

                EliteAPI.Events.StartJumpEvent += Events_StartJumpEvent;
                EliteAPI.Events.FSSAllBodiesFoundEvent += Events_FSSAllBodiesFoundEvent;
                EliteAPI.Events.ScanEvent += Events_ScanEvent;
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.Message);
            }
        }

        private void Events_ScanEvent(object sender, EliteAPI.Events.ScanInfo e)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_FSSAllBodiesFoundEvent(object sender, EliteAPI.Events.FSSAllBodiesFoundInfo e)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_StartJumpEvent(object sender, EliteAPI.Events.StartJumpInfo e)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private static void Process(EliteAPI.Events.IEvent e, List<IObserver<EliteAPI.Events.IEvent>> observers)
        {
            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_AllEvent(object sender, dynamic e)
        {
            if (e is EliteAPI.Events.IEvent)
                Process(e, observers);
        }

        public IDisposable Subscribe(IObserver<EliteAPI.Events.IEvent> observer)
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
                EliteAPI?.Stop();
                EliteAPI = null;
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<EliteAPI.Events.IEvent>> _observers;
            private readonly IObserver<EliteAPI.Events.IEvent> _observer;

            public Unsubscriber(List<IObserver<EliteAPI.Events.IEvent>> observers, IObserver<EliteAPI.Events.IEvent> observer)
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
