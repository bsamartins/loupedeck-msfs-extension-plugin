namespace Loupedeck.MsfsExtensionPlugin.SimConnect
{
    using System;
    using System.Collections.Generic;
    using System.Timers;

    using CTrue.FsConnect;
    using CTrue.FsConnect.Managers;

    using Loupedeck.MsfsExtensionPlugin.Events;

    using Microsoft.FlightSimulator.SimConnect;
   

    internal class SimConnectService
    {
        private static readonly Lazy<SimConnectService> _instance = new Lazy<SimConnectService>(() => {
            var service = new SimConnectService();
            service.Start();
            return service;
        });
        internal static SimConnectService Instance {
            get {
                return _instance.Value;
            }
        }

        private readonly FsConnect _fsConnect = new FsConnect();
        private readonly List<FBWAirbusAircraftChanged> _fbwAirbusAircaftChangeListeners = new List<FBWAirbusAircraftChanged>();
        private readonly AircraftManager<AirbusPlaneInfoResponse> _aircraftManager;
        private readonly Int32 _requestId;
        private readonly Int32 _eventGroupId;
        private readonly Timer _timer = new Timer(500);
        private readonly Dictionary<SendEvent, Int32> _sendEventIds = new Dictionary<SendEvent, Int32>();

        SimConnectService() {
            this._fsConnect.Connect("TestApp");
            this._fsConnect.FsError += HandleError;

            this._requestId = this._fsConnect.GetNextId();
            this._eventGroupId = this._fsConnect.GetNextId();
            var planeInfoDefinitionId = this._fsConnect.RegisterDataDefinition<AirbusPlaneInfoResponse>();
            this._aircraftManager = new AircraftManager<AirbusPlaneInfoResponse>(this._fsConnect, planeInfoDefinitionId, this._requestId);
            this._aircraftManager.Updated += this.ChangeHandler;
            this._aircraftManager.RequestMethod = RequestMethod.Continuously;
            this._timer.Elapsed += this.OnTick;

            foreach (SendEvent e in Enum.GetValues(typeof(SendEvent)))
            {
                var eventId = this._fsConnect.GetNextId();
                this._sendEventIds.Add(e, eventId);
                this._fsConnect.MapClientEventToSimEvent(this._eventGroupId, eventId, getVarName(e));
            }
        }

        private void HandleError(Object sender, FsErrorEventArgs e) => PluginLog.Error($"SimConnect error: {e.ExceptionCode} - [{e.Index}] {e.ExceptionDescription}");

        private void OnTick(Object sender, ElapsedEventArgs e) {
            PluginLog.Info($"Fetching values");
            this._aircraftManager.Get(); 
        }

        internal void RegisterChangeHandler(FBWAirbusAircraftChanged handler) {
            PluginLog.Info($"Registering event handler {handler}");
            this._fbwAirbusAircaftChangeListeners.Add(handler);
            _fsConnect.RegisterDataDefinition<AirbusPlaneInfoResponse>();
        }
        internal void UnregisterChangeHandler(FBWAirbusAircraftChanged handler) {
            PluginLog.Info($"Registering event handler {handler}");
            this._fbwAirbusAircaftChangeListeners.Remove(handler); 
        }

        internal void Start() {
            PluginLog.Info("Starting service");
            this._timer.Start();

        }

        internal void Stop() {            
            PluginLog.Info("Stopping service");
            this._timer.Stop();
            this._timer.Start();
        }

        internal void SendCommand(SendEvent e, Double value)
        {
            PluginLog.Info("Sending event");
            this._fsConnect.TransmitClientEvent(e, (UInt32) value, EventGroup.UNKNOWN);
        }

        private void ChangeHandler(Object sender, AircraftInfoUpdatedEventArgs<AirbusPlaneInfoResponse> e) {
            PluginLog.Info($"Received event {e}");            
            this._fbwAirbusAircaftChangeListeners.ForEach(h => h.OnAircraftChanged(e.AircraftInfo)); 
        }

        static String getVarName(SendEvent v) {
            switch (v)
            {
                case SendEvent.A32NX_FCU_SPD_SET: return "L:A32NX.FCU_SPD_SET";
                case SendEvent.AP_SPD_VAR_DEC: return "AP_SPD_VAR_DEC";
                case SendEvent.AP_SPD_VAR_INC: return "AP_SPD_VAR_INC";
                case SendEvent.AP_SPD_VAR_SET: return "AP_SPD_VAR_SET";
                default: throw new NotImplementedException($"Missing var name for {v}");
            }
        }
    }

    internal enum SendEvent {
        A32NX_FCU_SPD_SET,
        AP_SPD_VAR_DEC,
        AP_SPD_VAR_INC,
        AP_SPD_VAR_SET
    }

    internal enum EventGroup { 
        UNKNOWN
    }
}
