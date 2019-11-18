using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using GoogleChrome;
using KeyboardMapping.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMapping
{
    public class Controller : vJoyMapping.Common.Controller
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private KeyboardController keyboardController;
        public IEliteGameStatus GameStatus { get; private set; }

        public void Initialize(KeyboardController keyboardController, IEliteGameStatus gameStatus)
        {
            this.keyboardController = keyboardController;
            GameStatus = gameStatus;
            MapControls(this.keyboardController);
        }

        public void MapControls(KeyboardController keyboard)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                keyboard.Subscribe(x => KeyboardCameraHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                keyboard.Subscribe(x => KeyboardFunctionKeysHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                keyboard.Subscribe(x => KeyboardNumberKeysHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                keyboard.Subscribe(x => KeyboardArrowKeysHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
            };
        }

        // {"VirutalKey":112,"ScanCode":59,"Flags":0 F1
        // {"VirutalKey":49,"ScanCode":2,"Flags":0}  1
        // {"VirutalKey":9,"ScanCode":15,"Flags":0}  Tab
        // {"VirutalKey":32,"ScanCode":57,"Flags":0} LeftSpace


        //{"VirutalKey":87,"ScanCode":17,"Flags":0} w
// {"VirutalKey":65,"ScanCode":30,"Flags":1}
//    a
// {"VirutalKey":83,"ScanCode":31,"Flags":1}
//s
// {"VirutalKey":68,"ScanCode":32,"Flags":1} d
 
// 37-40 38 = up 40 =down 37 = leftw 
// {"VirutalKey":38,"ScanCode":72,"Flags":3  up
//  {"VirutalKey":38,"ScanCode":72,"Flags":3} Left
 
    }
}
