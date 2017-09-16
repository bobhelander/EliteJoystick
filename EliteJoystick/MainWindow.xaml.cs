using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using vJoyInterfaceWrap;

namespace EliteJoystick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        // Declaring one joystick (Device id 1) and a position structure. 
        //static public vJoy joystick;
        static public vJoy.JoystickState iReport;

        private EliteControllers eliteControllers = null;
        private EliteVirtualJoysticks eliteVirtualJoysticks = null;
        private vJoyMapper vJoyMapper = new vJoyMapper();
        private Settings settings = Settings.Load();

        private IpcServer ipcServer = null;

        ArduinoCommunication.Arduino arduino = null;

        public MainWindow()
        {
            InitializeComponent();

            vJoyMappingList.DataContext = vJoyMapper;
            launchAppList.DataContext = settings.StartUpApplications;
        }

        public void StartControllers()
        {
            try
            {
                EliteSharedState sharedState = new EliteSharedState { OrbitLines = true, HeadsUpDisplay = true };
                eliteControllers = new EliteControllers();

                eliteVirtualJoysticks = new EliteVirtualJoysticks();

                for (uint joyId = 1; joyId <= 6; joyId++)
                {
                    eliteVirtualJoysticks.Controllers.Add(new EliteVirtualJoystick
                    {
                        Joystick = eliteVirtualJoysticks.Joystick,
                        JoystickId = joyId,
                        VisualState = new VisualState { Name = "vJoy " + joyId }
                    });
                }

                eliteVirtualJoysticks.Initialize();

                //ipcServer = new IpcServer();
                //ipcServer.Start();

                if (true == arduinoCheckBox.IsChecked)
                {
                    arduino = new ArduinoCommunication.Arduino(settings.ArduinoCommPort);
                }

                if (true == swff2CheckBox.IsChecked)
                {
                    eliteControllers.Controllers.Add(
                        Sidewinder.ForceFeedback2.Swff2Controller.Create(
                            sharedState, 
                            eliteVirtualJoysticks.Joystick, 
                            vJoyMapper,
                            arduino));
                }

                if (true == tmWarthogCheckBox.IsChecked)    
                {
                    eliteControllers.Controllers.Add(
                        Thrustmaster.Warthog.TmThrottleController.Create(
                            sharedState, 
                            eliteVirtualJoysticks.Joystick,
                            vJoyMapper,
                            arduino));
                }

                if (true == chPedalsCheckBox.IsChecked)
                {
                    eliteControllers.Controllers.Add(
                        ChProducts.ChPedalsController.Create( 
                            sharedState, 
                            eliteVirtualJoysticks.Joystick, 
                            vJoyMapper));
                }

                if (true == scCheckBox.IsChecked)
                {
                    eliteControllers.Controllers.Add(
                        Sidewinder.Commander.ScController.Create( 
                            sharedState, 
                            eliteVirtualJoysticks.Joystick, 
                            vJoyMapper));
                }

                if (true == gvCheckBox.IsChecked)
                {
                    eliteControllers.Controllers.Add(
                        Sidewinder.GameVoice.SwGvController.Create( 
                            sharedState, 
                            eliteVirtualJoysticks.Joystick, 
                            vJoyMapper));
                }
                if (true == bbi32CheckBox.IsChecked)
                {
                    eliteControllers.Controllers.Add(
                        Other.BBI32.ButtonBoxController.Create(
                            sharedState,
                            eliteVirtualJoysticks.Joystick,
                            vJoyMapper,
                            arduino));
                }
                
                if (true == kpCheckBox.IsChecked)
                {
                    eliteControllers.Controllers.Add(
                        GearHead.Keypad.GhKpController.Create(
                            sharedState,
                            eliteVirtualJoysticks.Joystick,
                            vJoyMapper));
                }

                controllersList.DataContext = eliteControllers;
                //vControllersList.DataContext = eliteVirtualJoysticks;                

                eliteControllers.Initialize();
                //textBox.Text = "ready";

                ushort version = (ushort)Utils.GetvJoyVersion();
                //Utils.RefreshvJoySpecific(version);

                //var test = Utils.Disable(version);

                textBox.Text = String.Format("ready: {0}", version.ToString());

                //strategicCommander = Sidewinder.Commander.ScController.GetController();
                //gameVoice = SwGvController.GetController();
                //chPedals = ChProducts.ChPedalsController.GetController();
                //swff2 = Sidewinder.ForceFeedback2.Swff2Controller.GetController();
                //tmThrottle = Thrustmaster.Warthog.TmThrottleController.GetController();

                //strategicCommander.SharedState = sharedState;
                //tmThrottle.SharedState = sharedState;

                //var gameVoice = Faz.SideWinderSC.Logic.SwgvController.RetrieveAll().First();
                //gameVoice.SetLights(new Faz.SideWinderSC.Logic.SwscLight[] { Faz.SideWinderSC.Logic.SwscLight.Button1 });

                //var pedals = Faz.SideWinderSC.Logic.CHPedalsController.RetrieveAll().First();

                //pedals.Initialize();

                //var ff2 = Faz.SideWinderSC.Logic.Swff2Controller.RetrieveAll().First();
                //ff2.Initialize();

                //var throttle = Faz.SideWinderSC.Logic.TMWartHogThrottleController.RetrieveAll().First();
                //throttle.Initialize();

                // Create one joystick object and a position structure.
                //joystick = new vJoy();
                //iReport = new vJoy.JoystickState();


                //// Device ID can only be in the range 1-16
                //if (args.Length > 0 && !String.IsNullOrEmpty(args[0]))
                //    id = Convert.ToUInt32(args[0]);
                //if (id <= 0 || id > 16)
                //{
                //    Console.WriteLine("Illegal device ID {0}\nExit!", id);
                //    return;
                //}

                // Get the driver attributes (Vendor ID, Product ID, Version Number)
                //if (!joystick.vJoyEnabled())
                //{
                //    Console.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                //    return;
                //}
                //else
                //    Console.WriteLine("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", joystick.GetvJoyManufacturerString(), joystick.GetvJoyProductString(), joystick.GetvJoySerialNumberString());

                // Get the state of the requested device
                //VjdStat status = joystick.GetVJDStatus(id);
                //switch (status)
                //{
                //    case VjdStat.VJD_STAT_OWN:
                //        Console.WriteLine("vJoy Device {0} is already owned by this feeder\n", id);
                //        break;
                //    case VjdStat.VJD_STAT_FREE:
                //        Console.WriteLine("vJoy Device {0} is free\n", id);
                //        break;
                //    case VjdStat.VJD_STAT_BUSY:
                //        Console.WriteLine("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id);
                //        return;
                //    case VjdStat.VJD_STAT_MISS:
                //        Console.WriteLine("vJoy Device {0} is not installed or disabled\nCannot continue\n", id);
                //        return;
                //    default:
                //        Console.WriteLine("vJoy Device {0} general error\nCannot continue\n", id);
                //        return;
                //};

                // Check which axes are supported
                //bool AxisX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
                //bool AxisY = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
                //bool AxisZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z);
                //bool AxisRX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
                //bool AxisRZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
                //// Get the number of buttons and POV Hat switchessupported by this vJoy device
                //int nButtons = joystick.GetVJDButtonNumber(id);
                //int ContPovNumber = joystick.GetVJDContPovNumber(id);
                //int DiscPovNumber = joystick.GetVJDDiscPovNumber(id);

                // Print results
                //Console.WriteLine("\nvJoy Device {0} capabilities:\n", id);
                //Console.WriteLine("Numner of buttons\t\t{0}\n", nButtons);
                //Console.WriteLine("Numner of Continuous POVs\t{0}\n", ContPovNumber);
                //Console.WriteLine("Numner of Descrete POVs\t\t{0}\n", DiscPovNumber);
                //Console.WriteLine("Axis X\t\t{0}\n", AxisX ? "Yes" : "No");
                //Console.WriteLine("Axis Y\t\t{0}\n", AxisX ? "Yes" : "No");
                //Console.WriteLine("Axis Z\t\t{0}\n", AxisX ? "Yes" : "No");
                //Console.WriteLine("Axis Rx\t\t{0}\n", AxisRX ? "Yes" : "No");
                //Console.WriteLine("Axis Rz\t\t{0}\n", AxisRZ ? "Yes" : "No");

                //// Test if DLL matches the driver
                //UInt32 DllVer = 0, DrvVer = 0;
                //bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
                //if (match)
                //    Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
                // else
                //    Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);


                // Acquire the target
                //if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
                //{
                //    Console.WriteLine("Failed to acquire vJoy device number {0}.\n", id);
                //    return;
                //}
                //else
                //    Console.WriteLine("Acquired: vJoy device number {0}.\n", id);

                //int X, Y, Z, ZR, XR;
                //uint count = 0;
                //long maxval = 0;

                //X = 20;
                ///Y = 30;
                //Z = 40;
                //XR = 60;
                //ZR = 80;

                //joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxval);

                //bool res;
                // Reset this device to default values
                //joystick.ResetVJD(id);

                //strategicCommander.vJoy = joystick;

                //strategicCommander.Controller.Initialize();

                //gameVoice.vJoy = joystick;
                //gameVoice.Controller.Initialize();

                //chPedals.vJoy = joystick;
                //chPedals.Controller.Initialize();

                //swff2.vJoy = joystick;
                //swff2.Controller.Initialize();

                //tmThrottle.vJoy = joystick;
                //tmThrottle.vJoyId = 1;
                //tmThrottle.Controller.Initialize();

                //// Feed the device in endless loop
                //while (true)
                //{
                //    // Set position of 4 axes
                //    res = joystick.SetAxis(X, id, HID_USAGES.HID_USAGE_X);
                //    res = joystick.SetAxis(Y, id, HID_USAGES.HID_USAGE_Y);
                //    res = joystick.SetAxis(Z, id, HID_USAGES.HID_USAGE_Z);
                //    res = joystick.SetAxis(XR, id, HID_USAGES.HID_USAGE_RX);
                //    res = joystick.SetAxis(ZR, id, HID_USAGES.HID_USAGE_RZ);

                //    // Press/Release Buttons
                //    res = joystick.SetBtn(true, id, count / 50);
                //    res = joystick.SetBtn(false, id, 1 + count / 50);

                //    // If Continuous POV hat switches installed - make them go round
                //    // For high values - put the switches in neutral state
                //    if (ContPovNumber > 0)
                //    {
                //        if ((count * 70) < 30000)
                //        {
                //            res = joystick.SetContPov(((int)count * 70), id, 1);
                //            res = joystick.SetContPov(((int)count * 70) + 2000, id, 2);
                //            res = joystick.SetContPov(((int)count * 70) + 4000, id, 3);
                //            res = joystick.SetContPov(((int)count * 70) + 6000, id, 4);
                //        }
                //        else
                //        {
                //            res = joystick.SetContPov(-1, id, 1);
                //            res = joystick.SetContPov(-1, id, 2);
                //            res = joystick.SetContPov(-1, id, 3);
                //            res = joystick.SetContPov(-1, id, 4);
                //        };
                //    };

                //    // If Discrete POV hat switches installed - make them go round
                //    // From time to time - put the switches in neutral state
                //    if (DiscPovNumber > 0)
                //    {
                //        if (count < 550)
                //        {
                //            joystick.SetDiscPov((((int)count / 20) + 0) % 4, id, 1);
                //            joystick.SetDiscPov((((int)count / 20) + 1) % 4, id, 2);
                //            joystick.SetDiscPov((((int)count / 20) + 2) % 4, id, 3);
                //            joystick.SetDiscPov((((int)count / 20) + 3) % 4, id, 4);
                //        }
                //        else
                //        {
                //            joystick.SetDiscPov(-1, id, 1);
                //            joystick.SetDiscPov(-1, id, 2);
                //            joystick.SetDiscPov(-1, id, 3);
                //            joystick.SetDiscPov(-1, id, 4);
                //        };
                //    };

                //    System.Threading.Thread.Sleep(20);
                //    X += 150; if (X > maxval) X = 0;
                //    Y += 250; if (Y > maxval) Y = 0;
                //    Z += 350; if (Z > maxval) Z = 0;
                //    XR += 220; if (XR > maxval) XR = 0;
                //    ZR += 200; if (ZR > maxval) ZR = 0;
                //    count++;

                //    if (count > 640)
                //        count = 0;

                //} // While (Robust)

                //testing = ScController.GetController();
                //testing.Controller.Initialize();
            }
            catch(Exception ex)
            {
                textBox.Text = ex.ToString();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            StartControllers();
        }

        private void startAllbutton_Click(object sender, RoutedEventArgs e)
        {
            arduinoCheckBox.IsChecked = true;
            swff2CheckBox.IsChecked = true;
            tmWarthogCheckBox.IsChecked = true;
            chPedalsCheckBox.IsChecked = true;
            scCheckBox.IsChecked = true;
            gvCheckBox.IsChecked = true;
            bbi32CheckBox.IsChecked = true;
            //kpCheckBox.IsChecked = true;

            StartControllers();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach(var launchApp in settings.StartUpApplications)
            {
                launchApp.LaunchApplication();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vJoyMapper.Save();
            settings.Save();
            eliteVirtualJoysticks?.Release();
        }
    }
}
