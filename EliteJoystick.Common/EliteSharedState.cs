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

        #endregion


        public class ShiftChangedEventArgs : EventArgs
        {
            public ShiftState ShiftState { get; set; }
        }
        public class ThrottleShiftChangedEventArgs : EventArgs
        {
            public ThrottleShiftState ThrottleShiftState { get; set; }
        }
        public class MuteChangedEventArgs : EventArgs
        {
            public bool MuteState { get; set; }
        }
        public class GearDeployedEventArgs : EventArgs
        {
            public bool Deployed { get; set; }
        }

        public event EventHandler<ShiftChangedEventArgs> ShiftChanged;
        public event EventHandler<ThrottleShiftChangedEventArgs> ThrottleShiftChanged;
        public event EventHandler<MuteChangedEventArgs> MuteChanged;
        //public event EventHandler<GearDeployedEventArgs> GearChanged;

        public enum ShiftState { None, Shift1, Shift2, Shift3 }
        public enum ThrottleShiftState { None, Shift1, Shift2, Shift3 }
        public bool HardpointsDeployed { get; set; }
        public bool CameraActive { get; set; }
        public bool SecondaryFireActive { get; set; }

        

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

        private ShiftState shiftStateValue;
        public ShiftState ShiftStateValue
        {
            get { return shiftStateValue; }
            set
            {
                if (value != shiftStateValue)
                {
                    shiftStateValue = value;
                    OnShiftChanged(value);
                }
            }
        }

        private ThrottleShiftState throttleShiftStateValue;
        public ThrottleShiftState ThrottleShiftStateValue
        {
            get { return throttleShiftStateValue; }
            set
            {
                if (value != throttleShiftStateValue)
                {
                    throttleShiftStateValue = value;
                    OnThrottleShiftChanged(value);
                }
            }
        }

        private bool mute;
        public bool Mute
        {
            get { return mute; }
            set
            {
                if (value != mute)
                {
                    mute = value;
                    OnMuteChanged(value);
                }
            }
        }

        private bool gearDeployed;
        public bool GearDeployed
        {
            get { return gearDeployed; }
            set
            {
                if (value != gearDeployed)
                {
                    gearDeployed = value;
                    OnGearChanged(value);
                }
            }
        }

        public bool LeftThrottleEnabled { get; set; }
        public bool RightThrottleEnabled { get; set; }

        public bool OrbitLines { get; set; }
        public bool HeadsUpDisplay { get; set; }


        private void OnShiftChanged(ShiftState shift)
        {
            if (null != ShiftChanged)
                ShiftChanged(this, new ShiftChangedEventArgs { ShiftState = shift });
        }

        private void OnThrottleShiftChanged(ThrottleShiftState shift)
        {
            if (null != ThrottleShiftChanged)
                ThrottleShiftChanged(this, new ThrottleShiftChangedEventArgs { ThrottleShiftState = shift });
        }

        private void OnMuteChanged(bool mute)
        {
            if (null != MuteChanged)
                MuteChanged(this, new MuteChangedEventArgs { MuteState = mute });
        }
    }
}
