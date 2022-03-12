using EliteJoystick.Common.Logic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vKeyboard;

namespace EliteJoystickServiceTest
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class vKeyboardTests
    {
        public vKeyboardTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public async Task MyTestInitialize()
        { 
            // Make sure notepad is running
            var process = EliteJoystick.Common.Utils.FocusWindow("notepad");
            if (string.IsNullOrEmpty(process))
                EliteJoystick.Common.Utils.Launch("notepad.exe");

            EliteJoystick.Common.Utils.FocusWindow("notepad");
            await Task.Delay(300).ConfigureAwait(false);
        }
        
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public async Task TestMethod1()
        {
            var keyboard = new Keyboard(null);

            await keyboard.PressKey("b", null, -1);
            await Task.Delay(100);
            await keyboard.ReleaseKey("b", null);
            await Task.Delay(100);

            await keyboard.PressKey("o", null, -1);
            await Task.Delay(100);
            await keyboard.ReleaseKey("o", null);
            await Task.Delay(100);

            await keyboard .PressKey("b", null, -1);
            await Task.Delay(100);
            await keyboard .ReleaseKey("b", null);
            await Task.Delay(100);
        }

        [TestMethod]
        public async Task TestTypeFullString()
        {
            var loggerFactory = LoggerFactory.Create(builder => 
                builder.AddSimpleConsole(options => { options.SingleLine = true; options.TimestampFormat = "hh:mm:ss "; }).SetMinimumLevel(LogLevel.Debug));
            
            ILogger<Keyboard> logger = loggerFactory.CreateLogger<Keyboard>();

            //var keyboard = new Keyboard(logger);
            var keyboard = new Keyboard(new NullLogger<Keyboard>());

            var enter = KeyMap.KeyNameMap["KEY_ENTER"].Code;

            await keyboard.TypeFullString("Hello World!");
            await keyboard.PressKey(enter);
        }

        [TestMethod]
        public async Task TestTyping()
        {
            var keyboard = new Keyboard(new NullLogger<Keyboard>());
            var enter = KeyMap.KeyNameMap["KEY_ENTER"].Code;
            var key_a = KeyMap.KeyNameMap["KEY_A"].Code;
            var key_c = KeyMap.KeyNameMap["KEY_C"].Code;
            var key_v = KeyMap.KeyNameMap["KEY_V"].Code;
            var key_delete = KeyMap.KeyNameMap["KEY_DELETE"].Code;
            var key_esc = KeyMap.KeyNameMap["KEY_ESC"].Code;
            var key_end = KeyMap.KeyNameMap["KEY_END"].Code;

            await keyboard.PressKey(key_a, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"] });
            await keyboard.PressKey(key_delete);

            await keyboard.TypeFullString("Blank Page");
            await keyboard.PressKey(enter);
            await keyboard.TypeFullString("Hello World!");
            await keyboard.PressKey(enter);

            await keyboard.PressKey(key_a, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"] });
            await keyboard.PressKey(key_c, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"] });
            await keyboard.PressKey(key_end, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"] });
            await keyboard.PressKey(enter);
            await keyboard.PressKey(key_v, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"] });
            await keyboard.PressKey(key_v, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"] });
            await keyboard.PressKey(key_v, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"] });
        }

        [TestMethod]
        public async Task TestHold()
        {
            var keyboard = new Keyboard(new NullLogger<Keyboard>());
            var enter = KeyMap.KeyNameMap["KEY_ENTER"].Code;
            var key_a = KeyMap.KeyNameMap["KEY_A"].Code;

            await keyboard.PressKey(key_a, null, -1);
            await Task.Delay(5000); // Five seconds
            await keyboard.PressKey(key_a, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LSHIFT"] }, -1);
            await Task.Delay(10000); // Five seconds
            await keyboard.ReleaseKey(0, new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LSHIFT"] });
            await Task.Delay(10000); // Five seconds
            await keyboard.ReleaseKey(key_a, null);
        }
    }
}
