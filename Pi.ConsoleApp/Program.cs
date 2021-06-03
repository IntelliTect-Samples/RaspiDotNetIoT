using Pi.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using UnoPi = Unosquare.RaspberryIO.Pi;

namespace Pi.ConsoleApp
{
    public class Program
    {



        /// <summary>
        /// Console app for testing servo motor
        /// </summary>
        /// <param name="s">sweep between 0 and 180 degrees, and then finish at 90</param>
        /// <param name="g">get pwm value</param>
        /// <param name="angle">angle value to set</param>
        /// <param name="pwm">pwm value to set</param>
        /// <param name="buttons">listen for button events/param>
        /// 
        public static void Main(bool s = false, bool g = false, int angle = -1, int pwm = -1, bool buttons = false)
        {
            // Before start using RaspberryIO, you must initialize Pi class (bootstrapping process)
            // with the valid Abstractions implementation, in order to let Pi know what implementation is going to use:
            UnoPi.Init<BootstrapWiringPi>();

            using ServoController servoController = new ServoController(BcmPin.Gpio19);

            if (s) Sweep(servoController);
            if (angle != -1) SetAngle(angle, servoController);
            if (pwm != -1) SetPwm(pwm, servoController);
            if (buttons) Buttons(servoController);

        }

        private static void SetAngle(int angle, ServoController servoController)
        {

            Console.WriteLine("Setting Angle" + angle);
            servoController.WriteAngle(angle);
            Thread.Sleep(1000);

        }

        private static void Sweep(ServoController servoController)
        {
            for (int i = 0; i < 180; i++)
            {
                servoController.WriteAngle(i);

                Thread.Sleep(20);
            }
            Thread.Sleep(100);
            for (int i = 0; i < 180; i++)
            {
                servoController.WriteAngle(180 - i);

                Thread.Sleep(20);
            }
            Thread.Sleep(100);
            servoController.WriteAngle(90);
            Thread.Sleep(1000);
        }
        private static void SetPwm(int pwm, ServoController servoController)
        {

            Console.WriteLine("Setting Pwm");

            servoController.WritePwm(pwm);

            Thread.Sleep(2000);

        }

        private static void Buttons(ServoController servoController)
        {
            var UP_PIN = BcmPin.Gpio23;
            var DOWN_PIN = BcmPin.Gpio24;
            var cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => servoController.UseButtons(UP_PIN,DOWN_PIN, cancellationTokenSource.Token), TaskCreationOptions.LongRunning);
            Console.WriteLine("enter q to quit");

            while (true) {
               var input = Console.ReadLine();
               if (input.Contains("q")) break;
            }
            cancellationTokenSource.Cancel();

            Thread.Sleep(2000);

            
        }
    }
}
