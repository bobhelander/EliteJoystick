using EliteJoystick.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common.Interfaces
{
    public interface IEdsmConnector
    {
        Task<StarSystem> GetSystem(string systemName);
    }
}
