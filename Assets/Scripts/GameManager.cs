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

namespace SpaceJobs
{
    [RequireComponent(typeof(EnemyManager))]
    public class GameManager : MonoBehaviour
    {
        private const int MAX_QUANTITY_ENEMIES = 100;

        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private List<GameObject> enemies = new List<GameObject>();
        [SerializeField]
        private int enemiesPerWave = 5;
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

        [SerializeField]
        private BulletMovement bulletMovement;

        // Start is called before the first frame update
        void Awake()
        {
            bulletMovement = new BulletMovement();
            enemyManager = GetComponent<EnemyManager>();
            projManager = GetComponent<ProjectileManager>();
        }

        private void Start()
        {
            enemies = enemyManager.CreateEnemyPool(MAX_QUANTITY_ENEMIES, enemyPrefab);
            CreateEnemies(MAX_QUANTITY_ENEMIES);
            enemyManager.SpawnEnemies(enemiesPerWave);
        }

        public void OnDestroy()
        {
            CleanUp();
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

            //SetupTransforms();
            //SetPositions();
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
            ShipObjects = new NativeArray<CollisionObject>(shipActive, Allocator.TempJob);
            ProjectileObjects = new NativeArray<CollisionObject>(projActive, Allocator.TempJob);

            //Debug.LogWarning(enemyManager.activeEnemies.Count);
            shipTransforms = new Transform[shipActive];
            for (int i = 0; i < shipActive; i++)
            {
                GameObject go = enemyManager.activeEnemies[i];
                shipTransforms[i] = go.transform;
            }

            projectileTransforms = new Transform[projActive];
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
                CollisionObject Ship = new CollisionObject(
                    shipTransforms[i].localPosition, i);

                ShipObjects[i] = Ship;
                //Debug.Log(Ship.ObjectPosition + " " + Ship.ID);
            }

            for (int i = 0; i < ProjectileObjects.Length; i++)
            {
                CollisionObject Projectile = new CollisionObject(
                    projectileTransforms[i].localPosition, i);
                
                ProjectileObjects[i] = Projectile;
                //Debug.Log(Projectile.ObjectPosition + " " + Projectile.ID);
            }

            ShipCollisionIDs = new NativeArray<int>(100, Allocator.TempJob);
            ProjectileCollisionIDs = new NativeArray<int>(100, Allocator.TempJob);
            bool coll = false;
            LookForCollisionsJob collisionsJob = new LookForCollisionsJob
            {
                Ships = ShipObjects,
                Projectiles = ProjectileObjects,
                MinDistanceForHit = this.MinDistanceForHit,
                ShipCollisionIDs = this.ShipCollisionIDs,
                ProjectileCollisionIDs = this.ProjectileCollisionIDs,
                Collision = coll
            };
            //Debug.Log(collisionsJob.Ships.Length + " " + collisionsJob.Projectiles.Length
            //    + " " + collisionsJob.MinDistanceForHit + " " + collisionsJob.ShipCollisionIDs.Length
            //    + " " + collisionsJob.ProjectileCollisionIDs.Length);

            // Schedule() puts the job instance on the job queue.
            JobHandle findHandle = collisionsJob.Schedule();
            
            if (coll)
            {
                Debug.Log(collisionsJob.test + " " + collisionsJob.shiptest);
            }
            //
            
            // The Complete method will not return until the job represented by 
            // the handle finishes execution. Effectively, the main thread waits
            // here until the job is done.
            findHandle.Complete();

            int shipIDCount = 0;
            int projIDCount = 0;
            
            foreach (int h in ProjectileCollisionIDs)
            {
                if (h > 0)
                {
                    projIDCount++;
                }
            }
            Debug.Log("efter: " + collisionsJob.test + " " + collisionsJob.shiptest);
            if (coll)
            {
                Debug.Log("efter: " + shipIDCount + " " + projIDCount);
            }
            if (collisionsJob.Collision)
            {
                Debug.Log(collisionsJob.ProjectileCollisionIDs.Length);
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