using Logging;
using UnityEngine;

namespace TestLab.EventChannel
{
    public class EventRaiser : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO m_EventChannel;
        [SerializeField] private NetworkEventChannelSO m_NetworkEventChannel;

        public bool TestRaiseEvent = false;
        public bool TestRaiseIntEvent = false;
        public bool TestRaiseNetworkEvent = false;
        public int TestRaiseIntEventArg = 0;
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TriggerVoidEvent();
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                TriggerIntEvent(0);
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                TriggerIntEvent(1);
            }

            if (TestRaiseEvent)
            {
                TestRaiseEvent = false;
                TriggerVoidEvent();
            }

            if (TestRaiseIntEvent)
            {
                TestRaiseIntEvent = false;
                TriggerIntEvent(TestRaiseIntEventArg);
            }

            if (TestRaiseNetworkEvent)
            {
                TestRaiseNetworkEvent = false;
                TriggerNetworkEvent();
            }
        }

        private void TriggerVoidEvent()
        {
            ConditionalLogger.Log("[EventRaiser.TriggerEvent] raise event");
            m_EventChannel?.RaiseEvent();
        }

        private void TriggerIntEvent(int arg)
        {
            ConditionalLogger.Log("[EventRaiser.TriggerEvent] raise int event");
            m_EventChannel?.RaiseIntEvent(arg);
        }

        private void TriggerNetworkEvent()
        {
            ConditionalLogger.Log("[EventRaiser.TriggerNetworkEvent] raise network event");
            m_NetworkEventChannel?.RaiseEvent();
        }
    }
}