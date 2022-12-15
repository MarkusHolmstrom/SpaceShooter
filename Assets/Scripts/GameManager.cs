using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using TMPro;

namespace SpaceJobs
{
    [RequireComponent(typeof(EnemyManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int MaxQuantityEnemies = 100;

        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private List<GameObject> enemies = new List<GameObject>();
        [SerializeField]
        private int enemiesPerWave = 5;

        private EnemyManager enemyManager;
        [SerializeField]
        private ProjectileManager projManager;

        // For lookForCollisionsJob 
        public NativeArray<float3> ShipObjects;
        public NativeArray<float3> ProjectileObjects;
        [Header("Set how sensitive the jobsystem is locating nearby collisions")]
        public int MinDistanceForHit = 5;
        public NativeArray<float3> ShipCollisions;
        public NativeArray<float3> ProjectileCollisions;

        public static Transform[] shipTransforms;
        public static Transform[] projectileTransforms;

        private Transform playerTransform;
        // enemy wave variables
        [SerializeField]
        private TMP_Text waveText;
        [SerializeField]
        private float timerWave = 10f;
        private float currentTime = 0f;
        private int waveIndex = 0;

        // Start is called before the first frame update
        void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
            enemies = enemyManager.CreateEnemyPool(MaxQuantityEnemies, enemyPrefab);
            SpawnEnemies();
        }

        // Update is called once per frame
        void Update()
        {
            SpawnLoop();
            // These two functions handle the look for collision jobs
            SetupTransforms();
            SetPositions();
        }

        private void SpawnLoop()
        {
            // Spamming this button may cause an index out of range exception for some
            // reason, how fun it is with weird bugs the day before deadline, I am right?
            if (Input.GetKeyUp(KeyCode.H))
            {
                SpawnEnemies();
            }
            else if (currentTime < timerWave)
            {
                currentTime += Time.deltaTime;
            }
            else if (currentTime >= timerWave)
            {
                SpawnEnemies();
                currentTime = 0f;
            }
        }

        private void SpawnEnemies()
        {
            enemyManager.SpawnEnemies(enemiesPerWave);
            waveIndex++;
            waveText.text = "Current Wave: " + waveIndex;
        }

        public void OnDestroy()
        {
            CleanUp();
        }

        private void SetupTransforms()
        {
            // add one for the player ship
            int shipActive = enemyManager.activeEnemies.Count + 1;
            int projActive = projManager.activeProjectiles.Count;
            ShipObjects = new NativeArray<float3>(shipActive, Allocator.TempJob);
            ProjectileObjects = new NativeArray<float3>(projActive, Allocator.TempJob);

            shipTransforms = new Transform[shipActive];
            for (int i = 0; i < shipActive - 1; i++)
            {
                GameObject go = enemyManager.activeEnemies[i];
                shipTransforms[i] = go.transform;
            }
            // manually add the player transform
            shipTransforms[shipActive - 1] = playerTransform;

            projectileTransforms = new Transform[projActive];
            for (int i = 0; i < projActive; i++)
            {
                GameObject go = projManager.activeProjectiles[i];
                projectileTransforms[i] = go.transform;
            }
        }

        private void SetPositions()
        {
            for (int i = 0; i < ShipObjects.Length; i++)
            {
                ShipObjects[i] = shipTransforms[i].localPosition;
            }

            for (int i = 0; i < ProjectileObjects.Length; i++)
            {
                ProjectileObjects[i] = projectileTransforms[i].localPosition;
            }

            ShipCollisions = new NativeArray<float3>(MaxQuantityEnemies, Allocator.TempJob);
            ProjectileCollisions = new NativeArray<float3>(MaxQuantityEnemies, Allocator.TempJob);
            
            LookForCollisionsJob collisionsJob = new LookForCollisionsJob
            {
                ShipLocations = ShipObjects,
                ProjectileLocations = ProjectileObjects,
                MinDistanceForHit = this.MinDistanceForHit,
                ShipCollisions = this.ShipCollisions,
                ProjectileCollisions = this.ProjectileCollisions,
            };

            //  put the job instance on the job queue.
            JobHandle findHandle = collisionsJob.Schedule();
            
            // Wait for job to be done
            findHandle.Complete();
            // Go through all collision in an ugly way
            IterateCollisionArrays();
            // Dispose the native arrays for the job
            CleanUp();
        }

        private void IterateCollisionArrays()
        {
            // I wish I could find a better solution to deal with collision,
            // this is the best I could do for now
            for (int i = 0; i < ProjectileCollisions.Length; i++)
            {
                for (int j = 0; j < ShipCollisions.Length; j++)
                {
                    float distance = Vector3.Distance(ProjectileCollisions[i],
                        ShipCollisions[j]);

                    if (distance > 0 && distance < MinDistanceForHit)
                    {
                        // get the ship from the collision and remove it
                        GameObject go = FindCorrectObject(true, ShipCollisions[j]);
                        if (go != null && go.TryGetComponent<IShip>(out var ship))
                        {
                            ship.DoDamage(1);
                        }
                        // get the projectile from the collision and remove it
                        go = FindCorrectObject(false, ProjectileCollisions[i]);
                        if (go != null && go.TryGetComponent<Projectile>(out var proj))
                        {
                            projManager.activeProjectiles.Remove(go);
                            proj.RemoveAtCollision();

                        }
                    }
                }
            }
        }

        private GameObject FindCorrectObject(bool ship, Vector3 pos)
        {
            if (ship)
            {
                foreach (var item in shipTransforms)
                {
                    if (item.position == pos)
                    {
                        return item.gameObject;
                    }
                }
            }
            else
            {
                foreach (var item in projectileTransforms)
                {
                    if (item.position == pos)
                    {
                        return item.gameObject;
                    }
                }
            }
            return null;
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
            if (ProjectileCollisions.IsCreated)
            {
                ProjectileCollisions.Dispose();
            }
            if (ShipCollisions.IsCreated)
            {
                ShipCollisions.Dispose();
            }
        }
    }
}