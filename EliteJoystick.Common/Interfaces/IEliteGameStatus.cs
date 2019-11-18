using EliteJoystick.Common.EliteGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace EliteJoystick.Common.Interfaces
{
    public interface IEliteGameStatus
    {
        // Location
        string StarSystem { get; }
        string Body { get; }
        string BodyType { get; }
        string Station { get; }

        // Status
        bool SrvHandbreak { get; }
        bool SrvNearShip { get; }
        bool SrvDriveAssist { get; }
        bool MassLocked { get; }
        bool FsdCharging { get; }
        bool FsdCooldown { get; }
        bool LowFuel { get; }
        bool Overheating { get; }
        bool HasLatlong { get; }
        bool InDanger { get; }
        bool InInterdiction { get; }
        bool InMothership { get; }
        bool InFighter { get; }
        bool InSRV { get; }
        bool AnalysisMode { get; }
        bool NightVision { get; }
        string GameMode { get; }
        bool InNoFireZone { get; }
        double JumpRange { get; }
        bool IsRunning { get; }
        bool SrvTurrent { get; }
        bool InMainMenu { get; }
        string MusicTrack { get; }
        bool SilentRunning { get; }
        List<long> Pips { get; }
        long FireGroup { get; }
        long GuiFocus { get; }
        double FuelMain { get; }
        double FuelReservoir { get; }
        string LegalState { get; }
        long Cargo { get; }
        bool Docked { get; }
        bool Landed { get; }
        bool Gear { get; }
        bool Shields { get; }
        bool Supercruise { get; }
        bool FlightAssist { get; }
        bool Hardpoints { get; }
        bool Winging { get; }
        bool Lights { get; }
        bool CargoScoop { get; }
        bool Scooping { get; }
    }
}
