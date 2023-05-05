namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;

    using Loupedeck.MsfsExtensionPlugin.Helpers;
    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    internal class AltitudeEncoder : AirbusFCUEncoder
    {
        private Int64 _selected = 0;
        private Int64 _indicated = 0;
        private Int32 _step = 1000;
        public AltitudeEncoder() : base("Alt", "Altitude", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var setValue = this._selected < 0 ? "" : this._selected.ToString();
            return $"[{setValue}]\n{this._indicated}";
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            var value = ConvertTool.ApplyAdjustment(this._selected, diff, 100, 49999, this._step);
            this._selected = value;
            SimConnectService.Instance.SendCommand(SendEvent.AP_ALT_VAR_SET_ENGLISH, value);
            this.AdjustmentValueChanged();
        }

        protected override Boolean OnLoad() {
            SimConnectService.Instance.RegisterChangeHandler(this);
            return true;
        }

        protected override Boolean OnUnload() {
            SimConnectService.Instance.UnregisterChangeHandler(this);
            return true;
        }

        override public void OnAircraftChanged(AirbusPlaneInfoResponse e) {
            this._selected = e.AutoPilotAltitudeLockVar;
            this._indicated = e.IndicatedAltitude;
            this._step = e.AutopilotAltitudeIncrement;
            this.AdjustmentValueChanged();
        }
    }
}
