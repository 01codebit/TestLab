using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

namespace Networking
{
    public class NetworkTools : MonoBehaviour
    {
        private void Awake()
        {
            ListNetworkInterfaces();
        }
        
        private static void ListNetworkInterfaces()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in networkInterfaces)
            {
                Debug.Log($"Name: {ni.Name}");
                Debug.Log($"Status: {ni.OperationalStatus}");
                Debug.Log($"Speed: {ni.Speed}");
                Debug.Log($"Type: {ni.NetworkInterfaceType}");
                Debug.Log($"Description: {ni.Description}");
                Debug.Log($"Supports Multicast: {ni.SupportsMulticast}");
                Debug.Log($"Supports IPv4: {ni.Supports(NetworkInterfaceComponent.IPv4)}");
                Debug.Log($"Supports IPv6: {ni.Supports(NetworkInterfaceComponent.IPv6)}");
            }
        }

        private static void TestNetworkEvents()
        {
            NetworkChange.NetworkAvailabilityChanged += OnNetworkAvailabilityChanged;
            
            static void OnNetworkAvailabilityChanged(
                object? sender, NetworkAvailabilityEventArgs networkAvailability) =>
                Debug.Log($"Network is available: {networkAvailability.IsAvailable}");
            
            Debug.Log("Listening changes in network availability. Press any key to continue.\n");
            
            NetworkChange.NetworkAvailabilityChanged -= OnNetworkAvailabilityChanged;
        }
        
        private static async Task PingHost(string hostName)
        {
            using Ping ping = new();

            PingReply reply = await ping.SendPingAsync(hostName);
            Debug.Log($"Ping status for ({hostName}): {reply.Status}");
            if (reply is { Status: IPStatus.Success })
            {
                Debug.Log($"Address: {reply.Address}");
                Debug.Log($"Roundtrip time: {reply.RoundtripTime}");
                Debug.Log($"Time to live: {reply.Options?.Ttl}");
            }
        }
    }
}