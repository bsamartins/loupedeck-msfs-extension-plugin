namespace Loupedeck.MsfsExtensionPlugin.SimConnect
{
    using System;
    using System.Runtime.InteropServices;

    using CTrue.FsConnect;

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
}
