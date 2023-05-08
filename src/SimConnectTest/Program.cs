using System;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.FlightSimulator.SimConnect;

namespace SimConnectTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test SimConnect");
            SimConnect simconnect = null;

            const int WM_USER_SIMCONNECT = 0x0402;

            try
            {
                simconnect = new SimConnect("Managed Data Request", IntPtr.Zero, WM_USER_SIMCONNECT, null, 0);
                simconnect.OnRecvOpen += Simconnect_OnRecvOpen;
                simconnect.OnRecvEvent += Simconnect_OnRecvEvent;
                simconnect.OnRecvSimobjectDataBytype += Simconnect_OnRecvSimobjectDataBytype;
                simconnect.MapClientEventToSimEvent((EventEnum)0, "A32NX.FCU_ALT_INC");
                simconnect.TransmitClientEvent(0, (EventEnum)0, 0, (EventEnum)0, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }
            catch (COMException ex)
            {

            }

            if (simconnect != null)
            {
                simconnect.Dispose();
                simconnect = null;
            }

            Thread.Sleep(-1);
        }

        private static void Simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data) => throw new NotImplementedException();

        private static void Simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data) {
            Console.WriteLine("receive open");
        }

        private static void Simconnect_OnRecvEvent(SimConnect sender, SIMCONNECT_RECV_EVENT data) {
            Console.WriteLine("Event");
        }
    }

    enum EventEnum {}
}
