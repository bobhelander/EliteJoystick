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
        private readonly EdsmConnector.System starSystem;
        internal Explore(EdsmConnector.System starSystem) { this.starSystem = starSystem; }
    }
}
