using System;
using System.Linq;
using System.Runtime.InteropServices;

using CTrue.FsConnect;
using CTrue.FsConnect.Managers;

using Microsoft.FlightSimulator.SimConnect;

namespace SimConnectTest
{
    internal class FSConnectProgram
    {
        public enum Requests
        {
            PlaneInfoRequest = 0,
        }

        // Use field name and SimVar attribute to configure the data definition for the type.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct PlaneInfoResponse
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Title;
            [SimVar(UnitId = FsUnit.Degree)]
            public double PlaneLatitude;
            [SimVar(UnitId = FsUnit.Degree)]
            public double PlaneLongitude;
            [SimVar(UnitId = FsUnit.Feet)]
            public double PlaneAltitude;
            [SimVar(UnitId = FsUnit.Degree)]
            public double PlaneHeadingDegreesTrue;
            [SimVar(NameId = FsSimVar.AirspeedTrue, UnitId = FsUnit.MeterPerSecond)]
            public double AirspeedTrueInMeterPerSecond;
            [SimVar(NameId = FsSimVar.AirspeedTrue, UnitId = FsUnit.Knot)]
            public double AirspeedTrueInKnot;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Test SimConnect");
            FsConnect fsConnect = new FsConnect();
            fsConnect.Connect("TestApp");
            fsConnect.FsDataReceived += HandleReceivedFsData;

            int planeInfoDefinitionId = fsConnect.RegisterDataDefinition<PlaneInfoResponse>();

            ConsoleKeyInfo cki;

            do
            {
                fsConnect.RequestData((int) Requests.PlaneInfoRequest, planeInfoDefinitionId);
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);

            fsConnect.Disconnect();

        }

        private static void HandleReceivedFsData(Object sender, FsDataReceivedEventArgs e) {
            if (e.Data == null || e.Data.Count == 0)
                return;

            if (e.RequestId == (uint)Requests.PlaneInfoRequest)
            {
                PlaneInfoResponse r = (PlaneInfoResponse)e.Data.FirstOrDefault();
                Console.WriteLine($"{r.PlaneLatitude:F4} {r.PlaneLongitude:F4} {r.PlaneAltitude:F1}ft {r.PlaneHeadingDegreesTrue:F1}deg {r.AirspeedTrueInMeterPerSecond:F0}m/s {r.AirspeedTrueInKnot:F0}kt");
            }
        }
    }
}
