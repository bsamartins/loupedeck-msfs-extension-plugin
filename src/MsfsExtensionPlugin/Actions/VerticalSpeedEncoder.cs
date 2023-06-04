namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;

    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    internal class VerticalSpeedEncoder : AirbusFCUEncoder
    {
        private Int32 _selectedVs = 0;
        private Double _selectedFpa = 0;
        private Int64 _indicated = 0;
        private Boolean _track = false;
        public VerticalSpeedEncoder() : base("V/S", "Vertical Speed", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var setValue = "";
            if (this._track)
            {
                setValue = this._selectedFpa.ToString("0.0");
            }
            else
            { 
                setValue = this._selectedVs.ToString();
            }
            return $"[{setValue}]\n{this._indicated}";
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (diff < 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.AP_VS_VAR_DEC);
            }
            else if (diff > 0)
            {
                SimConnectService.Instance.SendCommand(SendEvent.AP_VS_VAR_INC);
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
            this._track = e.TrackFpaModeActive;
            this._selectedVs = e.AutopilotVsSelected;
            this._selectedFpa = e.AutopilotFpaSelected;
            this.Managed = e.FcVsManaged;
            //            this._indicated = e.AutopilotVsSelected;
            //          this.Managed = e.AutopilotHeadingSlotIndex == 2;            
            this.DisplayName = this._track ? "FPA" : "V/S";
            PluginLog.Info($"Tracking: {this._track}");
            this.AdjustmentValueChanged();
        }
    }
}
