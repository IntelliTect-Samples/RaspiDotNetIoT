
namespace Pi.IO
{
    public interface IPWMServoController
    {
        void WritePwm(int pwm);
        void ReadPwm();
        /// <summary>
        /// Allow for mocking a Gpiopin for testing your hardware code
        /// </summary>
        //public IUnoGpioPin GpioPin { set; }
    }
}
