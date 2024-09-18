using System.Collections.Generic;
using System.Diagnostics;
using Logging;
using TestLab.EventChannel.Model;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TestLab.EventChannel
{
    public class HttpServiceTester : MonoBehaviour
    {
        [SerializeField] private NetworkEventChannelSO m_NetworkEventChannel;

        private void OnEnable()
        {
            ConditionalLogger.Log("[EventListener.OnEnable] subscribe to VoidEventChannelSO");
            
            m_NetworkEventChannel.OnEventRaised += TestGetAsync;
        }

        private void OnDisable()
        {
            ConditionalLogger.Log("[EventListener.OnDisable] unsubscribe to VoidEventChannelSO");
            m_NetworkEventChannel.OnEventRaised -= TestGetAsync;
        }
        
        private static string _endpoint = "todos";
        private static Dictionary<string, string> _parameters = new Dictionary<string, string>()
        {
            {"userId","1"}, 
            {"completed","false"}
        };

        private async void TestGetAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
            ConditionalLogger.Log("[HttpServiceTester.Start] start");
            await HttpService.GetFromJsonAsync<Todo>(_endpoint, _parameters);
            sw.Stop();
            ConditionalLogger.Log($"[HttpServiceTester.Start] end ({sw.ElapsedMilliseconds}ms)");
            m_NetworkEventChannel.OnFinishEventRaised();
        }
    }
}