using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /// <summary>
    /// Container for the Controller objects
    /// </summary>
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
