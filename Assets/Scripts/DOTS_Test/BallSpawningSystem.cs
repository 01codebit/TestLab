using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DOTSTest
{
    [RequireMatchingQueriesForUpdate]
    [BurstCompile]
    public partial struct BallSpawningSystem : ISystem
    {
        private EntityQuery _ballQuery;
        public void OnCreate(ref SystemState state)
        {
            Debug.Log("[XXX][BallSpawningSystem.OnCreate]");
            _ballQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Ball, LocalTransform>().Build(ref state);
        }

        public void OnUpdate(ref SystemState state)
        {
            Debug.Log("[XXX][BallSpawningSystem.OnUpdate]");
            var localToWorldLookup = SystemAPI.GetComponentLookup<LocalToWorld>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var world = state.World.Unmanaged;
            
            foreach (var (ballGroup, ballGroupLocalToWorld, entity) in
                     SystemAPI.Query<RefRO<BallGroup>, RefRO<LocalToWorld>>()
                         .WithEntityAccess())
            {
                var totalBalls = ballGroup.ValueRO.Columns * ballGroup.ValueRO.Rows;

                var ballEntities =
                    CollectionHelper.CreateNativeArray<Entity, RewindableAllocator>(totalBalls, ref world.UpdateAllocator);

                state.EntityManager.Instantiate(ballGroup.ValueRO.Prefab, ballEntities);

                var setBallLocalToWorldJob = new SetBallLocalToWorld
                {
                    LocalToWorldFromEntity = localToWorldLookup,
                    Entities = ballEntities,
                    Center = ballGroupLocalToWorld.ValueRO.Position,
                    Columns = ballGroup.ValueRO.Columns,
                    Rows = ballGroup.ValueRO.Rows
                };

                state.Dependency = setBallLocalToWorldJob.Schedule(totalBalls, 64, state.Dependency);
                state.Dependency.Complete();

                // var setColorJob = new SetColor()
                // {
                //     Entities = ballEntities,
                // };
                //
                // state.Dependency = setColorJob.Schedule(totalBalls, 64, state.Dependency);
                // state.Dependency.Complete();
                
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(state.EntityManager);
            // TODO: all Prefabs are currently forced to TransformUsageFlags.Dynamic by default, which means boids get a LocalTransform
            // they don't need. As a workaround, remove the component at spawn-time.
            state.EntityManager.RemoveComponent<LocalTransform>(_ballQuery);
        }
    }

    [BurstCompile]
    struct SetBallLocalToWorld : IJobParallelFor
    {
        [NativeDisableContainerSafetyRestriction]
        [NativeDisableParallelForRestriction]
        public ComponentLookup<LocalToWorld> LocalToWorldFromEntity;

        public NativeArray<Entity> Entities;
        public float3 Center;
        public float Columns;
        public float Rows;

        public void Execute(int i)
        {
            var entity = Entities[i];
            var random = new Unity.Mathematics.Random(((uint)(entity.Index + i + 1) * 0x9F6ABC1));
            var dir = math.normalizesafe(random.NextFloat3() - new float3(0.5f, 0.5f, 0.5f));
            float3 origin = new float3(Center.x - Columns/2, 1.0f, Center.z - Rows/2);
            var pos = new float3(origin.x + i%Columns, origin.y, origin.z + i/Columns) * 5;

            var random2 = new Unity.Mathematics.Random(((uint)(entity.Index + i + 1) * 0x9F6ABC1)).NextFloat() + 0.5f;
            var newScale = new Vector3(random2, random2, random2);
            
            var localToWorld = new LocalToWorld
            {
                Value = float4x4.TRS(pos, quaternion.LookRotationSafe(dir, math.up()), newScale)
            };
            LocalToWorldFromEntity[entity] = localToWorld;
        }
    }

    [BurstCompile]
    struct SetColor : IJobParallelFor
    {
        [NativeDisableContainerSafetyRestriction]
        [NativeDisableParallelForRestriction]
        public ComponentLookup<MyOwnColor> MyOwnColorEntity;

        public NativeArray<Entity> Entities;

        public void Execute(int i)
        {
            var entity = Entities[i];
            var random = new Unity.Mathematics.Random(((uint)(entity.Index + i + 1) * 0x9F6ABC1));
            var dir = math.normalizesafe(random.NextFloat3() - new float3(0.5f, 0.5f, 0.5f));

            var t = Time.deltaTime;
            
            var myOwnColor = new MyOwnColor
            {
                Value = new float4(
                    math.cos(t + 1.0f),
                    math.cos(t + 2.0f),
                    math.cos(t + 3.0f),
                    1.0f)
            };
            MyOwnColorEntity[entity] = myOwnColor;
        }
    }
}
