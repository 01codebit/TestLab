using Unity.Entities;
using UnityEngine;

namespace DOTSTest
{
    public class BallGroupAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public int Columns;
        public int Rows;

        class Baker : Baker<BallGroupAuthoring>
        {
            public override void Bake(BallGroupAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new BallGroup
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Renderable|TransformUsageFlags.WorldSpace),
                    Columns = authoring.Columns,
                    Rows = authoring.Rows
                });
            }
        }
    }

    public struct BallGroup : IComponentData
    {
        public Entity Prefab;
        public int Columns;
        public int Rows;
    }
}
