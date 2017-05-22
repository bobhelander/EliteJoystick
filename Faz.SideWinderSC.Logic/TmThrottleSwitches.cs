﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faz.SideWinderSC.Logic
{
    public static class TmThrottleSwitches
    {
        static AllSwitches[] DefaultState = new AllSwitches[]
        {
            AllSwitches.MOUSENONE,
            AllSwitches.MSNONE,
            //AllSwitches.CSNONE,
            AllSwitches.SPDM,
            AllSwitches.BSM,
            AllSwitches.CHM,
            AllSwitches.PSM,
            AllSwitches.LTBNONE,
            AllSwitches.EFLOVER,
            AllSwitches.EFROVER,
            AllSwitches.EOLNORM,
            AllSwitches.EORNORM,
            AllSwitches.APUOFF,
            AllSwitches.LDGHNONE,
            AllSwitches.FLAPM,
            AllSwitches.EACOFF,
            AllSwitches.RDRDIS,
            AllSwitches.APENGNONE,
            AllSwitches.APAH,
            AllSwitches.IDLELOFF,
            AllSwitches.IDLEROFF
        };

        public enum AllSwitches
        {
            // MOUSE BUTTON
            MOUSENONE,
            MOUSEP,
            // MIC SWITCH
            MSNONE,
            MSP,
            MSU,
            MSR,
            MSD,
            MSL,
            // COOLIE SWITCH
            //CSNONE,
            //CSU,
            //CSR,
            //CSD,
            //CSL,
            // SPEEDBRAKE
            SPDF,
            SPDM,
            SPDB,
            // BOAT SWITCH
            BSF,
            BSM,
            BSB,
            // CHINA HAT
            CHF,
            CHM,
            CHB,
            // PINKY SWITCH
            PSF,
            PSM,
            PSB,
            // LEFT THROTTLE BUTTON
            LTBNONE,
            LTBP,
            // FUEL LEFT
            EFLNORM,
            EFLOVER,
            // FUEL RIGHT
            EFRNORM,
            EFROVER,
            // ENG LEFT 
            EOLIGN,
            EOLNORM,
            EOLMOTOR,
            // ENG RIGHT
            EORIGN,
            EORNORM,
            EORMOTOR,
            // APU
            APUON,
            APUOFF,
            // L/G WRN
            LDGHNONE,
            LDGHP,
            // FLAPS																
            FLAPU,
            FLAPM,
            FLAPD,
            // EAC
            EACON,
            EACOFF,
            // RDR
            RDRNRM,
            RDRDIS,
            // AUTOPILOT
            APENGNONE,
            APENGP,
            // LASTE
            APPAT,
            APAH,
            APALT,
            // LEFT IDLE
            IDLELOFF,
            IDLELON,
            // RIGHT IDLE
            IDLEROFF,
            IDLERON
        };
    };
}