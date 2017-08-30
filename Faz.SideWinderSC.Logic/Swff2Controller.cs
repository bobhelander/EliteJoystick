using System;
using System.Collections.Generic;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Overrides the <see cref="Controller"/> class to provides specific events for
    /// the management of a 'Microsoft Sidewinder Force Feedback 2'.
    /// </summary>
    public sealed class Swff2Controller : Controller
    {
        /// <summary>
        /// The vendor id for the Sidewinder Force Feedback 2 device.
        /// </summary>
        public const int VendorId = 0x045e;

        /// <summary>
        /// The product id for the Sidewinder Force Feedback 2 device.
        /// </summary>
        public const int ProductId = 0x001b;

        /// <summary>
        /// The stored state of the controller.
        /// </summary>
        private Swff2Status previousRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="Swff2Controller"/> class.
        /// </summary>
        /// <param name="devicePath">
        /// The path of the device.
        /// </param>
        private Swff2Controller(string devicePath)
            : base(devicePath)
        {
            this.Read += this.OnRead;
        }

        /// <summary>
        /// Occurs when a button is pressed.
        /// </summary>
        public event EventHandler<Swff2ButtonEventArg> ButtonDown;

        /// <summary>
        /// Occurs when a button is released.
        /// </summary>
        public event EventHandler<Swff2ButtonEventArg> ButtonUp;

        /// <summary>
        /// Occurs when the controller is moved.
        /// </summary>
        public event EventHandler<MoveEventArgs> Move;

        /// <summary>
        /// Occurs when the controller is turned.
        /// </summary>
        public event EventHandler<RotateEventArgs> Rotate;

        /// <summary>
        /// Occurs when the throttle is moved.
        /// </summary>
        public event EventHandler<SliderEventArgs> Slider;

        /// <summary>
        /// Occurs when the hat is moved.
        /// </summary>
        public event EventHandler<HatEventArgs> Hat;

        /// <summary>
        /// Occurs when the button states change
        /// </summary>
        public event EventHandler<ButtonsEventArgs> ButtonsChanged;

        /// <summary>
        /// Retrieves all the active Force Feedback 2 controllers.
        /// </summary>
        /// <returns>The active Force Feedback 2 controllers.</returns>
        public static ICollection<Swff2Controller> RetrieveAll()
        {
            ICollection<Swff2Controller> result = new LinkedList<Swff2Controller>();
            foreach (string devicePath in Controller.RetrieveAllDevicePath(VendorId, ProductId))
            {
                result.Add(new Swff2Controller(devicePath));
            }

            return result;
        }

        /// <summary>
        /// Initializes the controller for reads.
        /// </summary>
        public override void Initialize()
        {
            this.previousRead = Swff2Status.Empty;
            base.Initialize();
        }

        /// <summary>
        /// Compares the current read with the previous one to determine the updates.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The details of the event.</param>
        private void OnRead(object sender, ReadEventArgs e)
        {
            Swff2Status previous = this.previousRead;
            Swff2Status current = Swff2Status.Create(e.Values);

            // Trace the received data in both binary and rich formats
            if (Logger.Default.IsVerbose())
            {
               // Logger.Default.Verbose("{0:x8} {1}", e.Values.Reverse().Aggregate(0L, (l, b) => l << 8 | b), current);
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

            // Check whether the slider change
            if (previous.Slider != current.Slider)
            {
                this.OnSlider(current.Slider);
            }

            // Check whether the hat change
            if (previous.Hat != current.Hat)
            {
                this.OnHat(current.Hat);
            }

            // Check whether the buttons change
            if (previous.Buttons != current.Buttons)
                this.OnButtonsChange(current.Buttons, previous.Buttons);

            // Store the current status
            this.previousRead = current;
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.ButtonDown"/> event.
        /// </summary>
        /// <param name="buttons">The button states.</param>
        /// <param name="previousButtons">The last button states.</param>
        private void OnButtonsChange(uint buttons, uint previousButtons)
        {
            if (null != this.ButtonsChanged)
                this.ButtonsChanged(this, new ButtonsEventArgs { Buttons = buttons, PreviousButtons = previousButtons });
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
        /// <param name="r">The current rotation level.</param>
        private void OnRotate(int r)
        {
            if (this.Rotate != null)
            {
                this.Rotate(this, new RotateEventArgs(r));
            }
        }

        /// <summary>
        /// Raised the <see cref="Swff2Controller.Slider"/> event.
        /// </summary>
        /// <param name="slider">The current rotation level.</param>
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
        /// <param name="hat">The current rotation level.</param>
        private void OnHat(int hat)
        {
            if (this.Hat != null)
            {
                this.Hat(this, new HatEventArgs(hat));
            }
        }
    }
}
