using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace ObjectPool
{
    public class PooledObjectsManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pooledPrefab;
        [SerializeField] private int _maxSize = 20;

        private ObjectPool<GameObject> Pool { get; set; }

        private void Start()
        {
            var pc = new GameObjectsFromPrefabPool(_pooledPrefab, transform, "TestObject", _maxSize);

            Pool = pc.GetPool();
        }
        
        private void Update()
        {
#if UNITY_EDITOR
    if(test) TestPool();
    test = false;
    
    if(clear) ClearPool();
    clear = false;

    if(reset) Reset();
    reset = false;
#endif
        }

        [SerializeField] private bool test; 
        [SerializeField] private bool clear;
        [SerializeField] private bool reset;

        private List<GameObject> objects = new List<GameObject>(); 
        
        #if UNITY_EDITOR
        private void TestPool()
        {
            // if (used != Pool.CountActive)
            // {
            Debug.Log($"[RackLabelsContainer] active: {Pool.CountActive}, inactive: {Pool.CountInactive}, all: {Pool.CountAll}");
            // }

            var count = Random.Range(5, 12);
            
            for (var i = 0; i < count; i++)
            {
                var x = Pool.Get();
                x.name = $"TestObject #{Pool.CountActive}";
                x.transform.position += Random.insideUnitSphere * 5;
                
                objects.Add(x);
            }
        }
        #endif
        
#if UNITY_EDITOR
        private void ClearPool()
        {
            foreach (var go in objects)
            {
                Pool.Release(go);
            }

            objects.Clear();
        }

        private void Reset()
        {
            ClearPool();
            objects.Clear();
            Pool.Clear();
        }
#endif
    }
}