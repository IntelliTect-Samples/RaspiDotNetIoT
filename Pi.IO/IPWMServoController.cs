
using System;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using MicrosoftGpio = System.Device.Gpio;

namespace Pi.IO
{
    public interface IPWMServoController: IDisposable
    {
        void WritePwm(int pwm);
        string ReadPwm();
        /// <summary>
        /// Allow for mocking a Gpiopin for testing your hardware code
        /// </summary>
        //public IUnoGpioPin GpioPin { set; }

        public void ReadAngle(int angle);
        public void WriteAngle(int angle);

        public void TurnOff();

        public void ListenForButtons(BcmPin upBcmPin, BcmPin downBcmPin);

        public Task ListenForButtonsMicrosoftGpio(int upGpioPin, int downGpioPin, CancellationToken cancellationToken);

        public void IncreasePulse(int amount = 10);

        public void DecreasePulse(int amount =10);

    }
}
