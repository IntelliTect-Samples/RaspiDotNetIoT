using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Pi.IO
{
    class GpioPinMock : IUnoGpioPin
    {
        public BcmPin BcmPin => throw new NotImplementedException();

        public int BcmPinNumber => throw new NotImplementedException();

        public int PhysicalPinNumber => throw new NotImplementedException();

        public GpioHeader Header => throw new NotImplementedException();

        public GpioPinDriveMode PinMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GpioPinResistorPullMode InputPullMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PwmMode PwmMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int PwmClockDivisor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Read()
        {
            throw new NotImplementedException();
        }

        public void RegisterInterruptCallback(EdgeDetection edgeDetection, Action callback)
        {
            throw new NotImplementedException();
        }

        public void RegisterInterruptCallback(EdgeDetection edgeDetection, Action<int, int, uint> callback)
        {
            throw new NotImplementedException();
        }

        public void RemoveInterruptCallback(EdgeDetection edgeDetection, Action callback)
        {
            throw new NotImplementedException();
        }

        public void RemoveInterruptCallback(EdgeDetection edgeDetection, Action<int, int, uint> callback)
        {
            throw new NotImplementedException();
        }

        public bool WaitForValue(GpioPinValue status, int timeOutMillisecond)
        {
            throw new NotImplementedException();
        }

        public void Write(bool value)
        {
            throw new NotImplementedException();
        }

        public void Write(GpioPinValue value)
        {
            throw new NotImplementedException();
        }
    }
}
