using System;
using System.Threading.Tasks;
using Channels;
using Logging;
using UnityEngine;

namespace TestLab.EventChannel
{
    public class NetworkEventListener : MonoBehaviour
    {
        [SerializeField] private NetworkEventChannelSO m_NetworkEventChannel;
        
        private void OnEnable()
        {
            ConditionalLogger.Log("[NetworkEventListener.OnEnable] subscribe to NetworkEventChannelSO");
            m_NetworkEventChannel.OnFinishEventRaised += HandleFinishedEvent;
        }

        private void OnDisable()
        {
            ConditionalLogger.Log("[NetworkEventListener.OnEnable] unsubscribe to NetworkEventChannelSO");
            m_NetworkEventChannel.OnFinishEventRaised -= HandleFinishedEvent;
        }

        private async void HandleFinishedEvent()
        {
            ConditionalLogger.Log("[NetworkEventListener.HandleEvent] event received");

            // async call:
            {
                try
                {
                    await DoWorkAsync();
                }
                catch (Exception e)
                {
                    //Some form of logging.
                    Console.WriteLine($"Async task failure: {e.ToString()}");
                    // Consider gracefully, and quickly exiting.
                }
            };
        }

        private async Task DoWorkAsync()
        {
            await Task.Delay(3000);
            ConditionalLogger.Log("[NetworkEventListener.DoWorkAsync] finished");
        }
    }
}