using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class BulletMovement : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithoutBurst().ForEach((Transform transform, ref Translation t, in Movement movementData) =>
        {
            //float acceleration_right = movementData.Direction.x * movementData.MovementSpeed * deltaTime;
            //float acceleration_forward = movementData.Direction.y * movementData.MovementSpeed * deltaTime;

            //float3 acceleration = float3.zero;
            //acceleration.x = acceleration_right;
            //acceleration.y = acceleration_forward;
            float3 acceleration = transform.transform.forward * movementData.MovementSpeed * Time.DeltaTime;
            t.Value.xyz += acceleration;
            transform.position = t.Value;
        }).Run();
    }
}

//public struct Movement : IComponentData
//{
//    public int MovementSpeed { get; set; }
//    public float3 Direction { get; set; }

//    public Movement(int speed, float3 direction)
//    {
//        MovementSpeed = speed;
//        Direction = direction;
//    }
//}
