using EliteJoystick.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common
{
    /// <summary>
    /// Holds the shared state values for all the controllers.   
    /// </summary>
    public class EliteSharedState
    {
        #region Normal Properties

        public bool ThrottleShift1 { get; set; }
        public bool ThrottleShift2 { get; set; }

        public bool LeftThrottleEnabled { get; set; }
        public bool RightThrottleEnabled { get; set; } = true;

        public bool OrbitLines { get; set; }
        public bool HeadsUpDisplay { get; set; }

        public bool HardpointsDeployed { get; set; }
        public bool CameraActive { get; set; }
        public bool SecondaryFireActive { get; set; }

        public bool Mute { get; set; }

        public IEliteGameStatus EliteGameStatus { get; set; }

        #endregion

        #region RX Properties

        public Mode CurrentMode { get; private set; }

        private Subject<Mode> modeChanged = new Subject<Mode>();

        public IObservable<Mode> ModeChanged { get { return this.modeChanged.AsObservable(); } }

        public void ChangeMode(Mode mode) { CurrentMode = mode; this.modeChanged.OnNext(CurrentMode); }


        public bool LandingGear { get; private set; } = true;

        private Subject<bool> gearChanged = new Subject<bool>();

        public IObservable<bool> GearChanged { get { return this.gearChanged.AsObservable(); } }

        public void ChangeGear(bool deployed) { LandingGear = deployed; this.gearChanged.OnNext(LandingGear); }

        // This mode allows us to change the vJoy ids to match what the ganme is mapped to
        public bool ProgramIdsMode { get; private set; } = false;

        private Subject<bool> programModeChanged = new Subject<bool>();

        public IObservable<bool> ProgramModeChanged { get { return this.programModeChanged.AsObservable(); } }

        public void ChangeProgramMode(bool on) { ProgramIdsMode = on; this.programModeChanged.OnNext(ProgramIdsMode); }

        #endregion

        public enum Mode
        {
            Fighting,
            Travel,
            Mining,
            Docked,
            Landed,
            SRV,
            Fighter,
            Map
        }
    }
}
