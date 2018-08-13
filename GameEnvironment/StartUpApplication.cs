using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEnvironment
{
    /// <summary>
    /// Object to hold the name and command line for a start up application
    /// </summary>
    public class StartUpApplication
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public bool Launch { get; set; }

        [JsonIgnore]
        private Process Process { get; set; }

        public void LaunchApplication()
        {
            try
            {
                if (Launch)
                    Process = Utils.Launch(Command);
            }
            catch (Exception)
            {
                //System.Windows.MessageBox.Show(ex.Message);
            }            
        }
    }
}
