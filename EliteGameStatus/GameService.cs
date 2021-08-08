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

namespace EliteJoystickService
{
    public class GameService : IDisposable, IEliteGameStatus
    {
        private List<IDisposable> Disposables { get; set; }
        public Status GameStatusObservable { get; } = new Status();

        public ILogger Logger { get; set; }
        public ILogger InGameLogger { get; set; }

        public void Initialize()
        {
            Disposables = new List<IDisposable> {
                //GameStatusObservable.Subscribe(x => Mappings.GameStatusMapping.Process(x)),
                GameStatusObservable.Subscribe(x => EliteGameStatus.Handlers.JumpHandler.Process(x, Logger, InGameLogger)),
                GameStatusObservable.Subscribe(x => EliteGameStatus.Handlers.AllFoundHandler.Process(x, Logger, InGameLogger))
            };
        }

        public void Dispose() =>
            Disposables?.ForEach(disposable => disposable?.Dispose());

        #region Game Status 
        public string StarSystem => GameStatusObservable.EliteAPI.Location.StarSystem;
        public string Body => GameStatusObservable.EliteAPI.Location.Body;
        public string BodyType => GameStatusObservable.EliteAPI.Location.BodyType;
        public string Station => GameStatusObservable.EliteAPI.Location.Station;
        public bool SrvHandbreak => GameStatusObservable.EliteAPI.Status.SrvHandbreak;
        public bool SrvNearShip => GameStatusObservable.EliteAPI.Status.SrvNearShip;
        public bool SrvDriveAssist => GameStatusObservable.EliteAPI.Status.SrvDriveAssist;
        public bool MassLocked => GameStatusObservable.EliteAPI.Status.MassLocked;
        public bool FsdCharging => GameStatusObservable.EliteAPI.Status.FsdCharging;
        public bool FsdCooldown => GameStatusObservable.EliteAPI.Status.FsdCooldown;
        public bool LowFuel => GameStatusObservable.EliteAPI.Status.LowFuel;
        public bool Overheating => GameStatusObservable.EliteAPI.Status.Overheating;
        public bool HasLatlong => GameStatusObservable.EliteAPI.Status.HasLatlong;
        public bool InDanger => GameStatusObservable.EliteAPI.Status.InDanger;
        public bool InInterdiction => GameStatusObservable.EliteAPI.Status.InInterdiction;
        public bool InMothership => GameStatusObservable.EliteAPI.Status.InMothership;
        public bool InFighter => GameStatusObservable.EliteAPI.Status.InFighter;
        public bool InSRV => GameStatusObservable.EliteAPI.Status.InSRV;
        public bool AnalysisMode => GameStatusObservable.EliteAPI.Status.AnalysisMode;
        public bool NightVision => GameStatusObservable.EliteAPI.Status.NightVision;
        public string GameMode => GameStatusObservable.EliteAPI.Status.GameMode;
        public bool InNoFireZone => GameStatusObservable.EliteAPI.Status.InNoFireZone;
        public double JumpRange => GameStatusObservable.EliteAPI.Status.JumpRange;
        public bool IsRunning => GameStatusObservable.EliteAPI.Status.IsRunning;
        public bool SrvTurrent => GameStatusObservable.EliteAPI.Status.SrvTurrent;
        public bool InMainMenu => GameStatusObservable.EliteAPI.Status.InMainMenu;
        public string MusicTrack => GameStatusObservable.EliteAPI.Status.MusicTrack;
        public bool SilentRunning => GameStatusObservable.EliteAPI.Status.SilentRunning;
        public List<long> Pips => GameStatusObservable.EliteAPI.Status.Pips;
        public long FireGroup => GameStatusObservable.EliteAPI.Status.FireGroup;
        public long GuiFocus => GameStatusObservable.EliteAPI.Status.GuiFocus;
        public double FuelMain => GameStatusObservable.EliteAPI.Status.Fuel.FuelMain;
        public double FuelReservoir => GameStatusObservable.EliteAPI.Status.Fuel.FuelReservoir;
        public string LegalState => GameStatusObservable.EliteAPI.Status.LegalState;
        public long Cargo => GameStatusObservable.EliteAPI.Status.Cargo;
        public bool Docked => GameStatusObservable.EliteAPI.Status.Docked;
        public bool Landed => GameStatusObservable.EliteAPI.Status.Landed;
        public bool Gear => GameStatusObservable.EliteAPI.Status.Gear;
        public bool Shields => GameStatusObservable.EliteAPI.Status.Shields;
        public bool Supercruise => GameStatusObservable.EliteAPI.Status.Supercruise;
        public bool FlightAssist => GameStatusObservable.EliteAPI.Status.FlightAssist;
        public bool Hardpoints => GameStatusObservable.EliteAPI.Status.Hardpoints;
        public bool Winging => GameStatusObservable.EliteAPI.Status.Winging;
        public bool Lights => GameStatusObservable.EliteAPI.Status.Lights;
        public bool CargoScoop => GameStatusObservable.EliteAPI.Status.CargoScoop;
        public bool Scooping => GameStatusObservable.EliteAPI.Status.Scooping;

        #endregion
    }
}
