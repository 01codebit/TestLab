using Logging;
using UnityEngine;

namespace TestLab.EventChannel
{
    public class VisibilityChecker : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        
        void OnBecameInvisible()
        {
            ConditionalLogger.Log($"[{gameObject.name}] OnBecameInvisible");
        }

        void OnBecameVisible()
        {
            ConditionalLogger.Log($"[{gameObject.name}] OnBecameVisible");
        }

        private void Update()
        {
            var w2v = camera.WorldToViewportPoint(gameObject.transform.position);
            if (w2v.x < 0 || w2v.x > 1 || w2v.y < 0 || w2v.y > 1)
            {
                ConditionalLogger.Log($"[{gameObject.name}] invisible for selected camera");
                enabled = false;
            }
            else
            {
                enabled = true;
            }
        }
    }
}