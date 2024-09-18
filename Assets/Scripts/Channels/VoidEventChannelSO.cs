using UnityEngine;
using UnityEngine.Events;

namespace TestLab.EventChannel
{
    [CreateAssetMenu(menuName = "Events/Void Event Channel", fileName = "VoidEventChannel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        [Tooltip("The action to perform")] public UnityAction OnEventRaised;
        [Tooltip("The action to perform")] public UnityAction<int> OnIntEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }

        public void RaiseIntEvent(int arg)
        {
            OnIntEventRaised?.Invoke(arg);
        }

        public void TestVoidEvent()
        {
            RaiseEvent();
        }
    }
}