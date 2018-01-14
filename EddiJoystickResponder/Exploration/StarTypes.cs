using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddiJoystickResponder.Exploration
{
    public class StarTypes
    {
        public static Dictionary<string, StarScan> Stars = new Dictionary<string, StarScan> {
            { "O (Blue-White) Star", new StarScan {scan = false, value = 6135 } },
            { "B (Blue-White) Star", new StarScan {scan=false,value=3012}},
            { "A (Blue-White) Star", new StarScan {scan=false,value=2949}},
            { "A (Blue-White super giant) Star", new StarScan {scan=false,value=2949}},
            { "F (White) Star", new StarScan {scan=false,value=2932}},
            { "F (White super giant) Star", new StarScan {scan=false,value=2932}},
            { "G (White-Yellow) Star", new StarScan {scan=false,value=2919}},
            { "K (Yellow-Orange) Star", new StarScan {scan=false,value=2916}},
            { "K (Yellow-Orange giant) Star", new StarScan {scan=false,value=2916}},
            { "M (Red dwarf) Star", new StarScan {scan=false,value=2903}},
            { "M (Red giant) Star", new StarScan {scan=false,value=2903}},
            { "M (Red super giant) Star", new StarScan {scan=false,value=2903}},
            { "L (Brown dwarf) Star", new StarScan {scan=false,value=2889}},
            { "T (Brown dwarf) Star", new StarScan {scan=false,value=2895}},
            { "Y (Brown dwarf) Star", new StarScan {scan=false,value=2881}},
            { "T Tauri Star", new StarScan {scan=false,value=2895}},
            { "Herbig Ae/Be Star", new StarScan {scan=false,value=3077}},
            { "Wolf-Rayet Star", new StarScan {scan=false,value=2931}},
            { "Wolf-Rayet N Star", new StarScan {scan=false,value=2931}},
            { "Wolf-Rayet NC Star", new StarScan {scan=false,value=2931}},
            { "Wolf-Rayet C Star", new StarScan {scan=false,value=2931}},
            { "Wolf-Rayet O Star", new StarScan {scan=false,value=2931}},
            { "C Star", new StarScan {scan=false,value=2930}},
            { "CN Star", new StarScan {scan=false,value=2930}},
            { "CJ Star", new StarScan {scan=false,value=2930}},
            { "MS-type Star", new StarScan {scan=false,value=2930}},
            { "S-type Star", new StarScan {scan=false,value=2930}},
            { "D (White Dwarf) Star", new StarScan {scan=true,value=34294}},
            { "Neutron Star", new StarScan {scan=true,value=54782}},
            { "Black Hole", new StarScan {scan=true,value=60589 }}
        };

        public static Dictionary<string, BodyScan> Bodies = new Dictionary<string, BodyScan> {
            { "Metal-rich body", new BodyScan{scan=true, check_terraform=false, value=65045 } },
            { "High metal content world", new BodyScan{scan=true, check_terraform=true, value=34310 } },
            { "Rocky body", new BodyScan{scan=false, check_terraform=true, value=928 } },
            { "Rocky Ice world", new BodyScan{scan=false, check_terraform=false, value=0 } },
            { "Icy body", new BodyScan{scan=false, check_terraform=false, value=1246 } },
            { "Earth-like world", new BodyScan{scan=true, check_terraform=false, value=627885 } },
            { "Water world", new BodyScan{scan=true, check_terraform=true, value=301410 } },
            { "Water giant", new BodyScan{scan=false, check_terraform=false, value=1824 } },
            { "Ammonia world", new BodyScan{scan=true, check_terraform=false, value=320203 } },
            { "Gas giant with water-based life", new BodyScan{scan=false, check_terraform=false, value=2314 } },
            { "Gas giant with ammonia-based life", new BodyScan{scan=false, check_terraform=false, value=1721 } },
            { "Class I gas giant", new BodyScan{scan=false, check_terraform=false, value=7013 } },
            { "Class II gas giant", new BodyScan{scan=true, check_terraform=false, value=53663 } },
            { "Class III gas giant", new BodyScan{scan=false, check_terraform=false, value=2693 } },
            { "Class IV gas giant", new BodyScan{scan=false, check_terraform=false, value=2799 } },
            { "Class V gas giant", new BodyScan{scan=false, check_terraform=false, value=2761 } },
            { "Helium-rich gas giant", new BodyScan{scan=true, check_terraform=false, value=2095 } }
        };
    }
}
