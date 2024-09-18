using UnityEngine;

namespace Logging
{
    public static class ConditionalLogger
    {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void EditorOnlyLog(object message)
        {
            if (Application.isEditor)
            {
                Debug.Log(message);
            }
        }
    }
}