namespace EliteJoystick.Common.Interfaces
{
    public interface IForceFeedbackController : IJoystickServiceBase
    {
        bool CenterSpring { get; set; }
        bool Damper { get; set; }
        bool Vibration { get; set; }
        void StopAllEffects();
        void PlayFileEffect(string fileName, int duration);
    }
}
