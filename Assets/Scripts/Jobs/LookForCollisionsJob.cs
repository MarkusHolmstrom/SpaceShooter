using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace SpaceJobs
{
    [BurstCompile]
    public struct CollisionObject
    {
        public CollisionObject(float3 Position, int ID)
        {
            ObjectPosition = Position;
            this.ID = ID;
        }

        public float3 ObjectPosition;
        public int ID;
    }

    [BurstCompile]
    public struct LookForCollisionsJob : IJob
    {
        [ReadOnly] public NativeArray<CollisionObject> Ships;

        //[ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<CollisionObject> Projectiles;
        public int MinDistanceForHit;
        public bool Collision;
        // returns lists of ID for objects if Collision is true

        public NativeArray<int> ShipCollisionIDs;
        public NativeArray<int> ProjectileCollisionIDs;

        private int shipIDCount;
        private int projIDCount;

        // Execute() is called when the job runs.
        public void Execute()
        {
            // Compute the square distance from each seeker to every target.
            for (int i = 0; i < Projectiles.Length; i++)
            {
                float3 projPos = Projectiles[i].ObjectPosition;
                for (int j = 0; j < Ships.Length; j++)
                {
                    float3 targetPos = Ships[j].ObjectPosition;
                    float distSq = math.distancesq(projPos, targetPos);
                    if (distSq < MinDistanceForHit)
                    {
                        Collision = true;
                        projIDCount++;
                        ProjectileCollisionIDs[projIDCount] = Projectiles[i].ID;
                        shipIDCount++;
                        ShipCollisionIDs[shipIDCount] = Ships[i].ID;
                    }
                }
            }
        }
    }
}

