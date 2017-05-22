using System;
using System.Collections.Generic;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Overrides the <see cref="Controller"/> class to provides specific events for
    /// the management of a 'Keypad'.
    /// </summary>
    public sealed class KeypadController : Controller
    {
        /// <summary>
        /// The vendor id for the Gear Head device.
        /// </summary>
        public const int VendorId = 0x1A2C;

        /// <summary>
        /// The product id for the Keypad2 device.
        /// </summary>
        public const int ProductId = 0x0E24;

        /// <summary>
        /// The stored state of the controller.
        /// </summary>
        private KeypadStatus previousRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeypadController"/> class.
        /// </summary>
        /// <param name="devicePath">
        /// The path of the device.
        /// </param>
        private KeypadController(string devicePath)
            : base(devicePath)
        {
            this.Read += this.OnRead;
        }

        /// <summary>
        /// Occurs when the hat is moved.
        /// </summary>
        public event EventHandler<ButtonsEventArgs> ButtonsChanged;

        /// <summary>
        /// Retrieves all the active Strategic Commander controllers.
        /// </summary>
        /// <returns>The active Keypad controllers.</returns>
        public static ICollection<KeypadController> RetrieveAll()
        {
            ICollection<KeypadController> result = new LinkedList<KeypadController>();
            var controllers = Controller.RetrieveAllDevicePath(VendorId, ProductId).ToList();
            foreach (string devicePath in controllers)
            {
                try
                {
                    if (devicePath.Contains("mi_01"))
                    {
                        result.Add(new KeypadController(devicePath));
                        break;
                    }
                }
                catch(Exception ex)
                {
                    ;
                }
            }

            return result;
        }

        /// <summary>
        /// Initializes the controller for reads.
        /// </summary>
        public override void Initialize()
        {
            this.previousRead = KeypadStatus.Empty;
            base.Initialize();
        }

        /// <summary>
        /// Compares the current read with the previous one to determine the updates.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The details of the event.</param>
        private void OnRead(object sender, ReadEventArgs e)
        {
            KeypadStatus previous = this.previousRead;
            KeypadStatus current = KeypadStatus.Create(e.Values);

            // Check whether the buttons change
            if (previous.Buttons != current.Buttons)
                this.OnButtonsChange(current.Buttons, previous.Buttons);

            // Store the current status
            this.previousRead = current;
        }

        /// <summary>
        /// Raised the <see cref="KeypadController.ButtonDown"/> event.
        /// </summary>
        /// <param name="buttons">The pressed button.</param>
        private void OnButtonsChange(uint buttons, uint previousButtons)
        {
            if (null != this.ButtonsChanged)
                this.ButtonsChanged(this, new ButtonsEventArgs { Buttons = buttons, PreviousButtons = previousButtons });
        }
    }
}
