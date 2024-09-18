using System.Net.Http;

namespace TestLab.EventChannel
{
    static class HttpResponseMessageExtensions
    {
        internal static string WriteRequestToConsole(this HttpResponseMessage response)
        {
            if (response is null)
            {
                return null;
            }

            var request = response.RequestMessage;

            var message = $"{request?.Method} " + $"{request?.RequestUri} + $\"HTTP/{{request?.Version}}\n";
            return message;
        }
    }
}