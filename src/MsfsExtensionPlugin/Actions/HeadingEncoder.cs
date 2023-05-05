namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;

    using Loupedeck.MsfsExtensionPlugin.Events;
    using Loupedeck.MsfsExtensionPlugin.SimConnect;
    using Loupedeck.MsfsExtensionPlugin.Helpers;

    internal class HeadingEncoder : AirbusFCUEncoder
    {
        private Int64 _selected = 0;
        private Int64 _indicated = 0;
        public HeadingEncoder() : base("Head", "Heading", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var setValue = this._selected < 0 ? "" : this._selected.ToString();
            return $"[{setValue}]\n{this._indicated}";
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            var newValue = ConvertTool.ApplyAdjustment(this._selected, diff, 0, 360, 1);
            this._selected = newValue;            
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
            PluginLog.Info($"Selected airspeed {e.AutopilotHeadingSelected}");
            this._selected = e.AutopilotHeadingSelected;
            this._indicated = e.HeadingIndicator;
            this.Managed = e.AutopilotHeadingSlotIndex == 2;
            this.AdjustmentValueChanged();
        }
    }
}
