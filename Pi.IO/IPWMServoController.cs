
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;

namespace Pi.IO
{
    public interface IPWMServoController: IDisposable, INotifyPropertyChanged 
    {
        public int CurrentAngle { get; set; }
        void WritePwm(int pwmPulse);
        int ReadPwm();
        /// <summary>
        /// Allow for mocking a Gpiopin for testing your hardware code
        /// </summary>
        //public IUnoGpioPin GpioPin { set; }
        public int ReadAngle();
        public void WriteAngle(int angle);
        public void IncreaseAngle(int amount = 10);
        public void DecreaseAngle(int amount = 10);       
        public void ListenForButtons(BcmPin upBcmPin, BcmPin downBcmPin);
        public Task ListenForButtonsMicrosoftGpio(int upGpioPin, int downGpioPin, CancellationToken cancellationToken);
        public void TurnOff();

    }
}
