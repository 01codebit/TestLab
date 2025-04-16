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

        private ObjectPool<GameObject> Pool { get; set; }

        private void Start()
        {
            var pc = new GameObjectsFromPrefabPool()
                .WithPrefab(_pooledPrefab)
                .WithTransform(transform)
                .WithDefaultName("TestObject")
                .Build();

            Pool = pc.GetPool();
        }
        
        private void Update()
        {
#if UNITY_EDITOR
    if(test) TestPool();
    test = false;
    
    if(clear) ClearPool();
    clear = false;
#endif
        }

        [SerializeField] private bool test; 
        [SerializeField] private bool clear;

        private List<GameObject> objects = new List<GameObject>(); 
        
        #if UNITY_EDITOR
        private void TestPool()
        {
            // if (used != Pool.CountActive)
            // {
            Debug.Log($"[RackLabelsContainer] active: {Pool.CountActive}, inactive: {Pool.CountInactive}, all: {Pool.CountAll}");
            // }

            for (var i = 0; i < 10; i++)
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
        #endif
    }
}