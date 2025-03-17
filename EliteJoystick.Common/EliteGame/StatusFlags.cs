namespace EliteJoystick.Common.EliteGame
{
    public enum StatusFlags
    {
        // FlagDocked indicates that the ship is docked.
        FlagDocked = 0x00000001,
        // FlagLanded indicates that the ship is landed.
        FlagLanded = 0x00000002,
        // FlagLandingGearDown indicates that the landing gear is deployed.
        FlagLandingGearDown = 0x00000004,
        // FlagShieldsUp indicates that the ship's shields are up.
        FlagShieldsUp = 0x00000008,
        // FlagSupercruise indicates that the ship is in supercruise.
        FlagSupercruise = 0x00000010,
        // FlagFlightAssistOff indicates that flight assist is disabled.
        FlagFlightAssistOff = 0x00000020,
        // FlagHardpointsDeployed indicates that the ship's hardpoints are deployed.
        FlagHardpointsDeployed = 0x00000040,
        // FlagInWing indicates whether the player is in a wing.
        FlagInWing = 0x00000080,
        // FlagLightsOn indicates that the ship's lights are on.
        FlagLightsOn = 0x00000100,
        // FlagCargoScoopDeployed indicates that the cargo scoop is deployed.
        FlagCargoScoopDeployed = 0x00000200,
        // FlagSilentRunning indicates that silent running is on.
        FlagSilentRunning = 0x00000400,
        // FlagScoopingFuel indicates that the ship is currently scooping fuel.
        FlagScoopingFuel = 0x00000800,
        // FlagSRVHandbrake indicates that the SRV's handbrake is enabled.
        FlagSRVHandbrake = 0x00001000,
        // FlagSRVTurret indicates that the SRV's turret is deployed.
        FlagSRVTurret = 0x00002000,
        // FlagSRVUnderShip indicates that the SRV is positioned under the ship.
        FlagSRVUnderShip = 0x00004000,
        // FlagSRVDriveAssist indicates that the SRV's drive assist is on.
        FlagSRVDriveAssist = 0x00008000,
        // FlagFSDMassLocked indicates that the ship is mass locked.
        FlagFSDMassLocked = 0x00010000,
        // FlagFSDCharging indicates that the FSD is charging.
        FlagFSDCharging = 0x00020000,
        // FlagFSDCooldown indicates that the FSD is cooling down.
        FlagFSDCooldown = 0x00040000,
        // FlagLowFuel indicates that the ship is low on fuel.
        FlagLowFuel = 0x00080000,
        // FlagOverheating indicates that the ship is overheating.
        FlagOverheating = 0x00100000,
        // FlagHasLatLong indicates that latitude and longitude data are available.
        FlagHasLatLong = 0x00200000,
        // FlagIsInDanger indicates that the player is in danger.
        FlagIsInDanger = 0x00400000,
        // FlagBeingInterdicted indicates that the ship is being interdicted.
        FlagBeingInterdicted = 0x00800000,
        // FlagInMainShip indicates that the player is in the ship.
        FlagInMainShip = 0x01000000,
        // FlagInFighter indicates that the player is in a fighter.
        FlagInFighter = 0x02000000,
        // FlagInSRV indicates that the player is in an SRV.
        FlagInSRV = 0x04000000,
        // FlagInAnalysisMode indicates that analysis mode is selected.
        FlagInAnalysisMode = 0x08000000,
        // FlagNightVision indicates that night vision is enabled.
        FlagNightVision = 0x10000000
    }
}
