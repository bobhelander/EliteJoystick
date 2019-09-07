using EddiDataDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddiJoystickResponder.Exploration
{
    /* https://msdn.microsoft.com/en-us/library/ee844259.aspx */

    internal partial class Explore
    {
        private readonly StarSystem starSystem;
        internal Explore(StarSystem starSystem) { this.starSystem = starSystem; }
    }
}
