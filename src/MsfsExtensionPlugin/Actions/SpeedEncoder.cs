namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;

    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    internal class SpeedEncoder : AirbusFCUEncoder
    {
        private Int64 _selectedSpeed = 0;
        private Int64 _indicatedSpeed = 0;
        public SpeedEncoder() : base("SPD", "Speed", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var setSpeed = this._selectedSpeed < 0 ? "" : this._selectedSpeed.ToString();
            return $"[{setSpeed}]\n{this._indicatedSpeed}";
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (diff < 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.AP_SPD_DEC);
            } 
            else if (diff > 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.AP_SPD_INC);
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
            this._selectedSpeed = e.AutopilotSpeedSelected;
            this._indicatedSpeed = e.AirspeedIndicated;
            this.Managed = e.AutopilotSpeedSlotIndex == 2;
            this.AdjustmentValueChanged();
        }
    }
}
