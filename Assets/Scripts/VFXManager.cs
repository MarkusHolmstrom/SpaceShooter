using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Unity.Burst;
//using Unity.Entities;
using UnityEngine.Jobs;
using Unity.Mathematics;

// https://docs.unity3d.com/ScriptReference/Unity.Jobs.IJob.html
// An example job which increments all the numbers of an array.

// another example with float3s:
// https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/DOTS_Guide/jobs_tutorial/README.md

public struct IncrementJob : IJob
{
    [ReadOnly] public NativeArray<float3> TargetPositions;
    [ReadOnly] public NativeArray<float3> SeekerPositions;
    public NativeArray<float3> NearestTargetPositions;
    
    // Execute() is called when the job runs.
    public void Execute()
    {
        
    }
}

// A system that schedules the IJob.
//[BurstCompile]
//public partial struct MySystem : ISystem
//{
//    [BurstCompile]
//    public void OnCreate(ref SystemState state) { }

//    [BurstCompile]
//    public void OnDestroy(ref SystemState state) { }

//    [BurstCompile]
//    public void OnUpdate(ref SystemState state)
//    {
//        var job = new IncrementJob
//        {
//            Nums = new NativeArray<float>(1000, state.WorldUpdateAllocator),
//            Increment = 5f
//        };

//        JobHandle handle = job.Schedule();
//        handle.Complete();
//    }
//}
public class VFXManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
