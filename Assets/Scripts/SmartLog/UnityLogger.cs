using UnityEngine;
using System.Runtime.CompilerServices;

namespace SmartLog
{
    public class UnityLogger : ILogger
    {
        public void Log(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.Log($"[INFO][{System.IO.Path.GetFileName(file)}:{line} - {member}] {message}");
        }

        public void LogWarning(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.LogWarning($"[WARN][{System.IO.Path.GetFileName(file)}:{line} - {member}] {message}");
        }

        public void LogError(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.LogError($"[ERROR][{System.IO.Path.GetFileName(file)}:{line} - {member}] {message}");
        }
    }
}