using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vKeyboard.Logic;

namespace vKeyboard
{
    public partial class Keyboard
    {
        const ushort TTC_VENDORID = 0xF00F;
        const ushort TTC_PRODUCTID_KEYBOARD = 0x00000003;

        public Device device { get; set; }

        private void InitializeDevice()
        {
            device = DeviceFactory.Devices(log).First(x => x.VendorID == TTC_VENDORID && x.ProductID == TTC_PRODUCTID_KEYBOARD);

            if (device != null)
                device.Open();
            else
                log?.LogError("Keyboard device not found");
        }

        private void DisposeDevice()
        {
            device?.Dispose();
        }
    }
}
