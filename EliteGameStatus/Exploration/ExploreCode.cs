using EliteJoystick.Common.Models;

namespace EliteGameStatus.Exploration
{
    /* https://msdn.microsoft.com/en-us/library/ee844259.aspx */

    public partial class Explore
    {
        private readonly StarSystem starSystem;
        internal Explore(StarSystem starSystem) { this.starSystem = starSystem; }
    }
}
