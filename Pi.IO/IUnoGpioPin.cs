using System;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Pi.IO
{
    public interface IUnoGpioPin: IGpioPin
    {
        PwmMode PwmMode { get; set; }

        int PwmClockDivisor { get; set; }
    }
}
