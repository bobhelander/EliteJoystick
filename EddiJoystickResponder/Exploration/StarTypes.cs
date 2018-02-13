using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddiJoystickResponder.Exploration
{
    public class StarTypes
    {
        public static Dictionary<string, BodyScan> Stars = new Dictionary<string, BodyScan> {
            { "A" /*"A (Blue-White) Star"*/, new BodyScan {scan=false,value=2949}},
            { "AEBE" /*"Herbig Ae/Be Star"*/, new BodyScan {scan=false,value=3077}},
            { "B" /*"B (Blue-White) Star"*/, new BodyScan {scan=false,value=3012}},
            { "C" /*"C Star"*/, new BodyScan {scan=false,value=2930}},
            { "CH" /*"CN Star"*/, new BodyScan {scan=false,value=2930}},
            { "CHd" /*"CN Star"*/, new BodyScan {scan=false,value=2930}},
            { "CJ" /*"CJ Star"*/, new BodyScan {scan=false,value=2930}},
            { "CN" /*"CN Star"*/, new BodyScan {scan=false,value=2930}},
            { "CS" /*"CN Star"*/, new BodyScan {scan=false,value=2930}},
            { "D" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DA" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DAO" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DAV" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DAZ" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DB" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DBV" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DBZ" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DC" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DCV" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DO" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DOV" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DQ" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "DX" /*"D (White Dwarf) Star"*/, new BodyScan {scan=true,value=34294}},
            { "F" /*"F (White) Star"*/, new BodyScan {scan=false,value=2932}},
            { "G" /*"G (White-Yellow) Star"*/, new BodyScan {scan=false,value=2919}},
            { "K" /*"K (Yellow-Orange) Star"*/, new BodyScan {scan=false,value=2916}},
            { "L" /*"L (Brown dwarf) Star"*/, new BodyScan {scan=false,value=2889}},
            { "M" /*"M (Red dwarf) Star"*/, new BodyScan {scan=false,value=2903}},
            { "MS" /*"MS-type Star"*/, new BodyScan {scan=false,value=2930}},
            { "N" /*"Neutron Star"*/, new BodyScan {scan=true,value=54782}},
            { "O" /*"O (Blue-White) Star"*/, new BodyScan {scan = false, value = 6135 } },
            { "S" /*"S-type Star"*/, new BodyScan {scan=false,value=2930}},
            { "T" /*"T Tauri Star"*/, new BodyScan {scan=false,value=2895}},
            { "TTS" /*"T Tauri Star"*/, new BodyScan {scan=false,value=2895}},
            { "W" /*"Wolf-Rayet Star"*/, new BodyScan {scan=false,value=2931}},
            { "WC" /*"Wolf-Rayet C Star"*/, new BodyScan {scan=false,value=2931}},
            { "WN" /*"Wolf-Rayet N Star"*/, new BodyScan {scan=false,value=2931}},
            { "WNC" /*"Wolf-Rayet NC Star"*/, new BodyScan {scan=false,value=2931}},
            { "WNO" /*"Wolf-Rayet No Star"*/, new BodyScan {scan=false,value=2931}},
            { "WO" /*"Wolf-Rayet O Star"*/, new BodyScan {scan=false,value=2931}},
            { "Y" /*"Y (Brown dwarf) Star"*/, new BodyScan {scan=false,value=2881}},
            
            { "H" /*"Black Hole"*/, new BodyScan {scan=true,value=60589 }}
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
