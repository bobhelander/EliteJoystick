using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EliteRemoteJoystick
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var service1 = new EliteRemoteService();
                service1.TestStartupAndStop(args);
            }
            else
            {
                // Put the body of your old Main method here.  
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new EliteRemoteService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
