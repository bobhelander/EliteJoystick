using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EliteJoystick.ViewConverters
{
    /// <summary>
    /// XAML Value Converter to change to bits of a UInt32 to a boolean array.
    /// </summary>
    [ValueConversion(typeof(UInt32), typeof(bool))]
    public class Button32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UInt32 buttons = 0;

            if (null != value)
                buttons = (UInt32)value;

            BitArray bitArray = new BitArray(BitConverter.GetBytes(buttons));

            return bitArray.Cast<bool>().Select(bit => bit ? true : false).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
