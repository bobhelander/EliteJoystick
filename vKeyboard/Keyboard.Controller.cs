using System;
using System.Collections.Generic;
using System.Text;
using Usb.Hid.Connection;

namespace vKeyboard
{
    public partial class Keyboard
    {
        private readonly string Product = "Tetherscript Virtual Keyboard";
        private readonly string Manufacturer = "Tetherscript Technology Corporation";

        private Controller Controller { get; set; }

        private void InitializeController()
        {
            bool foundPath = false;

            foreach (var path in Devices.RetrieveAllDevicePath())
            {
                try
                {
                    using (var controller = new Controller(path, log))
                    {
                        if (Manufacturer.CompareTo(controller.ManufacturerString) == 0 &&
                            Product.CompareTo(controller.ProductString) == 0)
                        {
                            foundPath = true;
                        }
                    }
                }
                catch
                {
                    // Skip it
                }

                if (foundPath)
                {
                    Controller = new Controller(path, log);
                    break;
                }
            }
        }

        private void DisposeController()
        {
            Controller?.Dispose();
        }
    }
}
