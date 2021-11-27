using EliteJoystick.Common.Interfaces;
using EliteJoystick.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteGameStatus.Exploration
{
    /* https://msdn.microsoft.com/en-us/library/ee844259.aspx */

    public partial class Explore
    {
        private readonly StarSystem starSystem;
        internal Explore(StarSystem starSystem) { this.starSystem = starSystem; }
    }
}
