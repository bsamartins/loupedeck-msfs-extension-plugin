namespace Loupedeck.MsfsExtensionPlugin.Events
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Loupedeck.MsfsExtensionPlugin.SimConnect;

    internal interface FBWAirbusAircraftChanged
    {
        void OnAircraftChanged(AirbusPlaneInfoResponse e);
    }
}
