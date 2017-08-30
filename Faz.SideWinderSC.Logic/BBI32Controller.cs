using System;
using System.Collections.Generic;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Overrides the <see cref="Controller"/> class to provides specific events for
    /// the management of a 'BBI-32 Device'.
    /// </summary>
    public sealed class BBI32Controller : Controller
    {
        /// <summary>
        /// The vendor id for the Leo Bodnar BBI-32 device.
        /// </summary>
        public const int VendorId = 0x1DD2;

        /// <summary>
        /// The product id for the Leo Bodnar BBI-32 device.
        /// </summary>
        public const int ProductId = 0x1150;

        /// <summary>
        /// The stored state of the controller.
        /// </summary>
        private BBI32Status previousRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="BBI32Controller"/> class.
        /// </summary>
        /// <param name="devicePath">
        /// The path of the device.
        /// </param>
        private BBI32Controller(string devicePath)
            : base(devicePath)
        {
            this.Read += this.OnRead;

            ProcessAllReports = false;
        }

        /// <summary>
        /// Handler for events
        /// </summary>
        public event EventHandler<ButtonStateEventArgs> ButtonsChanged;

        /// <summary>
        /// Retrieves all the active BBI-32 controllers.
        /// </summary>
        /// <returns>The active BBI-32 controllers.</returns>
        public static ICollection<BBI32Controller> RetrieveAll()
        {
            ICollection<BBI32Controller> result = new LinkedList<BBI32Controller>();
            foreach (string devicePath in Controller.RetrieveAllDevicePath(VendorId, ProductId))
            {
                result.Add(new BBI32Controller(devicePath));
            }

            return result;
        }

        /// <summary>
        /// Initializes the controller for reads.
        /// </summary>
        public override void Initialize()
        {
            this.previousRead = BBI32Status.Empty;
            base.Initialize();
        }

        /// <summary>
        /// Compares the current read with the previous one to determine the updates.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The details of the event.</param>
        private void OnRead(object sender, ReadEventArgs e)
        {
            BBI32Status previous = this.previousRead;
            BBI32Status current = BBI32Status.Create(e.Values);

            if (current.Buttons != previous.Buttons)
                OnButtonStateChanged(current.Buttons, previous.Buttons);

            // Store the current status
            this.previousRead = current;
        }

        private void OnButtonStateChanged(uint currentButtons, uint previousButtons)
        {
            if (ButtonsChanged != null)
            {
                ButtonsChanged(this, 
                    new ButtonStateEventArgs
                    {
                        ButtonsState = currentButtons,
                        PreviousButtonsState = previousButtons,
                    }
                );
            }
        }
    }
}
