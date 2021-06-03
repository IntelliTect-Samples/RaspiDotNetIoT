using System;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using UnoPi = Unosquare.RaspberryIO.Pi;

namespace Pi.IO
{
    public class ServoPin 
    {
        public GpioPin _Pin;
        public const int _ServoClockDivisor = 384; // 19.2MHz/384 = 50Hz
        public const int _ServoPWMRange = 2000; // If pwmClock is 192 (19.2MHz) and pwmRange is 2000 we'll get the PWM frequency = 50

        const int _ServoAngleMin = 0;  // Min pulse length out of 2000
        const int _ServoAngleMax = 180;  // Max pulse length out of 2000

        const int _ServoRegisterMin = 25;  // Min pulse length out of 2000
        const int _ServoRegisterMax = 110;  // Max pulse length out of 2000

        public ServoPin(BcmPin bcmPin)
        {
            _Pin = (GpioPin)UnoPi.Gpio[bcmPin];

            _Pin.PinMode = GpioPinDriveMode.PwmOutput;
            _Pin.PwmMode = PwmMode.MarkSign;
            _Pin.PwmClockDivisor = _ServoClockDivisor;
            _Pin.PwmRange = _ServoPWMRange; 
        }

        public void WriteAngle(int angle) {
            angle = _ServoAngleMax - angle;
            Console.WriteLine(angle);

            if (angle > _ServoAngleMax || angle < _ServoAngleMin) return;

            _Pin.PwmRegister = (int)((double)angle / (double)_ServoAngleMax * (double)(_ServoRegisterMax-_ServoRegisterMin)) + _ServoRegisterMin;

            Console.WriteLine(_Pin.PwmRegister);
        }

        public void WritePwm(int pwm)
        {
            _Pin.PwmRegister = pwm;

            Console.WriteLine(_Pin.PwmRegister);
        }

        public void ReleasePin() {
            _Pin.PinMode = GpioPinDriveMode.Input;
        }

    }
}