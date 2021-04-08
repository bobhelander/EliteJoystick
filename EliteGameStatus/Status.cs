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
    public class Status : IObservable<EliteAPI.Events.IEvent>
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<IObserver<EliteAPI.Events.IEvent>> observers = new List<IObserver<EliteAPI.Events.IEvent>>();

        public EliteDangerousAPI EliteAPI { get; }

        public Status()
        {
            EliteAPI = new EliteDangerousAPI();
            try
            {
                EliteAPI.Start();
                EliteAPI.Events.AllEvent += Events_AllEvent;

                EliteAPI.Events.StartJumpEvent += Events_StartJumpEvent;
                EliteAPI.Events.FSSAllBodiesFoundEvent += Events_FSSAllBodiesFoundEvent;
                EliteAPI.Events.ScanEvent += Events_ScanEvent;
            }
            catch(Exception ex)
            {
                ;
            }
        }

        private void Events_ScanEvent(object sender, EliteAPI.Events.ScanInfo e)
        {
            log.Debug($"ScanEvent Detected");

            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_FSSAllBodiesFoundEvent(object sender, EliteAPI.Events.FSSAllBodiesFoundInfo e)
        {
            log.Debug($"AllBodiesFound Detected");

            foreach (var observer in observers)
                observer.OnNext(e);
        }

        private void Events_StartJumpEvent(object sender, EliteAPI.Events.StartJumpInfo e)
        {
            log.Debug($"JumpEvent Detected");

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

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<EliteAPI.Events.IEvent>> _observers;
            private IObserver<EliteAPI.Events.IEvent> _observer;

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
