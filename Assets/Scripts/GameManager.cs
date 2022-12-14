using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;
using Unity.Collections;
using Unity.Mathematics;
using static UnityEngine.GraphicsBuffer;
using Unity.Jobs;
using System.Linq;
using Unity.VisualScripting;
using Unity.Entities.UniversalDelegates;
using Unity.Burst.CompilerServices;

namespace SpaceJobs
{
    [RequireComponent(typeof(EnemyManager))]
    public class GameManager : MonoBehaviour
    {
        private const int MAX_QUANTITY_ENEMIES = 50;

        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private List<GameObject> enemies = new List<GameObject>();
        [SerializeField]
        private int enemiesPerWave = 5;
        ObjectFactory objectFactory = new ObjectFactory();
        EnemyManager enemyManager;
        [SerializeField]
        private ProjectileManager projManager;

        // For lookForCollisionsJob 
        public NativeArray<float3> ShipObjects;
        public NativeArray<float3> ProjectileObjects;
        public int MinDistanceForHit = 5;
        public NativeArray<float3> ShipCollisionIDs;
        public NativeArray<float3> ProjectileCollisionIDs;

        public static Transform[] shipTransforms;
        public static Transform[] projectileTransforms;
        [SerializeField]
        private float searchOffset = 6f;

        // Start is called before the first frame update
        void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        private void Start()
        {
            enemies = enemyManager.CreateEnemyPool(MAX_QUANTITY_ENEMIES, enemyPrefab);
            CreateEnemies(MAX_QUANTITY_ENEMIES);
            enemyManager.SpawnEnemies(enemiesPerWave);
        }

        // Update is called once per frame
        void Update()
        {
            //bulletMovement.Update();
            if (Input.GetKeyUp(KeyCode.H))
            {
                // TODO set this as a timer asswell... 
                enemyManager.SpawnEnemies(enemiesPerWave);
            }

            SetupTransforms();
            SetPositions();
        }

        public void OnDestroy()
        {
            CleanUp();
        }

        private void CreateEnemies(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                objectFactory.CreateItem(IGameFactory.Item.Enemy, enemies[i]);
            }
        }

        private void SetupTransforms()
        {
            int shipActive = enemyManager.activeEnemies.Count;
            int projActive = projManager.activeProjectiles.Count;
            ShipObjects = new NativeArray<float3>(shipActive, Allocator.TempJob);
            ProjectileObjects = new NativeArray<float3>(projActive, Allocator.TempJob);

            //Debug.LogWarning(enemyManager.activeEnemies.Count);
            shipTransforms = new Transform[shipActive];
            for (int i = 0; i < shipActive; i++)
            {
                GameObject go = enemyManager.activeEnemies[i];
                shipTransforms[i] = go.transform;
            }

            projectileTransforms = new Transform[projActive];
            //GameObject[] = GameObject.FindGameObjectsWithTag("Projectile");
            //Debug.Log(projManager.activeProjectiles.Count);
            for (int i = 0; i < projActive; i++)
            {
                GameObject go = projManager.activeProjectiles[i];
                projectileTransforms[i] = go.transform;
            }
        }

        private void SetPositions()
        {
            //if (true)
            //{
            //    return;
            //}
            //Debug.Log("nu kor vi igen...");
            for (int i = 0; i < ShipObjects.Length; i++)
            {

                ShipObjects[i] = shipTransforms[i].localPosition;
                //Debug.Log(ShipObjects[i]);
            }

            for (int i = 0; i < ProjectileObjects.Length; i++)
            {
                
                ProjectileObjects[i] = projectileTransforms[i].localPosition;
                //Debug.LogWarning(ProjectileObjects[i]);
            }

            ShipCollisionIDs = new NativeArray<float3>(MAX_QUANTITY_ENEMIES, Allocator.TempJob);
            ProjectileCollisionIDs = new NativeArray<float3>(MAX_QUANTITY_ENEMIES, Allocator.TempJob);
            
            LookForCollisionsJob collisionsJob = new LookForCollisionsJob
            {
                ShipLocations = ShipObjects,
                ProjectileLocations = ProjectileObjects,
                MinDistanceForHit = this.MinDistanceForHit,
                ShipCollisions = this.ShipCollisionIDs,
                ProjectileCollisions = this.ProjectileCollisionIDs,
            };
            //Debug.Log(collisionsJob.Ships.Length + " " + collisionsJob.Projectiles.Length
            //    + " " + collisionsJob.MinDistanceForHit + " " + collisionsJob.ShipCollisionIDs.Length
            //    + " " + collisionsJob.ProjectileCollisionIDs.Length);

            // Schedule() puts the job instance on the job queue.
            JobHandle findHandle = collisionsJob.Schedule();
            
            // The Complete method will not return until the job represented by 
            // the handle finishes execution. Effectively, the main thread waits
            // here until the job is done.
            findHandle.Complete();

            for (int i = 0; i < collisionsJob.ProjectileCollisions.Length; i++)
            {
                float distance = Vector3.Distance(collisionsJob.ProjectileCollisions[i],
                    collisionsJob.ShipCollisions[i]);
                //float distanceTwo = Vector3.Distance(projectileTransforms[i].position,
                //    shipTransforms[i].position);
                if (distance > 0)
                {
                    RaycastHit hit;
                    Vector3 start = collisionsJob.ProjectileCollisions[i];
                    float length = MinDistanceForHit + searchOffset;
                    if (Physics.SphereCast(start, length, transform.forward, out hit, length))
                    {
                        GameObject go = hit.rigidbody.gameObject;
                        if (go.GetComponent<IShip>() != null)
                        {
                            Debug.Log(go.name);
                            go.SetActive(false);

                        }
                    }
                    // jadu-....
                    start = collisionsJob.ShipCollisions[i];
                    if (Physics.SphereCast(start, length, transform.forward, out hit, length))
                    {
                        GameObject go = hit.rigidbody.gameObject;
                        if (go.GetComponent<Projectile>() != null)
                        {
                            Debug.LogWarning(go.name);
                            go.SetActive(false);

                        }
                    }
                    Debug.DrawLine(collisionsJob.ProjectileCollisions[i],
                            collisionsJob.ShipCollisions[i]);

                }

            }
            //if (collisionsJob.Collision)
            {
                //at least this works now = P
                //Debug.Log(collisionsJob.ProjectileCollisionIDs.Length);
                //foreach (float3 ID in collisionsJob.ProjectileCollisionIDs)
                //{
                //    if (ID.y != 0 && ID.z != 0)
                //    {
                //        Debug.LogError("Nu kommer proj id:");

                //        Debug.LogError(ID);

                //    }
                //}
                //foreach (float3 ID in collisionsJob.ShipCollisionIDs)
                //{
                //    if (ID.y != 0 && ID.z != 0)
                //    {
                //        Debug.LogError("Nu kommer ship id:");

                //        Debug.LogError(ID);
                //    }
                //}
            }

            CleanUp();
        }

        private void CleanUp()
        {
            if (ShipObjects.IsCreated)
            {
                ShipObjects.Dispose();
            }
            if (ProjectileObjects.IsCreated)
            {
                ProjectileObjects.Dispose();
            }
            if (ProjectileCollisionIDs.IsCreated)
            {
                ProjectileCollisionIDs.Dispose();
            }
            if (ShipCollisionIDs.IsCreated)
            {
                ShipCollisionIDs.Dispose();
            }
        }
    }
}