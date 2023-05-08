namespace Loupedeck.MsfsExtensionPlugin.Actions
{
    using System;
    using Loupedeck.MsfsExtensionPlugin.Events;
    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    internal abstract class AirbusFCUEncoder : PluginDynamicAdjustment, FBWAirbusAircraftChanged
    {
        public Boolean Managed = false;
        protected AirbusFCUEncoder(String displayName, String description, String groupName, Boolean hasReset) : base(displayName, description, groupName, hasReset)
        {
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            var managed = this.Managed ? "⬤" : "";
            return $"{this.DisplayName}{managed}";
        }

        abstract public void OnAircraftChanged(AirbusPlaneInfoResponse e);
    }
}
