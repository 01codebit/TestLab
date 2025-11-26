using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTSTest
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    [BurstCompile]
    public partial struct TestSystem : ISystem
    {
        private EntityQuery _ballQuery;

        public void OnUpdate(ref SystemState state)
        {
            var ballsQuery = SystemAPI.QueryBuilder().WithAll<Ball>().WithAllRW<LocalToWorld>().Build();
            
            var world = state.WorldUnmanaged;
            state.EntityManager.GetAllUniqueSharedComponents(out NativeList<Ball> uniqueBallsTypes, world.UpdateAllocator.ToAllocator);
            var dt = math.min(0.05f, SystemAPI.Time.DeltaTime);

            foreach (var ballSettings in uniqueBallsTypes)
            {
                ballsQuery.AddSharedComponentFilter(ballSettings);

                var ballsCount = ballsQuery.CalculateEntityCount();
                if (ballsCount == 0)
                {
                    // Early out. If the given variant includes no Boids, move on to the next loop.
                    // For example, variant 0 will always exit early bc it's it represents a default, uninitialized
                    // Boid struct, which does not appear in this sample.
                    ballsQuery.ResetFilter();
                    continue;
                }

                var move = ballSettings.MoveDelta * math.sin(ballSettings.MoveSpeed * (float)SystemAPI.Time.ElapsedTime);
                
                var steerBallJob = new SteerBallJob
                {
                    MoveDistance = move,
                };
                var steerBallJobHandle = steerBallJob.Schedule(ballsQuery, state.Dependency);
                state.Dependency = steerBallJobHandle;
                
                ballsQuery.AddDependency(state.Dependency);
                ballsQuery.ResetFilter();
            }
        }
    }

    [BurstCompile]
    partial struct SteerBallJob : IJobEntity
    {
        public float MoveDistance;

        private void Execute([EntityIndexInQuery] int entityIndexInQuery, ref LocalToWorld localToWorld)
        {
            var currentPosition = localToWorld.Position;
            var currentRotation = localToWorld.Rotation;

            localToWorld = new LocalToWorld
            {
                Value = float4x4.TRS(
                    // TODO: precalc speed*dt
                    new float3(currentPosition.x, currentPosition.y + MoveDistance, currentPosition.z),
                    currentRotation,
                    new float3(1.0f, 1.0f, 1.0f))
            };
        }
    }
}
