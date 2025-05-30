using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestLab.EventChannel;
using TestLab.EventChannel.Model;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class Anagrafica
    {
        public string _id { get; set; }
        public string mmsi { get; set; }
        public string imo_number { get; set; }
        public string type { get; set; }
        public string flag { get; set; }
        public string rtrim { get; set; }
        public string name { get; set; }
        
        public override string ToString()
        {
            return $"Anagrafica [_id: {_id}, mmsi: {mmsi}, imo_number: {imo_number}, type: {type}, flag: {flag}, rtrim: {rtrim}, name: {name}]";
        }
    }
    
    [Serializable]
    public class ResponseListWrapper<T>
    {
        public List<T> response = new List<T>();
    }

    public class TestAnagrafica : MonoBehaviour
    {
        private static HttpClient? _sharedClient;

        private async Task Start()
        {
            Debug.Log("[TestAnagrafica.Start]");
            
            InitClient();
            await GetAnagrafica();
        }

        private static void InitClient()
        {
            // var handler = new SocketsHttpHandler
            // {
            //     PooledConnectionLifetime = TimeSpan.FromMinutes(15) // Recreate every 15 minutes
            // };

            // Create a new instance of the HttpClient class
            // _sharedClient = new HttpClient(handler);
            _sharedClient = new HttpClient();

            // Set the base address for the HttpClient
            _sharedClient.BaseAddress = new Uri("http://api2.marittimi.dev.white3.it");

            // Set a timeout for requests
            _sharedClient.Timeout = TimeSpan.FromSeconds(30);

            // Add default headers
            _sharedClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
        }

        private async Task GetAnagrafica()
        {
            if (_sharedClient == null)
            {
                Console.WriteLine("HttpClient is not initialized.");
                InitClient();
            }

            // Send a GET request to the specified URI
            var response = await _sharedClient.GetAsync("/v1/timmarittimi/anagrafica");

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                // take only the substring until the char '}'
                // int endIndex = content.IndexOf('}');
                // if (endIndex != -1)
                // {
                //     content = content.Substring(0, endIndex + 1);
                // }

                Debug.Log($"[TestAnagrafica.GetAnagrafica] Response content: {content}");

                
                content = HttpUtils.FixJson(content);
                var listFromServer = JsonUtility.FromJson<ResponseListWrapper<Anagrafica>>(content);
                
                for (var i=0; i<listFromServer.response.Count; ++i)
                {
                    var x = listFromServer.response[i];
                    Debug.Log($"[TestAnagrafica.GetAnagrafica] [{i+1}/{listFromServer.response.Count}]: {x.ToString()}");
                }
            }
        }
    }
}