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
    public class ServoController : IPWMServoController
    {
        public ServoPin ServoPin { set; private get; }
        // GpioPin GpioPin = UnoPi.Gpio[BcmPin.Gpio24] as GpioPin;
        private int _CurrentAngle = 0;
        public int CurrentAngle
        {
            get { return _CurrentAngle; }
            set
            {
                if (_CurrentAngle != value) _CurrentAngle = value;
                OnPropertyChanged(nameof(CurrentAngle));
            }
        }

        const int _ServoAngleMin = 0;  // Angle corresponding to _ServoRegisterMinPulse
        const int _ServoAngleMax = 180;  // Angle corresponding to _ServoRegisterMaxPulse

        public ServoController(BcmPin bcmPin)
        {
            ServoPin = new ServoPin(bcmPin);
        }

        public int ReadPwm()
        {
            return ServoPin._Pin.PwmRegister;
        }
        public void WritePwm(int pwm)
        {
            ServoPin.WritePwm(pwm);
        }

        public int ReadAngle()
        {
            double percentagePwm = (ServoPin._Pin.PwmRegister- ServoPin._ServoRegisterMinPulse) / (double)(ServoPin._ServoRegisterMaxPulse - ServoPin._ServoRegisterMinPulse);
            int angle = _ServoAngleMax - (int)(_ServoAngleMax * percentagePwm);
            
            return angle;
            
        }

        public void WriteAngle(int angle)
        {      
            Console.WriteLine("writing angle: " + angle);
            if (angle > _ServoAngleMax || angle < _ServoAngleMin) return;

            int invertAngle = _ServoAngleMax - angle;
            int newPwmRegister = (int)((double)invertAngle / (double)_ServoAngleMax * (double)(ServoPin._ServoRegisterMaxPulse - ServoPin._ServoRegisterMinPulse)) + ServoPin._ServoRegisterMinPulse;

            ServoPin.WritePwm(newPwmRegister);
            CurrentAngle = ReadAngle();
        }

        public void DecreaseAngle(int amount = 5)
        {
            Console.WriteLine("decrease Angle");
            ServoPin.IncreasePwmPulse(amount);
            CurrentAngle = ReadAngle();
            Console.WriteLine("angle: " + CurrentAngle);

        }

        public void IncreaseAngle(int amount = 5)
        {
            Console.WriteLine("increase angle");
            ServoPin.DecreasePwmPulse(amount);
            CurrentAngle = ReadAngle();
            Console.WriteLine("angle: " + CurrentAngle);
        }

        public void TurnOff()
        {
            try
            {
                using var gpioController = new MicrosoftGpio.GpioController();
                gpioController.ClosePin(upPinGpioNum);
                gpioController.ClosePin(downPinGpioNum);
            }
            catch (Exception e)
            { //if closing fails we dont mind (maybe UnoSqaure was used instead of Microsoft.Gpio)
            }
            ServoPin.ReleasePin();
        }

        public void Dispose()
        {
            TurnOff();
        }

        int upPinGpioNum = 23;
        int downPinGpioNum = 24;

        public async Task ListenForButtonsMicrosoftGpio(int upGpioPin, int downGpioPin, CancellationToken cancellationToken)
        {
            upPinGpioNum = upGpioPin; downPinGpioNum = downGpioPin;

            using var gpioController = new MicrosoftGpio.GpioController();
            gpioController.OpenPin(upPinGpioNum);
            gpioController.OpenPin(downPinGpioNum);
            gpioController.SetPinMode(upPinGpioNum, MicrosoftGpio.PinMode.InputPullUp);
            gpioController.SetPinMode(downPinGpioNum, MicrosoftGpio.PinMode.InputPullUp);

            gpioController.RegisterCallbackForPinValueChangedEvent(upPinGpioNum, MicrosoftGpio.PinEventTypes.Rising, UpPinEventHandler);
            gpioController.RegisterCallbackForPinValueChangedEvent(downPinGpioNum, MicrosoftGpio.PinEventTypes.Rising, DownPinEventHandler);

            var checkForCancelationInterval = new TimeSpan(0, 0, 1);
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("listening...");
                await Task.Delay(checkForCancelationInterval);
            }

            Console.WriteLine("UseButtons listening task canceled");

        }


        public void ListenForButtons(BcmPin upBcmPin, BcmPin downBcmPin)
        {

            //----------- unosquare 

            var upPin = (GpioPin)UnoPi.Gpio[upBcmPin];
            upPin.PinMode = GpioPinDriveMode.Input;
            upPin.InputPullMode = GpioPinResistorPullMode.PullUp;
            upPin.RegisterInterruptCallback(EdgeDetection.FallingEdge, UpPinEventHandler);

            var downPin = (GpioPin)UnoPi.Gpio[downBcmPin];
            downPin.PinMode = GpioPinDriveMode.Input;
            downPin.InputPullMode = GpioPinResistorPullMode.PullUp;
            downPin.RegisterInterruptCallback(EdgeDetection.FallingEdge, DownPinEventHandler);

        }



        public void UpPinEventHandler()
        {
            Console.WriteLine("up button pressed");
            IncreaseAngle(3);
            CurrentAngle = ReadAngle();
            Thread.Sleep(100);
        }

        public void DownPinEventHandler()
        {
            Console.WriteLine("down button pressed");
            DecreaseAngle(3);
            CurrentAngle = ReadAngle();
            Thread.Sleep(100);
        }

        public void UpPinEventHandler(object sender, MicrosoftGpio.PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            Console.WriteLine("Increase pulse");
            IncreaseAngle(10);
            CurrentAngle = ReadAngle();
        }

        public void DownPinEventHandler(object sender, MicrosoftGpio.PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            Console.WriteLine("decrease pulse");
           DecreaseAngle(10);
            CurrentAngle = ReadAngle();
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
