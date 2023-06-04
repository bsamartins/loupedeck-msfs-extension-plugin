namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;

    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    internal class AltitudeEncoder : AirbusFCUEncoder
    {
        private Int64 _selected = 0;
        private Int64 _indicated = 0;
        public AltitudeEncoder() : base("ALT", "Altitude", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var setValue = this._selected < 0 ? "" : this._selected.ToString();
            return $"[{setValue}]\n{this._indicated}";
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (diff < 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.AP_ALT_DEC);
                //SimConnectService.Instance.SendCommand(SendEvent.FBW_AP_ALT_DEC);
            }
            else if (diff > 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.AP_ALT_INC);
                //SimConnectService.Instance.SendCommand(SendEvent.FBW_AP_ALT_INC);
            }
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
            PluginLog.Info($"Got airbus info: {e.AutoPilotAltitudeLockVar} / {e.AutopilotAltitudeIncrement} / {e.IndicatedAltitude}");
            this._selected = e.AutoPilotAltitudeLockVar;
            this._indicated = e.IndicatedAltitude;
            this.Managed = e.FcVsManaged;
            //this._step = e.AutopilotAltitudeIncrement;
            this.AdjustmentValueChanged();
        }
    }
}
