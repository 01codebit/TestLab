using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Channels
{
    [CreateAssetMenu(menuName = "Events/Network Event Channel", fileName = "NetworkEventChannelSO")]
    public class NetworkEventChannelSO : ScriptableObject
    {
        [Tooltip("The action to perform")] public UnityAction OnEventRaised;
        [Tooltip("The action to perform")] public UnityAction OnFinishEventRaised;

        [CanBeNull] public event Func<Task> OnEventRaisedAsync;

        
        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
            OnEventRaisedAsync?.Invoke();
        }

        public void RaiseFinishEvent()
        {
            OnFinishEventRaised?.Invoke();
        }
    }
}