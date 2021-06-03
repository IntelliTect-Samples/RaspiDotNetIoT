using System;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using MicrosoftGpio = System.Device.Gpio;

namespace Pi.IO
{
    public class ServoController : IPWMServoController, IDisposable
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

        public void TurnOff()
        {
            ServoPin.ReleasePin();
        }

        public void Dispose()
        {
            TurnOff();
        }

        public async Task UseButtons(BcmPin upBcmPin, BcmPin downBcmPin, CancellationToken cancellationToken)
        {
            int upPinGpioNum = 23;
            int downPinGpioNum = 24;

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
                await Task.Delay(checkForCancelationInterval);
            }

            gpioController.ClosePin(upPinGpioNum);
            gpioController.ClosePin(downPinGpioNum);


            //----------- unosquare 

            /*  var upPin = (GpioPin)UnoPi.Gpio[upBcmPin];
              upPin.PinMode = GpioPinDriveMode.Input;
              upPin.InputPullMode = GpioPinResistorPullMode.PullUp;
              upPin.RegisterInterruptCallback(EdgeDetection.FallingEdge, UpPinEventHandler);

              var downPin = (GpioPin)UnoPi.Gpio[downBcmPin];
              downPin.PinMode = GpioPinDriveMode.Input;
              downPin.InputPullMode = GpioPinResistorPullMode.PullUp;
              downPin.RegisterInterruptCallback(EdgeDetection.FallingEdge, DownPinEventHandler);

              var checkForCancelationInterval = new TimeSpan(0, 0, 1);
              while (!cancellationToken.IsCancellationRequested)
              {
                  Console.WriteLine("listening...");
                  await Task.Delay(checkForCancelationInterval);
              }

              Console.WriteLine("UseButtons listening task canceled");*/

        }



        public void UpPinEventHandler()
        {
            Console.WriteLine("Increase pulse");
            ServoPin.IncreasePwmPulse(10);
            Thread.Sleep(100);
        }

        public void DownPinEventHandler()
        {
            Console.WriteLine("decrease pulse");
            ServoPin.DecreasePwmPulse(10);
            Thread.Sleep(100);
        }

        public void UpPinEventHandler(object sender, MicrosoftGpio.PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            Console.WriteLine("Increase pulse");
            ServoPin.IncreasePwmPulse(10);
        }

        public void DownPinEventHandler(object sender, MicrosoftGpio.PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            Console.WriteLine("decrease pulse");
            ServoPin.DecreasePwmPulse(10);
        }
    }
}
