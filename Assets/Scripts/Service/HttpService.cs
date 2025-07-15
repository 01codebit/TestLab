using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Logging;
using TestLab.EventChannel.Model;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;
using UnityEngine.EventSystems;


namespace TestLab.EventChannel
{
    [BurstCompile]
    public static class HttpService //: IJob
    {
        private static bool initialized = false;

        private static HttpRequestHeaders defaultRequestHeaders;
        
        private static void Initialize()
        {
            if (initialized) return;
            
            sharedClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri)
            };
            
            // sharedClient.DefaultRequestHeaders.Add("", "");

            initialized = true;
        }
        
        // public void Execute()
        // {

        // }

        private const string baseUri = "https://jsonplaceholder.typicode.com"; 
        
        // HttpClient lifecycle management best practices:
        // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
        // ...BUT, Unity uses .NET Standard 2.1:
        // https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-net-http-httpclient
        private static HttpClient sharedClient;
        
        public static async Task<string> GetAsync()
        {
            using HttpResponseMessage response = await sharedClient.GetAsync("todos/3");
    
            var request = response.EnsureSuccessStatusCode().WriteRequestToConsole();
    
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var result = $"request: {request}\nresponse: {jsonResponse}"; 
            
            // Expected output:
            //   GET https://jsonplaceholder.typicode.com/todos/3 HTTP/1.1
            //   {
            //     "userId": 1,
            //     "id": 3,
            //     "title": "fugiat veniam minus",
            //     "completed": false
            //   }

            return result;
        }
        
        public static async Task GetFromJsonAsync<T>(string endpoint, Dictionary<string, string> parameters, DataStructure<T> dataStructure=null)
        {
            if(!initialized) Initialize();

            if (sharedClient == null)
            {
                throw new InvalidOperationException("HttpClient is not initialized. Call Initialize() first.");
            }

            if (string.IsNullOrEmpty(endpoint)) return;
            var requestUri = HttpUtils.GetRequestUri(endpoint, parameters);
            
            //var response = await sharedClient.GetAsync("todos?userId=1&completed=false");
            ConditionalLogger.Log($"[HttpService.GetFromJsonAsync] requestUri: {requestUri.ToString()}");
            var response = await sharedClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var result = $"[GET] {baseUri}/{requestUri} [HTTP/1.1] responseBody: {responseBody}";
            ConditionalLogger.Log($"[HttpService.GetFromJsonAsync] {result}");

            if (responseBody.StartsWith('['))
            {
                // la response è un json array
            
                responseBody = HttpUtils.FixJson(responseBody);
                var listFromServer = JsonUtility.FromJson<ListWrapper<T>>(responseBody);
                
                // update model data structure
                if (dataStructure != null)
                {
                    dataStructure.Data = listFromServer.Items;
                }
            }
            else
            {
                // la response è un json object

                var objectFromServer = JsonUtility.FromJson<T>(responseBody);
                
                // update model data structure
                // if (dataStructure != null)
                // {
                //     dataStructure.Data = objectFromServer;
                // }
            }

            
            // Expected output:
            //   GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1
            //   Todo { UserId = 1, Id = 1, Title = delectus aut autem, Completed = False }
            //   Todo { UserId = 1, Id = 2, Title = quis ut nam facilis et officia qui, Completed = False }
            //   Todo { UserId = 1, Id = 3, Title = fugiat veniam minus, Completed = False }
            //   Todo { UserId = 1, Id = 5, Title = laboriosam mollitia et enim quasi adipisci quia provident illum, Completed = False }
            //   Todo { UserId = 1, Id = 6, Title = qui ullam ratione quibusdam voluptatem quia omnis, Completed = False }
            //   Todo { UserId = 1, Id = 7, Title = illo expedita consequatur quia in, Completed = False }
            //   Todo { UserId = 1, Id = 9, Title = molestiae perspiciatis ipsa, Completed = False }
            //   Todo { UserId = 1, Id = 13, Title = et doloremque nulla, Completed = False }
            //   Todo { UserId = 1, Id = 18, Title = dolorum est consequatur ea mollitia in culpa, Completed = False }
        }

        public static async Task<List<T>> GetFromJsonAsync<T>(string endpoint, Dictionary<string, string> parameters)
        {
            if(!initialized) Initialize();
            
            if (sharedClient == null)
            {
                throw new InvalidOperationException("HttpClient is not initialized. Call Initialize() first.");
            }

            if (string.IsNullOrEmpty(endpoint))
            {
                throw new InvalidOperationException("endpoint can't be null or empty.");
            }

            var data = new List<T>();

            var requestUri = HttpUtils.GetRequestUri(endpoint, parameters);
            
            //var response = await sharedClient.GetAsync("todos?userId=1&completed=false");
            ConditionalLogger.Log($"[HttpService.GetFromJsonAsync] requestUri: {requestUri.ToString()}");
            var response = await sharedClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var result = $"[GET] {baseUri}/{requestUri} [HTTP/1.1] responseBody: {responseBody}";
            ConditionalLogger.Log($"[HttpService.GetFromJsonAsync] {result}");
            
            if (responseBody.StartsWith('['))
            {
                // la response è un json array
                responseBody = HttpUtils.FixJson(responseBody);
                var listFromServer = JsonUtility.FromJson<ListWrapper<T>>(responseBody);
                data.AddRange(listFromServer.Items);
            }
            else
            {
                // la response è un singolo json object
                var objectFromServer = JsonUtility.FromJson<T>(responseBody);
                data.Add(objectFromServer);
            }

            return data;
        }
    }
}