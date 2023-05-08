﻿namespace Loupedeck.MsfsExtensionPlugin.SimConnect
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Timers;

    using CTrue.FsConnect;
    using CTrue.FsConnect.Managers;

    using Loupedeck.MsfsExtensionPlugin.Events;

    using Microsoft.FlightSimulator.SimConnect;
   

    internal class SimConnectService
    {
        private static Boolean _initialized = false;
        private static readonly Lazy<SimConnectService> _instance = new Lazy<SimConnectService>(() => {
            var service = new SimConnectService();
            service.Start();
            return service;
        });
        internal static SimConnectService Instance => _instance.Value;

        private readonly FsConnect _fsConnect = new FsConnect();
        private readonly List<FBWAirbusAircraftChanged> _fbwAirbusAircaftChangeListeners = new List<FBWAirbusAircraftChanged>();
        private readonly AircraftManager<AirbusPlaneInfoResponse> _aircraftManager;
        private readonly Int32 _requestId;
        private readonly Timer _timer = new Timer(500);

        SimConnectService() {
            this._fsConnect.Connect("TestApp");
            this._fsConnect.FsError += this.HandleError;

            this._requestId = this._fsConnect.GetNextId();
            var planeInfoDefinitionId = this._fsConnect.RegisterDataDefinition<AirbusPlaneInfoResponse>();
            this._aircraftManager = new AircraftManager<AirbusPlaneInfoResponse>(this._fsConnect, planeInfoDefinitionId, this._requestId);
            this._aircraftManager.Updated += this.ChangeHandler;
            this._aircraftManager.RequestMethod = RequestMethod.Continuously;
            this._timer.Elapsed += this.OnTick;
            
            foreach (SendEvent e in Enum.GetValues(typeof(SendEvent)))
            {
                var varName = getVarName(e);
                PluginLog.Info($"Registering event: {e} -> {varName}");
                this._fsConnect.MapClientEventToSimEvent(EventGroup.UNKNOWN, e, varName);
            }
        }

        private void HandleError(Object sender, FsErrorEventArgs e) => PluginLog.Error($"SimConnect error: [{e.SendID}] {e.ExceptionCode} - [{e.Index}] {e.ExceptionDescription}");

        private void OnTick(Object sender, ElapsedEventArgs e) {
            this._aircraftManager.Get(); 
        }

        internal void RegisterChangeHandler(FBWAirbusAircraftChanged handler) {
            PluginLog.Info($"Registering event handler {handler}");
            this._fbwAirbusAircaftChangeListeners.Add(handler);
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

        internal void SendCommand(SendEvent e, uint data = 0)
        {
            var varName = getVarName(e);
            PluginLog.Info($"Sending event {e}[{varName}]={data}");
            this._fsConnect.TransmitClientEvent(e, data, EventGroup.UNKNOWN);
            //this._fsConnect.GetSimConnect().TransmitClientEvent(0, e, data, EventGroup.UNKNOWN, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
        }

        private void ChangeHandler(Object sender, AircraftInfoUpdatedEventArgs<AirbusPlaneInfoResponse> e) {
            this._fbwAirbusAircaftChangeListeners.ForEach(h => h.OnAircraftChanged(e.AircraftInfo)); 
        }

        static String getVarName(SendEvent v) {
            switch (v)
            {
                case SendEvent.A32NX_FCU_SPD_SET: return "L:A32NX.FCU_SPD_SET";
                case SendEvent.AP_SPD_DEC: return "AP_SPD_VAR_DEC";
                case SendEvent.AP_SPD_INC: return "AP_SPD_VAR_INC";
                case SendEvent.AP_SPD_SET: return "AP_SPD_VAR_SET";
                case SendEvent.AP_ALT_SET: return "AP_ALT_VAR_SET_ENGLISH";
                case SendEvent.AP_ALT_DEC: return "AP_ALT_VAR_DEC";
                case SendEvent.AP_ALT_INC: return "AP_ALT_VAR_INC";
                case SendEvent.FBW_AP_ALT_DEC: return "A32NX.FCU_ALT_DEC";
                case SendEvent.FBW_AP_ALT_INC: return "A32NX.FCU_ALT_INC";
                default: throw new NotImplementedException($"Missing var name for {v}");
            }
        }
    }

    static class FsConnectExtension
    {
        public static SimConnect GetSimConnect(this FsConnect fsConnect)
        {
            var field= typeof(FsConnect).GetField("_simConnect", BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(fsConnect) as SimConnect;
        }
    }

    internal enum SendEvent {
        A32NX_FCU_SPD_SET,
        AP_SPD_DEC,
        AP_SPD_INC,
        AP_SPD_SET,
        AP_ALT_SET,
        AP_ALT_DEC,
        AP_ALT_INC,
        FBW_AP_ALT_DEC,
        FBW_AP_ALT_INC,
    }

    internal enum EventGroup { 
        UNKNOWN
    }
}
