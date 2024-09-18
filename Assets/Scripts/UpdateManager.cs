using System;
using System.Collections.Generic;
using System.Diagnostics;
using Logging;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OptimizationExamples.UpdateManagerExample
{
	public static class UpdateManager
	{
        public static Stopwatch SW { get; private set; } = new Stopwatch();
        public static Action StopWatchStoppedCallback;

        class UpdateManagerInnerMonoBehaviour : MonoBehaviour
        {
            private void Start()
            {
                ConditionalLogger.Log("[UpdateManager.Start]");
            }
            
            void Update()
            {
                SW.Restart();
                // TODO: Update all
                SW.Stop();
                // StopWatchStoppedCallback?.Invoke();
            }
        }

        #region Static constructor and field for inner MonoBehaviour

        static UpdateManager()
        {
            var gameObject = new GameObject();
            gameObject.name = "UpdateManager";
            _innerMonoBehaviour = gameObject.AddComponent<UpdateManagerInnerMonoBehaviour>();

#if UNITY_EDITOR
        gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        _innerMonoBehaviour.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
#endif
            
        }
        static UpdateManagerInnerMonoBehaviour _innerMonoBehaviour;

        #endregion
    }
}