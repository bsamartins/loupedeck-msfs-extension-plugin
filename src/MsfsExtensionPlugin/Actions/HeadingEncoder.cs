namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;

    using Loupedeck;
    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    internal class HeadingEncoder : AirbusFCUEncoder
    {
        private Int64 _selected = 0;
        private Int64 _indicated = 0;
        private Boolean _track = false;
        public HeadingEncoder() : base("HDG", "Heading", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var setValue = this._selected < 0 ? "" : this._selected.ToString();
            return $"[{setValue}]\n{this._indicated}";
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (diff < 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.HEADING_BUG_DEC);
            }
            else if (diff > 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.HEADING_BUG_INC);
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
            this._selected = e.AutopilotHeadingSelected;
            this._indicated = e.HeadingIndicator;
            this.Managed = e.AutopilotHeadingSlotIndex == 2;
            this._track = e.TrackFpaModeActive;
            this.DisplayName = this._track ? "TRK" : "HDG";
            PluginLog.Info($"Tracking: {this._track}");
            this.AdjustmentValueChanged();
        }
    }
}
