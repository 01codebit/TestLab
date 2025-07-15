using Channels;
using Logging;
using UnityEngine;
using UnityEngine.InputSystem; // 1. The Input System "using" statement

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
        
        // 2. These variables are to hold the Action references
        InputAction enterAction;
        InputAction numpad0Action;
        InputAction numpad1Action;

        void Start()
        {
            enterAction = InputSystem.actions.FindAction("Test_Enter");
            numpad0Action = InputSystem.actions.FindAction("Test_Numpad_0");
            numpad1Action = InputSystem.actions.FindAction("Test_Numpad_1");
        }


        public void Update()
        {
            if (enterAction.IsPressed())
            {
                TriggerVoidEvent();
            }

            if (numpad0Action.IsPressed())
            {
                TriggerIntEvent(0);
            }

            if (numpad1Action.IsPressed())
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