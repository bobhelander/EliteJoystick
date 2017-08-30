using System;
using System.Collections.Generic;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Overrides the <see cref="Controller"/> class to provides specific events for
    /// the management of a 'Thrustmaster Warthog'.
    /// </summary>
    public sealed class TMWartHogThrottleController : Controller
    {
        /// <summary>
        /// The vendor id for the Thrustmaster Warthog Throttle device.
        /// </summary>
        public const int VendorId = 0x044f;

        /// <summary>
        /// The product id for the Thrustmaster Warthog Throttle device.
        /// </summary>
        public const int ProductId = 0x0404;

        /// <summary>
        /// The stored state of the controller.
        /// </summary>
        private TmThrottleStatus previousRead;

        /// <summary>
        /// Device current status
        /// </summary>
        public TmThrottleStatus CurrentStatus { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TMWartHogThrottleController"/> class.
        /// </summary>
        /// <param name="devicePath">
        /// The path of the device.
        /// </param>
        private TMWartHogThrottleController(string devicePath)
            : base(devicePath)
        {
            this.Read += this.OnRead;
        }

        /// <summary>
        /// Occurs when the controller is moved.
        /// </summary>
        public event EventHandler<MoveEventArgs> Move;

        /// <summary>
        /// Occurs when the controller is turned.
        /// </summary>
        public event EventHandler<ThrottleAxisEventArgs> ThrottleRight;

        /// <summary>
        /// Occurs when the controller is turned.
        /// </summary>
        public event EventHandler<ThrottleAxisEventArgs> ThrottleLeft;

        /// <summary>
        /// Occurs when the throttle is moved.
        /// </summary>
        public event EventHandler<SliderEventArgs> Slider;

        /// <summary>
        /// Occurs when the hat is moved.
        /// </summary>
        public event EventHandler<HatEventArgs> Hat;

        /// <summary>
        /// Occurs when a state has changed on a switch.
        /// </summary>
        public event EventHandler<TmThrottleSwitchEventArgs> SwitchState;

        /// <summary>
        /// Retrieves all the active Thrustmaster Warthog controllers.
        /// </summary>
        /// <returns>The active Thrustmaster Warthog controllers.</returns>
        public static ICollection<TMWartHogThrottleController> RetrieveAll()
        {
            ICollection<TMWartHogThrottleController> result = new LinkedList<TMWartHogThrottleController>();
            foreach (string devicePath in Controller.RetrieveAllDevicePath(VendorId, ProductId))
            {
                result.Add(new TMWartHogThrottleController(devicePath));
            }

            return result;
        }

        /// <summary>
        /// Initializes the controller for reads.
        /// </summary>
        public override void Initialize()
        {
            this.previousRead = TmThrottleStatus.Empty;
            this.CurrentStatus = TmThrottleStatus.Empty;
            base.Initialize();
        }

        /// <summary>
        /// Controls the LED lights on the controller
        /// </summary>
        public byte LightIntensity
        {
            get { return CurrentStatus.LightIntensity; }
            set { UpdateLights(Lights, value); }
        }

        /// <summary>
        /// Controls the LED lights on the controller
        /// </summary>
        public byte Lights
        {
            get { return CurrentStatus.Lights; }
            set { UpdateLights(value, LightIntensity); }
        }

        /// <summary>
        /// Controls the LED lights on the controller
        /// </summary>
        public void UpdateLights(byte lights, byte lightIntensity)
        {
            byte[] buffer = new byte[WriteLength];
            buffer[0] = 0x01;
            buffer[1] = 0x06;
            buffer[2] = lights;
            buffer[3] = lightIntensity;

            Write(buffer, WriteLength);
        }

        /// <summary>
        /// Compares the current read with the previous one to determine the updates.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The details of the event.</param>
        private void OnRead(object sender, ReadEventArgs e)
        {
            TmThrottleStatus previous = this.previousRead;
            CurrentStatus = TmThrottleStatus.Create(e.Values);
            TmThrottleStatus current = CurrentStatus;

            // Check whether the position change
            if (previous.X != current.X || previous.Y != current.Y)
            {
                this.OnMove(current.X, current.Y);
            }

            // Check whether the rotation change
            if (previous.Z != current.Z)
            {
                this.OnThrottleRight(current.Z);
            }

            // Check whether the rotation change
            if (previous.Zr != current.Zr)
            {
                this.OnThrottleLeft(current.Zr);
            }

            // Check whether the slider change
            if (previous.Slider != current.Slider)
            {
                this.OnSlider(current.Slider);
            }

            // Check whether the hat change
            if (previous.Hat != current.Hat)
            {
                this.OnHat(current.Hat, current.HatSwitch);
            }
            
            // Check if a button changed
            if (previous.buttons != current.buttons)
            {
                this.OnSwitchState(current.buttons, previous.buttons);
            }

            // Store the current status
            this.previousRead = current;
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.Move"/> event.
        /// </summary>
        /// <param name="x">The x part of the position.</param>
        /// <param name="y">The y part of the position.</param>
        private void OnMove(int x, int y)
        {
            if (this.Move != null)
            {
                this.Move(this, new MoveEventArgs(x, y));
            }
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.Rotate"/> event.
        /// </summary>
        /// <param name="z">The current rotation level.</param>
        private void OnThrottleRight(ushort z)
        {
            if (this.ThrottleRight != null)
            {
                this.ThrottleRight(this, new ThrottleAxisEventArgs(z));
            }
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.Rotate"/> event.
        /// </summary>
        /// <param name="z">The current rotation level.</param>
        private void OnThrottleLeft(ushort z)
        {
            if (this.ThrottleLeft != null)
            {
                this.ThrottleLeft(this, new ThrottleAxisEventArgs(z));
            }
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.Slider"/> event.
        /// </summary>
        /// <param name="slider">The current slider level.</param>
        private void OnSlider(int slider)
        {
            if (this.Slider != null)
            {
                this.Slider(this, new SliderEventArgs(slider));
            }
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.Hat"/> event.
        /// </summary>
        /// <param name="hat">The hat position.</param>
        /// <param name="hatSwitch">The hat position.</param>
        private void OnHat(int hat, int hatSwitch)
        {
            if (this.Hat != null)
            {
                this.Hat(this, new HatEventArgs(hat, hatSwitch));
            }
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.Hat"/> event.
        /// </summary>
        /// <param name="allSwitches">The current rotation level.</param>
        private void OnSwitchState(UInt32 buttons, UInt32 previousButtons)
        {
            if (this.SwitchState != null)
            {
                this.SwitchState(this, new TmThrottleSwitchEventArgs(buttons, previousButtons));
            }
        }
    }
}
