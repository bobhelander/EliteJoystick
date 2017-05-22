using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class EliteControllers
    {
        public EliteControllers()
        {
            Controllers = new List<Controller>();
        }

        public List<Controller> Controllers { get; set; }

        public void Initialize()
        {
            foreach (var controller in Controllers)
                controller.Initialize();
        }
    }
}
