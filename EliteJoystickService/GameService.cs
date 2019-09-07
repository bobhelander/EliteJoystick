using EliteGameStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using EliteJoystick.Common.Interfaces;
using EliteJoystick.Common.EliteGame;

namespace EliteJoystickService
{
    public class GameService : IDisposable, IEliteGameStatus
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<IDisposable> Disposables { get; set; }
        private Status GameStatusObservable { get; set; } = new Status();

        public GameStatus Status { get; set; }

        public void Initialize()
        {
            Disposables = new List<IDisposable> {
                GameStatusObservable.DistinctUntilChanged(x => x.Flags).Subscribe(x => Update(x), ex => log.Error($"Exception : {ex}")),
                GameStatusObservable.DistinctUntilChanged(x => (x.Flags & (int)StatusFlags.FlagLightsOn)).Subscribe(x => log.Debug("Lights On/Off"), ex => log.Error($"Exception : {ex}"))
            };
        }

        private void Update(GameStatus status)
        {
            Status = status;
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables ?? new List<IDisposable>())
                disposable?.Dispose();
        }
    }
}
