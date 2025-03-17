using System;
using System.IO;
using System.Reactive.Linq;

namespace EliteGameStatus
{
    public class StatusWatcher
    {
        private readonly FileSystemWatcher fileSystemWatcher;

        public IObservable<FileSystemEventArgs> Changed { get; private set; }

        public StatusWatcher()
        {
            fileSystemWatcher = new FileSystemWatcher(
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Saved Games\\Frontier Developments\\Elite Dangerous")
            {
                Filter = "Status.json",
                EnableRaisingEvents = true
            };

            Changed =
                Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    h => fileSystemWatcher.Changed += h,
                    h => fileSystemWatcher.Changed -= h)
                .Select(x => x.EventArgs);
        }
    }
}
