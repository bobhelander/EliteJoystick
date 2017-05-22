using System;
using System.Collections.Generic;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Overrides the <see cref="Controller"/> class to provides specific events for
    /// the management of a 'Microsoft Sidewinder Game Voice'.
    /// </summary>
    public sealed class SwgvController : Controller
    {
        /// <summary>
        /// The vendor id for the Sidewinder Game Voice device.
        /// </summary>
        public const int VendorId = 0x045e;

        /// <summary>
        /// The product id for the Sidewinder Game Voice device.
        /// </summary>
        public const int ProductId = 0x003B;

        /// <summary>
        /// The stored state of the controller.
        /// </summary>
        private SwgvStatus previousRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwgvController"/> class.
        /// </summary>
        /// <param name="devicePath">
        /// The path of the device.
        /// </param>
        private SwgvController(string devicePath)
            : base(devicePath)
        {
            this.Read += this.OnRead;
        }

        public event EventHandler<SwgvButtonStateEventArgs> ButtonsChanged;

        /// <summary>
        /// Occurs when a button is pressed.
        /// </summary>
        public event EventHandler<SwgvButtonEventArgs> ButtonDown;

        /// <summary>
        /// Occurs when a button is released.
        /// </summary>
        public event EventHandler<SwgvButtonEventArgs> ButtonUp;

        /// <summary>
        /// Retrieves all the active Strategic Commander controllers.
        /// </summary>
        /// <returns>The active Strategic Commander controllers.</returns>
        public static ICollection<SwgvController> RetrieveAll()
        {
            ICollection<SwgvController> result = new LinkedList<SwgvController>();
            foreach (string devicePath in Controller.RetrieveAllDevicePath(VendorId, ProductId))
            {
                result.Add(new SwgvController(devicePath));
            }

            return result;
        }

        /// <summary>
        /// Initializes the controller for reads.
        /// </summary>
        public override void Initialize()
        {
            this.previousRead = SwgvStatus.Empty;
            base.Initialize();
        }

        /// <summary>
        /// Defines the enabled lights on the controller.
        /// </summary>
        /// <param name="lights">The enabled lights on the controller.</param>
        public byte Lights
        {
            get
            {
                return FeatureValue[1];
            }

            set
            {
                FeatureValue = new byte[] { 0x00, value };
            }
        }

        /// <summary>
        /// Compares the current read with the previous one to determine the updates.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The details of the event.</param>
        private void OnRead(object sender, ReadEventArgs e)
        {
            SwgvStatus previous = this.previousRead;
            SwgvStatus current = SwgvStatus.Create(e.Values);

            // Trace the received data in both binary and rich formats
            if (Logger.Default.IsVerbose())
            {
                //Logger.Default.Verbose("{0:x8} {1}", e.Values.Reverse().Aggregate(0L, (l, b) => l << 8 | b), current);
            }

            // Check whether the buttons change
            var upButtons = previous.DownButtons.Except(current.DownButtons);
            foreach (SwgvButton button in upButtons)
            {
                this.OnButtonUp(button);
            }

            var downButtons = current.DownButtons.Except(previous.DownButtons);
            foreach (SwgvButton button in downButtons)
            {
                this.OnButtonDown(button);
            }

            if (current.Buttons != previous.Buttons)
                OnButtonStateChanged(current.Buttons, previous.Buttons);

            // Store the current status
            this.previousRead = current;
        }

        

        /// <summary>
        /// Raised the <see cref="SwgvController.ButtonUp"/> event.
        /// </summary>
        /// <param name="button">The released button.</param>
        private void OnButtonUp(SwgvButton button)
        {
            if (this.ButtonUp != null)
            {
                this.ButtonUp(this, new SwgvButtonEventArgs(button, false));
            }
        }

        /// <summary>
        /// Raised the <see cref="SwgvController.ButtonDown"/> event.
        /// </summary>
        /// <param name="button">The pressed button.</param>
        private void OnButtonDown(SwgvButton button)
        {
            if (this.ButtonDown != null)
            {
                this.ButtonDown(this, new SwgvButtonEventArgs(button, true));
            }
        }

        private void OnButtonStateChanged(byte currentButtons, byte previousButtons)
        {
            if (ButtonsChanged != null)
            {
                ButtonsChanged(this, new SwgvButtonStateEventArgs {
                    ButtonsState = currentButtons,
                    PreviousButtonsState = previousButtons,
                    }
                );
            }
        }
    }
}
