using EliteJoystick.Common;
using Microsoft.Extensions.Logging;

namespace EliteDesktopApp
{
    public partial class Form1 : Form
    {
        private readonly JoystickService _service;
        private readonly ILogger<Form1>? _log;

        public Form1(
            JoystickService service,
            ILogger<Form1> log)
        {
            this._service = service;
            this._log = log;

            InitializeComponent();

            _log?.LogDebug("Starting Service from form");
            _service.Start();
        }
    }
}