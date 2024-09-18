using Logging;
using UnityEngine;

namespace TestLab.EventChannel
{
    public class ScrollViewItemVisibilityChecker : MonoBehaviour
    {
        void OnBecameInvisible()
        {
            ConditionalLogger.Log($"[ScrollViewItemVisibilityChecker.OnBecameInvisible] gameObject: {gameObject.name}");
        }

        void OnBecameVisible()
        {
            ConditionalLogger.Log($"[ScrollViewItemVisibilityChecker.OnBecameVisible] gameObject: {gameObject.name}");
        }

        private void OnEnable()
        {
            ConditionalLogger.Log($"[ScrollViewItemVisibilityChecker.OnEnable] gameObject: {gameObject.name}");
        }

        private void OnDisable()
        {
            ConditionalLogger.Log($"[ScrollViewItemVisibilityChecker.OnDisable] gameObject: {gameObject.name}");
        }
    }
}