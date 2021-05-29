using System;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Pi.IO
{
    public interface IPWMServoController
    {
        void WritePWM();
        void ReadPWM();
        /// <summary>
        /// Allow for mocking a Gpiopin for testing your hardware code
        /// </summary>
        public IUnoGpioPin GpioPin { set; }
    }
}
