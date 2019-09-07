using EliteJoystick.Common.EliteGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text;

namespace EliteGameStatus
{
    public class Status : IObservable<GameStatus>
    {
        private List<IObserver<GameStatus>> observers = new List<IObserver<GameStatus>>();

        private IDisposable Handler { get; set; }

        public Status()
        {
            Handler = new StatusWatcher().Changed.Subscribe(x => Process(x, observers));
        }

        private static void Process(FileSystemEventArgs e, List<IObserver<GameStatus>> observers)
        {
            var fileContents = string.Empty;

            using (var stream = File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var textReader = new StreamReader(stream))
                {
                    fileContents = textReader.ReadToEnd();
                }
            }

            if (false == string.IsNullOrEmpty(fileContents))
            {
                var updatedStatus = JsonConvert.DeserializeObject<GameStatus>(fileContents);

                foreach (var observer in observers)
                    observer.OnNext(updatedStatus);
            }
        }

        public IDisposable Subscribe(IObserver<GameStatus> observer)
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
            private List<IObserver<GameStatus>> _observers;
            private IObserver<GameStatus> _observer;

            public Unsubscriber(List<IObserver<GameStatus>> observers, IObserver<GameStatus> observer)
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
