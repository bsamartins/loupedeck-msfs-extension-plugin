namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;

    using Loupedeck.MsfsExtensionPlugin.Events;
    using Loupedeck.MsfsExtensionPlugin.SimConnect;
    using Loupedeck.MsfsExtensionPlugin.Helpers;

    internal class SpeedEncoder : AirbusFCUEncoder
    {
        private Int64 _selectedSpeed = 0;
        private Int64 _indicatedSpeed = 0;
        public SpeedEncoder() : base("Speed", "Speed", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var setSpeed = this._selectedSpeed < 0 ? "" : this._selectedSpeed.ToString();
            return $"[{setSpeed}]\n{this._indicatedSpeed}";
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            var newValue = ConvertTool.ApplyAdjustment(this._selectedSpeed, diff, 0, 399, 1);
            this._selectedSpeed = newValue;            
            SimConnectService.Instance.SendCommand(SendEvent.AP_SPD_VAR_INC, 1);            
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
            PluginLog.Info($"Selected airspeed {e.AutopilotSpeedSelected}");
            this._selectedSpeed = e.AutopilotSpeedSelected;
            this._indicatedSpeed = e.AirspeedIndicated;
            this.Managed = e.AutopilotSpeedSlotIndex == 2;
            this.AdjustmentValueChanged();
        }
    }
}
