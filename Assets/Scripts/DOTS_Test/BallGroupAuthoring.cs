using Unity.Entities;
using UnityEngine;

namespace DOTSTest
{
    public class BallGroupAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public int Count;

        class Baker : Baker<BallGroupAuthoring>
        {
            public override void Bake(BallGroupAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new BallGroup
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Renderable|TransformUsageFlags.WorldSpace),
                    Count = authoring.Count
                });
            }
        }
    }

    public struct BallGroup : IComponentData
    {
        public Entity Prefab;
        public int Count;
    }
}
