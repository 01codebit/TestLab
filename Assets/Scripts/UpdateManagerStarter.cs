using Logging;
using OptimizationExamples.UpdateManagerExample;
using UnityEngine;

public class UpdateManagerStarter : MonoBehaviour
{
    void OnEnable()
    {
        UpdateManager.StopWatchStoppedCallback += StopWatchCallback;
    }

    private void OnDisable()
    {
        UpdateManager.StopWatchStoppedCallback -= StopWatchCallback;
    }
    
    void StopWatchCallback()
    {
        ConditionalLogger.Log("[UpdateManagerStarter.StopWatchCallback]");
    }
}
