using EliteGameStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using EliteJoystick.Common.Interfaces;
using EliteJoystick.Common.EliteGame;
using Microsoft.Extensions.Logging;
using EliteAPI.Abstractions;

namespace EliteJoystickService
{
    public class GameService : IGameService, IEliteGameStatus
    {
        private List<IDisposable> Disposables { get; set; }
        public Status GameStatusObservable { get; set; }

        private IEliteDangerousApi eliteApi { get; set; }

        private ILogger log { get; set; }
        private ILogger inGameLogger { get; set; }

        public GameService(
            IEliteDangerousApi eliteApi,
            ILogger<GameService> log,
            ILoggerFactory loggerFactory)
        {
            this.eliteApi = eliteApi;
            this.log = log;
            this.inGameLogger = loggerFactory.CreateLogger("InGame");
        }

        public void Initialize()
        {
            eliteApi.StartAsync().Wait();

            GameStatusObservable = new Status(eliteApi, log);
            Disposables = new List<IDisposable> {
                //GameStatusObservable.Subscribe(x => Mappings.GameStatusMapping.Process(x)),
                GameStatusObservable.Subscribe(x => EliteGameStatus.Handlers.JumpHandler.Process(x, log, inGameLogger)),
                GameStatusObservable.Subscribe(x => EliteGameStatus.Handlers.AllFoundHandler.Process(x, log, inGameLogger)),
                GameStatusObservable
            };
        }

        public void Dispose() =>
            Disposables?.ForEach(disposable => disposable?.Dispose());
    }
}
