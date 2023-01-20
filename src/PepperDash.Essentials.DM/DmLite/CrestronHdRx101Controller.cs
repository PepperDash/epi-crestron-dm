using System;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.DM.Endpoints.Receivers;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;

namespace PepperDash.Essentials.DM.DmLite
{
    public class CrestronHdRx101Controller : EssentialsBridgeableDevice,
                                             ICommunicationMonitor,
                                             IOnline,
                                             IRoutingInputsOutputs

    {
        private HdRx101CEDmLite _rx;
        private CrestronGenericBaseCommunicationMonitor _monitor;

        public CrestronHdRx101Controller(string key, string name, Func<HdRx101CEDmLite> builder) : base(key, name)
        {
            InputPorts = new RoutingPortCollection<RoutingInputPort>
                         {
                             new RoutingInputPort("DmIn",
                                                  eRoutingSignalType.AudioVideo,
                                                  eRoutingPortConnectionType.DmCat,
                                                  new object(),
                                                  this)
                         };

            OutputPorts = new RoutingPortCollection<RoutingOutputPort>
                          {
                              new RoutingOutputPort("HdmiOut",
                                                    eRoutingSignalType.AudioVideo,
                                                    eRoutingPortConnectionType.Hdmi,
                                                    new object(),
                                                    this)
                          };

            AddPreActivationAction(() => _rx = builder());
            AddPreActivationAction(() => _monitor = new CrestronGenericBaseCommunicationMonitor(this, _rx, 30000, 60000));
        }

        public override void Initialize()
        {
            if (_rx.Registerable)
            {
                _rx.RegisterWithLogging(Key);
            }
        }

        public override void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
           
            
        }

        public StatusMonitorBase CommunicationMonitor
        {
            get { return _monitor; }
        }

        public BoolFeedback IsOnline
        {
            get { return _monitor.IsOnlineFeedback; }
        }

        public RoutingPortCollection<RoutingInputPort> InputPorts { get; private set; }
        public RoutingPortCollection<RoutingOutputPort> OutputPorts { get; private set; }
    }
}