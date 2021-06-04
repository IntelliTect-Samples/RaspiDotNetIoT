using System;
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
            return ServoPin._Pin.PwmRegister;
        }

        public void WriteAngle(int angle)
        {
            ServoPin.WriteAngle(angle);
        }

        public void IncreasePulse(int amount =0)
        {
            Console.WriteLine("Increase pulse");
            ServoPin.IncreasePwmPulse(amount);
            
        }

        public void DecreasePulse(int amount =10)
        {
            Console.WriteLine("decrease pulse");
            ServoPin.DecreasePwmPulse(amount);  
        }

        public void TurnOff()
        {
            try
            {
                using var gpioController = new MicrosoftGpio.GpioController();
                gpioController.ClosePin(upPinGpioNum);
                gpioController.ClosePin(downPinGpioNum);
            }
            catch (Exception e) { //if closing fails we dont mind (maybe UnoSqaure was used instead of Microsoft.Gpio)
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
