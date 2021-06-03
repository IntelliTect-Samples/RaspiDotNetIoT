using System;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;

namespace Pi.IO
{
    public class ServoController : IPWMServoController ,IDisposable
    {
        public ServoPin ServoPin { set; private get; }
        // GpioPin GpioPin = UnoPi.Gpio[BcmPin.Gpio24] as GpioPin;


        public ServoController(BcmPin bcmPin)
        {
            ServoPin = new ServoPin(bcmPin);

        }

        public void ReadPwm()
        {
            throw new NotImplementedException();

        }
        public void WritePwm(int pwm)
        {
            ServoPin.WritePwm(pwm);

        }

        public void ReadAngle(int angle)
        {
            ServoPin.WriteAngle(angle);
           
        }

        public void WriteAngle(int angle)
        {
            ServoPin.WriteAngle(angle);
            
        }

        public void TurnOff() {
            ServoPin.ReleasePin();
        }

        public void Dispose()
        {
            TurnOff();
        }
    }
}
