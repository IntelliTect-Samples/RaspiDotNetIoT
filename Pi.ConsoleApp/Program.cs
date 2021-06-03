using Pi.IO;
using System;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using UnoPi = Unosquare.RaspberryIO.Pi;

namespace Pi.ConsoleApp
{
    public class Program
    {
        
        static ServoController ServoController;

        /// <summary>
        /// Console app for testing servo motor
        /// </summary>
        /// <param name="s">set pwm value</param>
        /// <param name="g">get pwm value</param>
        /// <param name="pwm">pwm value to set</param>
        /// <param name="buttons">listen for button events/param>
        /// 
        public static void Main(bool s = false, bool g = false, int pwm = -1, bool buttons = false)
        {
            // Before start using RaspberryIO, you must initialize Pi class (bootstrapping process)
            // with the valid Abstractions implementation, in order to let Pi know what implementation is going to use:
            UnoPi.Init<BootstrapWiringPi>();
            
            ServoController = new ServoController(BcmPin.Gpio19);


            if (s) SetAngle(pwm);
            if (pwm != -1) SetPwm(pwm);


        }

        private static void SetAngle(int pwm)
        {

            Console.WriteLine("Setting Angle");

            for (int i = 0; i < 180; i++) {
                ServoController.WriteAngle(i);
                
                Thread.Sleep(60);
            }
            ServoController.TurnOff();


        }
        private static void SetPwm(int pwm)
        {

            Console.WriteLine("Setting Pwm");

            
                ServoController.WritePwm(pwm);

                Thread.Sleep(2000);
            
            ServoController.TurnOff();


        }
    }
}
