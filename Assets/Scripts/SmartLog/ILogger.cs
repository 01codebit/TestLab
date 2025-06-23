using System.Runtime.CompilerServices;

namespace SmartLog
{
    public interface ILogger
    {
        void Log(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0);

        void LogWarning(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0);

        void LogError(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0);
    }
}