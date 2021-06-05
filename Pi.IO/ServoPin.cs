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


        public const int _ServoRegisterMinPulse = 28;  // Min pulse length out of 2000
        public const int _ServoRegisterMaxPulse = 110;  // Max pulse length out of 2000

        private int _CurrentPwmRegister = 0;

        public ServoPin(BcmPin bcmPin)
        {
            _Pin = (GpioPin)UnoPi.Gpio[bcmPin];

            _Pin.PinMode = GpioPinDriveMode.PwmOutput;
            _Pin.PwmMode = PwmMode.MarkSign;
            _Pin.PwmClockDivisor = _ServoClockDivisor;
            _Pin.PwmRange = _ServoPWMRange;
            WritePwm((_ServoRegisterMaxPulse + _ServoRegisterMinPulse) / 2); //set to middle
            
        }

        public void WritePwm(int pwm)
        {
            _Pin.PwmRegister = pwm;

            Console.WriteLine("wrote pwm: "+_Pin.PwmRegister);
        }

        public void ReleasePin() {
            _Pin.PinMode = GpioPinDriveMode.Input;
        }

        internal void IncreasePwmPulse(int increasePulse)
        {
            int newPulse = _Pin.PwmRegister+increasePulse;

            if (newPulse > _ServoRegisterMaxPulse) { WritePwm(_ServoRegisterMaxPulse); return; }

            WritePwm(newPulse);
           
        }

        internal void DecreasePwmPulse(int decreasePulse)
        {
            int newPulse = _Pin.PwmRegister - decreasePulse;

            if (newPulse < _ServoRegisterMinPulse) { WritePwm(_ServoRegisterMinPulse); return; }

            WritePwm(newPulse);
        }

    }
}