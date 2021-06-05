using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using MicrosoftGpio = System.Device.Gpio;
using UnoPi = Unosquare.RaspberryIO.Pi;

namespace Pi.IO
{
    public class MockServoController : IPWMServoController
    {
        public ServoPin ServoPin { set; private get; }
        // GpioPin GpioPin = UnoPi.Gpio[BcmPin.Gpio24] as GpioPin;
        private int _CurrentAngle = 0;
        private BcmPin gpio19;

        public MockServoController(BcmPin gpio19)
        {
            this.gpio19 = gpio19;
        }

        public int CurrentAngle
        {
            get { return _CurrentAngle; }
            set
            {
                if (_CurrentAngle != value) _CurrentAngle = value;
                OnPropertyChanged(nameof(CurrentAngle));
            }
        }

        public void WritePwm(int pwm)
        {
            CurrentAngle = pwm;
        }

        public int ReadPwm()
        {
            return 42;
        }

        public int ReadAngle()
        {
            return 42;
        }

        public void WriteAngle(int angle)
        {
            CurrentAngle = angle;
        }

        public void TurnOff()
        {
            
        }

        public void ListenForButtons(BcmPin upBcmPin, BcmPin downBcmPin)
        {
           
        }

        public Task ListenForButtonsMicrosoftGpio(int upGpioPin, int downGpioPin, CancellationToken cancellationToken)
        {
            return new Task(()=>Console.WriteLine());
        }

        public void IncreaseAngle(int amount = 10)
        {
            CurrentAngle += amount;
        }

        public void DecreaseAngle(int amount = 10)
        {
            CurrentAngle -= amount;
        }

        public void Dispose()
        {
           
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    
        #endregion
    }
}
