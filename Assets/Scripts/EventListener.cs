using System;
using System.Threading.Tasks;
using Logging;
using UnityEngine;

namespace TestLab.EventChannel
{
    public class EventListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO m_EventChannel;
        [SerializeField] private HandleEventSO m_EventHandler;
        
        private void OnEnable()
        {
            ConditionalLogger.Log("[EventListener.OnEnable] subscribe to VoidEventChannelSO");
            
            m_EventHandler.SetGameObject(gameObject);
            m_EventChannel.OnEventRaised += m_EventHandler.HandleEvent;
            m_EventChannel.OnIntEventRaised += HandleIntEvent;
        }

        private void OnDisable()
        {
            ConditionalLogger.Log("[EventListener.OnDisable] unsubscribe to VoidEventChannelSO");
            m_EventChannel.OnEventRaised -= m_EventHandler.HandleEvent;
            m_EventChannel.OnIntEventRaised -= HandleIntEvent;
        }

        private async void HandleEvent()
        {
            ConditionalLogger.Log("[EventListener.HandleEvent] event received");

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
            ConditionalLogger.Log("DoWorkAsync finished");
        }
        
        private void HandleIntEvent(int arg)
        {
            ConditionalLogger.Log($"[EventListener.HandleEvent] int event received {arg}");
        }
        
        // worker.StartWorking += async (sender, eventArgs) =>
        // {
        //     try
        //     {
        //         await DoWorkAsync();
        //     }
        //     catch (Exception e)
        //     {
        //         //Some form of logging.
        //         Console.WriteLine($"Async task failure: {e.ToString()}");
        //         // Consider gracefully, and quickly exiting.
        //     }
        // };
    }
}