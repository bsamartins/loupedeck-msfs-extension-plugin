namespace Loupedeck.MsfsExtensionPlugin
{
    using System;

    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    // This class contains the plugin-level logic of the Loupedeck plugin.

    public class MsfsExtensionPlugin : Plugin
    {
        // Gets a value indicating whether this is an Universal plugin or an Application plugin.
        public override Boolean UsesApplicationApiOnly => true;

        // Gets a value indicating whether this is an API-only plugin.
        public override Boolean HasNoApplication => true;

        public MsfsExtensionPlugin() => PluginLog.Init(this.Log);

        // This method is called when the plugin is loaded during the Loupedeck service start-up.
        public override void Load()
        {
        }

        // This method is called when the plugin is unloaded during the Loupedeck service shutdown.
        public override void Unload()
        {
            SimConnectService.Instance.Stop();
        }
    }
}
