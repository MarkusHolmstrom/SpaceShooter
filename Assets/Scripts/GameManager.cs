using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;
using Unity.Collections;
using Unity.Mathematics;
using static UnityEngine.GraphicsBuffer;
using Unity.Jobs;
using SpaceJobs;
using System.Linq;

namespace Space
{
    public class GameManager : MonoBehaviour
    {
        private const int MAX_QUANTITY_ENEMIES = 20;
        private const int MAX_QUANTITY_PROJECTILES = 40;
        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private List<GameObject> enemies = new List<GameObject>();
        [SerializeField]
        private int spawnsPerWave = 5;
        ObjectFactory objectFactory = new ObjectFactory();
        EnemyManager enemyManager;
        ProjectileManager projManager;

        // For lookForCollisionsJob 
        public NativeArray<CollisionObject> ShipObjects;
        public NativeArray<CollisionObject> ProjectileObjects;
        public int MinDistanceForHit = 2;
        public NativeArray<int> ShipCollisionIDs;
        public NativeArray<int> ProjectileCollisionIDs;

        public static Transform[] shipTransforms;
        public static Transform[] projectileTransforms;

        // Start is called before the first frame update
        void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            projManager = GetComponent<ProjectileManager>();
            enemies = enemyManager.CreateEnemyPool(MAX_QUANTITY_ENEMIES, enemyPrefab);
            CreateEnemies(MAX_QUANTITY_ENEMIES);
            enemyManager.SpawnEnemies(spawnsPerWave);

            
        }

        public void OnDestroy()
        {
            CleanUp();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.H))
            {
                enemyManager.SpawnEnemies(spawnsPerWave);
            }

            SetupTransforms();
            SetPositions();
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
            ShipObjects = new NativeArray<CollisionObject>(shipActive, Allocator.Persistent);
            ProjectileObjects = new NativeArray<CollisionObject>(projActive, Allocator.Persistent);

            projectileTransforms = new Transform[projActive];
            //Debug.Log(projManager.activeProjectiles.Count);
            for (int i = 0; i < projActive; i++)
            {
                GameObject go = projManager.activeProjectiles[i];
                projectileTransforms[i] = go.transform;
            }
            //Debug.LogWarning(enemyManager.activeEnemies.Count);
            shipTransforms = new Transform[shipActive];
            for (int i = 0; i < shipActive; i++)
            {
                GameObject go = enemyManager.activeEnemies[i];
                shipTransforms[i] = go.transform;
            }
        }

        private void SetPositions()
        {
            //if (true)
            //{
            //    return;
            //}
            for (int i = 0; i < ShipObjects.Length; i++)
            {
                CollisionObject Ship = new CollisionObject(
                    shipTransforms[i].localPosition, i);

                ShipObjects[i] = Ship;
            }

            for (int i = 0; i < ProjectileObjects.Length; i++)
            {
                CollisionObject Projectile = new CollisionObject(
                    projectileTransforms[i].localPosition, i);
                
                ProjectileObjects[i] = Projectile;
            }

            ShipCollisionIDs = new NativeArray<int>(100, Allocator.Persistent);
            ProjectileCollisionIDs = new NativeArray<int>(100, Allocator.Persistent);


            LookForCollisionsJob collisionsJob = new LookForCollisionsJob
            {
                Ships = ShipObjects,
                Projectiles = ProjectileObjects,
                MinDistanceForHit = this.MinDistanceForHit,
                ShipCollisionIDs = this.ShipCollisionIDs,
                ProjectileCollisionIDs = this.ProjectileCollisionIDs
            };

            // Schedule() puts the job instance on the job queue.
            JobHandle findHandle = collisionsJob.Schedule();

            // The Complete method will not return until the job represented by 
            // the handle finishes execution. Effectively, the main thread waits
            // here until the job is done.
            findHandle.Complete();

            if (collisionsJob.Collision)
            {
                Debug.Log(collisionsJob.ProjectileCollisionIDs);
                foreach (int ID in collisionsJob.ProjectileCollisionIDs)
                {
                    Debug.Log("Nu kommer proj id:");

                    Debug.Log(ID);
                }
                foreach (int ID in collisionsJob.ShipCollisionIDs)
                {
                    Debug.LogWarning("Nu kommer ship id:");

                    Debug.LogWarning(ID);
                }
            }

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