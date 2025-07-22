using System;
using Unity.Entities;
using UnityEngine;

namespace DOTSTest
{
    public class BallAuthoring : MonoBehaviour
    {
        public float MoveSpeed = 25.0f;

        class Baker : Baker<BallAuthoring>
        {
            public override void Bake(BallAuthoring authoring)
            {
                Debug.Log("[XXX][BallAuthoring.Baker.Bake]");
                var entity = GetEntity(TransformUsageFlags.Renderable | TransformUsageFlags.WorldSpace);
                AddSharedComponent(entity, new Ball
                {
                    MoveSpeed = authoring.MoveSpeed
                });
            }
        }
    }

    [Serializable]
    public struct Ball : ISharedComponentData
    {
        public float MoveSpeed;
    }
}
