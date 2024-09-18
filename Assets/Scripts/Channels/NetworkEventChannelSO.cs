using UnityEngine;
using UnityEngine.Events;

namespace TestLab.EventChannel
{
    [CreateAssetMenu(menuName = "Events/Network Event Channel", fileName = "NetworkEventChannelSO")]
    public class NetworkEventChannelSO : ScriptableObject
    {
        [Tooltip("The action to perform")] public UnityAction OnEventRaised;
        [Tooltip("The action to perform")] public UnityAction OnFinishEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }

        public void RaiseFinishEvent()
        {
            OnFinishEventRaised?.Invoke();
        }
    }
}