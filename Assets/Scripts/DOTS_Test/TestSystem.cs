using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace DOTSTest
{
    [RequireMatchingQueriesForUpdate]
    [BurstCompile]
    public partial struct TestSystem : ISystem
    {
        private EntityQuery _ballQuery;

        public void OnCreate(ref SystemState state)
        {
            Debug.Log("[TestSystem.OnCreate]");
            _ballQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Ball, LocalTransform>().Build(ref state);
        }

        public void OnUpdate(ref SystemState state)
        {
            Debug.Log("[TestSystem.OnUpdate]");
        }
    }

    [BurstCompile]
    struct TestJob : IJobParallelFor
    {
        public void Execute(int i)
        {
            Debug.Log("[TestJob.Execute]");
        }
    }
}
