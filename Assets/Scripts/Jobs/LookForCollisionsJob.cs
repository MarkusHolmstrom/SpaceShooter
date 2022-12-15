using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace SpaceJobs
{
    [BurstCompile]
    public struct LookForCollisionsJob : IJob
    {
        [ReadOnly] public NativeArray<float3> ShipLocations;
        [ReadOnly] public NativeArray<float3> ProjectileLocations;
        public int MinDistanceForHit;

        public NativeArray<float3> ShipCollisions;
        public NativeArray<float3> ProjectileCollisions;


        // Execute() is called when the job runs.
        public void Execute()
        {
            // Compute the square distance from each seeker to every target.
            for (int i = 0; i < ProjectileLocations.Length; i++)
            {
                float3 projPos = ProjectileLocations[i];
                for (int j = 0; j < ShipLocations.Length; j++)
                {
                    float3 targetPos = ShipLocations[j];
                    float distSq = math.distancesq(projPos, targetPos);
                    if (distSq < MinDistanceForHit)
                    {
                        ProjectileCollisions[i] = ProjectileLocations[i];
                        ShipCollisions[j] = ShipLocations[j];
                    }
                }
            }
        }
    }
}

