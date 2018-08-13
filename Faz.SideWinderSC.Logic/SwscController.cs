using System;
using System.Collections.Generic;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Overrides the <see cref="Controller"/> class to provides specific events for
    /// the management of a 'Microsoft SideWinder Strategic Commander'.
    /// </summary>
    public sealed class SwscController : Controller
    {
        /// <summary>
        /// The vendor id for the Strategic Commander device.
        /// </summary>
        public const int VendorId = 0x045e;

        /// <summary>
        /// The product id for the Strategic Commander device.
        /// </summary>
        public const int ProductId = 0x0033;

        /// <summary>
        /// The stored state of the controller.
        /// </summary>
        private SwscStatus previousRead;

        /// <summary>
        /// Current status of the device
        /// </summary>
        public SwscStatus CurrentStatus { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwscController"/> class.
        /// </summary>
        /// <param name="devicePath">
        /// The path of the device.
        /// </param>
        private SwscController(string devicePath)
            : base(devicePath)
        {
            this.Read += this.OnRead;
        }

        /// <summary>
        /// Occurs when the profile changed.
        /// </summary>
        public event EventHandler<ProfileChangedEventArgs> ProfileChanged;

        /// <summary>
        /// Occurs when a button is pressed.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonDown;

        /// <summary>
        /// Occurs when a button is released.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonUp;

        /// <summary>
        /// Occurs when the controller is moved.
        /// </summary>
        public event EventHandler<MoveEventArgs> Move;

        /// <summary>
        /// Occurs when the controller is turned.
        /// </summary>
        public event EventHandler<RotateEventArgs> Rotate;

        /// <summary>
        /// Occurs when the buttons changed.
        /// </summary>
        public event EventHandler<ButtonsEventArgs> ButtonsChanged;

        /// <summary>
        /// Retrieves all the active Strategic Commander controllers.
        /// </summary>
        /// <returns>The active Strategic Commander controllers.</returns>
        public static ICollection<SwscController> RetrieveAll()
        {
            ICollection<SwscController> result = new LinkedList<SwscController>();
            foreach (string devicePath in Controller.RetrieveAllDevicePath(VendorId, ProductId))
            {
                result.Add(new SwscController(devicePath));
            }

            return result;
        }

        /// <summary>
        /// Initializes the controller for reads.
        /// </summary>
        public override void Initialize()
        {
            this.previousRead = SwscStatus.Empty;
            this.CurrentStatus = SwscStatus.Empty;
            base.Initialize();
        }

        /// <summary>
        /// Defines the enabled lights on the controller.
        /// </summary>
        /// <param name="lights">The enabled lights on the controller.</param>
        public void SetLights(params SwscLight[] lights)
        {
            short lightsValue = (short)lights.Sum(l => (int)l);
            byte[] values = new byte[this.FeatureLength];
            values[1] = (byte)(lightsValue & 0x00ff);
            values[2] = (byte)(lightsValue >> 8);
            this.Feature = values;
        }

        /// <summary>
        /// Compares the current read with the previous one to determine the updates.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The details of the event.</param>
        private void OnRead(object sender, ReadEventArgs e)
        {
            SwscStatus previous = this.previousRead;
            CurrentStatus = SwscStatus.Create(e.Values);
            SwscStatus current = CurrentStatus;

            // Trace the received data in both binary and rich formats
            if (Logger.Default.IsVerbose())
            {
                Logger.Default.Verbose("{0:x8} {1}", e.Values.Reverse().Aggregate(0L, (l, b) => l << 8 | b), current);
            }

            // Check whether the profile change
            if (previous.Profile != current.Profile)
            {
                this.OnProfileChanged(current.Profile);
            }

            // Check whether the position change
            if (previous.X != current.X || previous.Y != current.Y)
            {
                this.OnMove(current.X, current.Y);
            }

            // Check whether the rotation change
            if (previous.R != current.R)
            {
                this.OnRotate(current.R);
            }

            // Check whether the buttons change
            var upButtons = previous.DownButtons.Except(current.DownButtons);
            foreach (SwscButton button in upButtons)
            {
                this.OnButtonUp(button);
            }

            var downButtons = current.DownButtons.Except(previous.DownButtons);
            foreach (SwscButton button in downButtons)
            {
                this.OnButtonDown(button);
            }

            // Check whether the buttons change
            if (previous.Buttons != current.Buttons)
                this.OnButtonsChange(current.Buttons, previous.Buttons);

            // Store the current status
            this.previousRead = current;
        }

        /// <summary>
        /// Raised the <see cref="SwscController.ProfileChanged"/> event.
        /// </summary>
        /// <param name="profile">The new profile.</param>
        private void OnProfileChanged(int profile)
        {
            this.ProfileChanged?.Invoke(this, new ProfileChangedEventArgs(profile));
        }

        /// <summary>
        /// Raised the <see cref="SwscController.ButtonUp"/> event.
        /// </summary>
        /// <param name="button">The released button.</param>
        private void OnButtonUp(SwscButton button)
        {
            this.ButtonUp?.Invoke(this, new ButtonEventArgs(button, false));
        }

        /// <summary>
        /// Raised the <see cref="SwscController.ButtonDown"/> event.
        /// </summary>
        /// <param name="button">The pressed button.</param>
        private void OnButtonDown(SwscButton button)
        {
            this.ButtonDown?.Invoke(this, new ButtonEventArgs(button, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="previousButtons"></param>
        private void OnButtonsChange(uint buttons, uint previousButtons)
        {
            this.ButtonsChanged?.Invoke(this, new ButtonsEventArgs { Buttons = buttons, PreviousButtons = previousButtons });
        }

        /// <summary>
        /// Raised the <see cref="SwscController.Move"/> event.
        /// </summary>
        /// <param name="x">The x part of the position.</param>
        /// <param name="y">The y part of the position.</param>
        private void OnMove(int x, int y)
        {
            this.Move?.Invoke(this, new MoveEventArgs(x, y));
        }

        /// <summary>
        /// Raised the <see cref="SwscController.Rotate"/> event.
        /// </summary>
        /// <param name="r">The current rotation level.</param>
        private void OnRotate(int r)
        {
            this.Rotate?.Invoke(this, new RotateEventArgs(r));
        }
    }
}
