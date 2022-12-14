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
            float3 acceleration = movementData.MovementSpeed * Time.DeltaTime * transform.transform.forward;
            t.Value.xyz += acceleration;
            transform.position = t.Value;
        }).Run();
    }
}

