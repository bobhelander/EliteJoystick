using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using EliteAPI.Abstractions;

namespace EliteGameStatus.Services
{
    public class GameService : IGameService, IEliteGameStatus
    {
        private List<IDisposable> Disposables { get; set; }
        public Status GameStatusObservable { get; set; }

        private IEliteDangerousApi eliteApi { get; set; }
        private IEdsmConnector edsmConnector { get; set; }

        private ExplorationService explorationService { get; set; }

        private ILogger log { get; set; }

        public GameService(
            IEliteDangerousApi eliteApi,
            IEdsmConnector edsmConnector,
            ExplorationService explorationService,
            ILogger<GameService> log)
        {
            this.eliteApi = eliteApi;
            this.edsmConnector = edsmConnector;
            this.explorationService = explorationService;
            this.log = log;
        }

        public void Initialize()
        {
            eliteApi.StartAsync().Wait();

            GameStatusObservable = new Status(eliteApi, log);
            Disposables = new List<IDisposable> {
                //GameStatusObservable.Subscribe(x => Mappings.GameStatusMapping.Process(x)),
                GameStatusObservable.Subscribe(x => EliteGameStatus.Handlers.JumpHandler.Process(x, edsmConnector, explorationService, log)),
                GameStatusObservable.Subscribe(x => EliteGameStatus.Handlers.AllFoundHandler.Process(x, edsmConnector, explorationService, log)),
                GameStatusObservable
            };
        }

        public void Dispose() =>
            Disposables?.ForEach(disposable => disposable?.Dispose());
    }
}
