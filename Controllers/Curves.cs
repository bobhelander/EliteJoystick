using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /// <summary>
    /// Add axis curves to the output
    /// Allows axis movements of the physical joystick to be translated into finer movements of the virtual joystick
    /// until a threshold is reached.  Then the movement is mapped 1 to 1.
    /// This makes a zone where fine movements can be made with larger movements of the physical joystick
    /// </summary>
    public static class Curves
    {
        // Use simple Polynomials to make a curve that goes from 0 to 1
        public static int Calculate(int input, int max_input, double minimumPercent)
        {
            // Curve function
            double percent_of_max = ((double)Math.Abs(input) / max_input);

            // We want some movement at the bottom of the range
            if (percent_of_max < minimumPercent)
                percent_of_max = minimumPercent;

            // This calculates the amount of difference between the two values 0-1 where 1 is no difference
            double multiplier = Math.Pow(percent_of_max, 2);

            return (int)((double)input * multiplier);
        }
    }
}
