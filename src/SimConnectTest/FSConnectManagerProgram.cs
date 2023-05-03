using System;
using System.Runtime.InteropServices;
using System.Threading;

using CTrue.FsConnect;
using CTrue.FsConnect.Managers;

namespace SimConnectTest
{
    internal class FSConnectManagerProgram
    {
        public enum Requests
        {
            PlaneInfoRequest = 0,
            AircraftManager = 1
        }

        // Use field name and SimVar attribute to configure the data definition for the type.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct PlaneInfoResponse
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Title;
            [SimVar(Name = "L:A32NX_FLIGHT_CONTROLS_TRACKING_MODE", UnitId = FsUnit.Bool)]
            public bool FcTrackingMode;
            [SimVar(Name = "L:A32NX_FCU_VS_MANAGED", UnitId = FsUnit.Bool)]
            public bool FcVsManaged;
            [SimVar(Name = "L:A32NX_TRK_FPA_MODE_ACTIVE", UnitId = FsUnit.Bool)]
            public bool TrackFpaModeActive;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Test SimConnect");
            var fsConnect = new FsConnect();
            fsConnect.Connect("TestApp");

            var planeInfoDefinitionId = fsConnect.RegisterDataDefinition<PlaneInfoResponse>();

            var aircraftManager = new AircraftManager<PlaneInfoResponse>(fsConnect, planeInfoDefinitionId, (int) Requests.AircraftManager);

            aircraftManager.Updated += HandleReceivedFsData;

            // Set request method to continuously to start automatic updates using the Updated event.
            aircraftManager.RequestMethod = RequestMethod.Continuously;

            ConsoleKeyInfo cki;
            var escape = false;

            while(!escape)
            {
                aircraftManager.Get();
                cki = Console.ReadKey();
                escape = cki.Key != ConsoleKey.Escape;
                Thread.Sleep(1000);
            }

            fsConnect.Disconnect();

        }

        private static void HandleReceivedFsData(Object sender, AircraftInfoUpdatedEventArgs<PlaneInfoResponse> e) {
            var r = e.AircraftInfo;
            Console.WriteLine($"{r.FcTrackingMode} {r.FcVsManaged} {r.TrackFpaModeActive}");           
        }
    }
}
