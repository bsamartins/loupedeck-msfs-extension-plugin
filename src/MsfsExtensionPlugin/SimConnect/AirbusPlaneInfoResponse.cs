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
        [SimVar(Name = "AUTOPILOT ALTITUDE LOCK VAR:3", UnitId = FsUnit.Feet)]
        public Int32 AutoPilotAltitudeLockVar;
        [SimVar(Name = "AUTOPILOT SPEED SLOT INDEX", UnitId = FsUnit.Number)]
        public Int32 AutopilotSpeedSlotIndex;
        [SimVar(Name = "L:A32NX_AUTOPILOT_SPEED_SELECTED", UnitId = FsUnit.Number)]
        public Int32 AutopilotSpeedSelected;
        [SimVar(Name = "AIRSPEED INDICATED", UnitId = FsUnit.Knots)]
        public Int32 AirspeedIndicated;
        [SimVar(Name = "L:A32NX_AUTOPILOT_HEADING_SELECTED", UnitId = FsUnit.Degree)]
        public Int32 AutopilotHeadingSelected;
        [SimVar(Name = "HEADING INDICATOR", UnitId = FsUnit.Degree)]
        public Int32 HeadingIndicator;
        [SimVar(Name = "AUTOPILOT HEADING SLOT INDEX", UnitId = FsUnit.Number)]
        public Int32 AutopilotHeadingSlotIndex;
        [SimVar(Name = "INDICATED ALTITUDE", UnitId = FsUnit.Feet)]
        public Int32 IndicatedAltitude;
        [SimVar(Name = "L:XMLVAR_AUTOPILOT_ALTITUDE_INCREMENT", UnitId = FsUnit.Feet)]
        public Int32 AutopilotAltitudeIncrement;
        [SimVar(Name = "L:A32NX_AUTOPILOT_VS_SELECTED", UnitId = FsUnit.FeetPerMinute)]
        public Int32 AutopilotVsSelected;
        [SimVar(Name = "L:A32NX_AUTOPILOT_FPA_SELECTED", UnitId = FsUnit.Degree)]
        public Double AutopilotFpaSelected;
    }
}
