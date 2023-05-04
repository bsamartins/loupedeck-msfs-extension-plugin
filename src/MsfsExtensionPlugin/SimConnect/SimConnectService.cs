namespace Loupedeck.MsfsExtensionPlugin.SimConnect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CTrue.FsConnect;

    internal class SimConnectService
    {
        static SimConnectService Instance = new SimConnectService();

        private readonly FsConnect _fsConnect = new FsConnect();        

        SimConnectService() {
            this._fsConnect.Connect("TestApp");
        }


        void Start() {
            
        }
        void Stop() {}
    }
}
