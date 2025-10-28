using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Random = Unity.Mathematics.Random;

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

                var rand = new Random();
                rand.InitState();
                var color = rand.NextFloat4(0f, 1.0f);
                color.w = 1.0f;
                Debug.Log($"[{this.GetName()}] color: {color}");
                
                AddSharedComponent(entity, new Ball
                {
                    MoveSpeed = authoring.MoveSpeed
                });
                AddComponent(entity, new MyColor
                {
                    Value = color
                });
            }
        }
    }

    [Serializable]
    public struct Ball : ISharedComponentData
    {
        public float MoveSpeed;
    }
    
    [Serializable]
    [MaterialProperty("_Color")]
    public struct MyColor : IComponentData
    {
        public float4 Value;
    }

}
