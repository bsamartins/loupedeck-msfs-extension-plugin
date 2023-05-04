namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Loupedeck.MsfsExtensionPlugin.Events;

    internal class AltitudeEncoder : PluginDynamicAdjustment, FBWAirbusAircraftChanged
    {
        private Int64 _value = 0;
        private Boolean managed = false;
        public AltitudeEncoder() : base("Alt", "Altitude", "Fly By Wire", true) { }

        protected override String GetAdjustmentValue(String actionParameter) => $"[{this._value}]\n{this._value}";

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            this._value += diff * 100;
            this.AdjustmentValueChanged();
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) {
            var managed = this.managed ? "*" : "";
            return $"{this.DisplayName}{managed}";
        }

        protected override void RunCommand(String actionParameter)
        {
            this.managed = !this.managed;
            this.AdjustmentValueChanged();
        }

        protected override String GetAdjustmentDisplayName(String actionParameter, PluginImageSize imageSize) {
            return "test";
        }
    }
}
