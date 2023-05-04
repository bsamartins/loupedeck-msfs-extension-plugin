using System;
using System.Collections.Generic;
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
        public struct AirbusPlaneInfoResponse
        {
            [SimVar(Name = "L:A32NX_FLIGHT_CONTROLS_TRACKING_MODE", UnitId = FsUnit.Bool)]
            public Boolean FcTrackingMode;
            [SimVar(Name = "L:A32NX_FCU_VS_MANAGED", UnitId = FsUnit.Bool)]
            public Boolean FcVsManaged;
            [SimVar(Name = "L:A32NX_TRK_FPA_MODE_ACTIVE", UnitId = FsUnit.Bool)]
            public Boolean TrackFpaModeActive;
        }

        static void Main(String[] args)
        {
            Console.WriteLine("Test SimConnect");
            var fsConnect = new FsConnect();
            fsConnect.Connect("TestApp");

            var planeInfoDefinitionId = fsConnect.RegisterDataDefinition<AirbusPlaneInfoResponse>();
            var aircraftManager = new AircraftManager<AirbusPlaneInfoResponse>(fsConnect, planeInfoDefinitionId, (int) Requests.AircraftManager);

            aircraftManager.Updated += HandleReceivedFsData;
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

        private static void HandleReceivedFsData(Object sender, AircraftInfoUpdatedEventArgs<AirbusPlaneInfoResponse> e) {
            Console.WriteLine(sender);
            var r = e.AircraftInfo;
            Console.WriteLine($"{r.FcTrackingMode} {r.FcVsManaged} {r.TrackFpaModeActive}");           
        }
    }
}
