using UnityEngine;
using System.Runtime.CompilerServices;

namespace SmartLog
{
    public class SmartLog : MonoBehaviour
    {
        ILogger logger = new UnityLogger();

        void Start()
        {
            logger.Log("SmartLog initialized successfully.");
            DoSomething();

            ShowFormatted(12345);
            ShowFormatted(-12345);
            ShowFormatted(0);
        }

        private void ShowFormatted(int value)
        {
            string format = "##;(##);**Zero**";
            string result = value.ToString(format);
            logger.Log($"Input: {value} -> Formatted: {result}");
        }

        void DoSomething()
        {
            logger.Log("Doing something in SmartLog.");
            logger.LogWarning("Test Warning in SmartLog.");
            // logger.LogError("Test Error in SmartLog.");
        }
    }
}