using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTSTest
{
    [RequireMatchingQueriesForUpdate]
    [BurstCompile]
    public partial struct BallSpawningSystem : ISystem
    {
        private EntityQuery _ballQuery;
        public void OnCreate(ref SystemState state)
        {
            _ballQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Ball, LocalTransform>().Build(ref state);
        }

        public void OnUpdate(ref SystemState state)
        {
            var localToWorldLookup = SystemAPI.GetComponentLookup<LocalToWorld>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var world = state.World.Unmanaged;

            foreach (var (ballGroup, ballGroupLocalToWorld, entity) in
                     SystemAPI.Query<RefRO<BallGroup>, RefRO<LocalToWorld>>()
                         .WithEntityAccess())
            {
                var boidEntities =
                    CollectionHelper.CreateNativeArray<Entity, RewindableAllocator>(ballGroup.ValueRO.Count,
                        ref world.UpdateAllocator);

                state.EntityManager.Instantiate(ballGroup.ValueRO.Prefab, boidEntities);

                var setBoidLocalToWorldJob = new SetBallLocalToWorld
                {
                    LocalToWorldFromEntity = localToWorldLookup,
                    Entities = boidEntities,
                    Center = ballGroupLocalToWorld.ValueRO.Position
                };
                state.Dependency = setBoidLocalToWorldJob.Schedule(ballGroup.ValueRO.Count, 64, state.Dependency);
                state.Dependency.Complete();

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
        public float Radius;

        public void Execute(int i)
        {
            var entity = Entities[i];
            var random = new Random(((uint)(entity.Index + i + 1) * 0x9F6ABC1));
            var dir = math.normalizesafe(random.NextFloat3() - new float3(0.5f, 0.5f, 0.5f));
            var pos = Center + (dir * Radius);
            var localToWorld = new LocalToWorld
            {
                Value = float4x4.TRS(pos, quaternion.LookRotationSafe(dir, math.up()), new float3(1.0f, 1.0f, 1.0f))
            };
            LocalToWorldFromEntity[entity] = localToWorld;
        }
    }
}
