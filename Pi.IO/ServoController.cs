using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using UnoPi = Unosquare.RaspberryIO.Pi;

namespace Pi.IO
{
    public class ServoController : IPWMServoController
    {
        public IUnoGpioPin GpioPin { set; private get; }


       // GpioPin pin = UnoPi.Gpio[BcmPin.Gpio24] as GpioPin;

        public ServoController(IUnoGpioPin gpioPin)
        {
            GpioPin = gpioPin;
            GpioPin.PinMode = GpioPinDriveMode.PwmOutput;
            GpioPin.PwmMode = PwmMode.Balanced;
            GpioPin.PwmClockDivisor = 2;
        }

        public void ReadPWM()
        {
            throw new NotImplementedException();

        }

        public void WritePWM()
        {
            throw new NotImplementedException();

        }
    }
}
