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
        /// <param name="s">sweep between 0 and 150 degrees, and then finish at 90</param>
        /// <param name="g">get pwm value</param>
        /// <param name="angle">angle value to set</param>
        /// <param name="pwm">pwm value to set</param>
        /// <param name="ub">listen for button events with UnoSquare GPIO/param>
        /// <param name="mb">listen for button events with Microsoft GPIO/param>
        /// 
        public static void Main(bool s = false, bool g = false, int angle = -1, int pwm = -1, bool ub = false,
            bool mb = false, bool hub=false, string url = "http://10.10.8.129:45455/CloudHub", bool debug=false )
        {
            if (debug)
            {
                Console.WriteLine(@"enter ""y"" once debugger is attached to continue...");
                while (true)
                { // so we have time to attach the debugger 
                    var proceed = Console.ReadLine();
                    if (proceed == "y") break;
                }
            }

            // Before start using RaspberryIO, you must initialize Pi class (bootstrapping process)
            // with the valid Abstractions implementation, in order to let Pi know what implementation is going to use:
            UnoPi.Init<BootstrapWiringPi>();

            using ServoController servoController = new ServoController(BcmPin.Gpio19);

            if (s) Sweep(servoController);
            if (angle != -1) SetAngle(angle, servoController);
            if (pwm != -1) SetPwm(pwm, servoController);
            if (ub) UnoSquareButtons(servoController);
            if (mb) MicrosoftButtons(servoController);



            if (hub) { InitializeHub(url, servoController); }

            Console.WriteLine(@"enter ""q"" close app...");
            while (true)
            { // so we have time to attach the debugger 
                var proceed = Console.ReadLine();
                if (proceed == "q") break;
            }

        }

        private static void InitializeHub(string url, ServoController servoController)
        {
            var UP_PIN = BcmPin.Gpio23;
            var DOWN_PIN = BcmPin.Gpio24;
            servoController.ListenForButtons(UP_PIN, DOWN_PIN);

            CloudHubConnection.Initialize(url, servoController);
            servoController.WriteAngle(servoController.ReadAngle()); //give the hub the 

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

        private static void UnoSquareButtons(ServoController servoController)
        {
            //if using UnoSquare
            var UP_PIN = BcmPin.Gpio23;
            var DOWN_PIN = BcmPin.Gpio24;

            servoController.ListenForButtons(UP_PIN, DOWN_PIN);
            Console.WriteLine("enter q to quit");

            while (true) {
               var input = Console.ReadLine();
               if (input.Contains("q")) break;
            }          
        }

        private static void MicrosoftButtons(ServoController servoController)
        {
            //if using UnoSquare
            /*var UP_PIN = BcmPin.Gpio23;
            var DOWN_PIN = BcmPin.Gpio24;*/

            //if using Microsoft Gpio
            int UP_PIN = 23;
            int DOWN_PIN = 24;

            var cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => servoController.ListenForButtonsMicrosoftGpio(UP_PIN, DOWN_PIN, cancellationTokenSource.Token), TaskCreationOptions.LongRunning);
            Console.WriteLine("enter q to quit");

            while (true)
            {
                var input = Console.ReadLine();
                if (input.Contains("q")) break;
            }

            //Microsoft
            cancellationTokenSource.Cancel();
            Thread.Sleep(2000);
        }
    }
}
