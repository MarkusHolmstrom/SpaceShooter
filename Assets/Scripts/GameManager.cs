using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;
using Unity.Collections;
using Unity.Mathematics;
using static UnityEngine.GraphicsBuffer;
using Unity.Jobs;

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

        ShipObjects = new NativeArray<CollisionObject>(MAX_QUANTITY_ENEMIES, Allocator.Persistent);
        ProjectileObjects = new NativeArray<CollisionObject>(MAX_QUANTITY_PROJECTILES, Allocator.Persistent);
        
    }

    public void OnDestroy()
    {
        ShipObjects.Dispose();
        ProjectileObjects.Dispose();
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
        projectileTransforms = new Transform[MAX_QUANTITY_PROJECTILES];
        for (int i = 0; i < projManager.activeProjectiles.Count; i++)
        {
            GameObject go = projManager.activeProjectiles[i];
            projectileTransforms[i] = go.transform;
        }

        shipTransforms = new Transform[MAX_QUANTITY_ENEMIES];
        for (int i = 0; i < enemyManager.activeEnemies.Count; i++)
        {
            GameObject go = enemyManager.activeEnemies[i];
            shipTransforms[i] = go.transform;
        }
    }

    private void SetPositions()
    {
        for (int i = 0; i < ShipObjects.Length; i++)
        {
            CollisionObject Ship = new CollisionObject
            {
                ObjectPosition = shipTransforms[i].localPosition,
                ID = i
            };
            // Vector3 is implicitly converted to float3
            ShipObjects[i] = Ship;
        }

        for (int i = 0; i < ProjectileObjects.Length; i++)
        {
            CollisionObject Projectile = new CollisionObject
            {
                ObjectPosition = projectileTransforms[i].localPosition,
                ID = i
            };

            ProjectileObjects[i] = Projectile;
        }

        LookForCollisionsJob collisionsJob = new LookForCollisionsJob
        {
            Ships = ShipObjects,
            Projectiles = ProjectileObjects,
            MinDistanceForHit = this.MinDistanceForHit
        };

        // Schedule() puts the job instance on the job queue.
        JobHandle findHandle = collisionsJob.Schedule();

        // The Complete method will not return until the job represented by 
        // the handle finishes execution. Effectively, the main thread waits
        // here until the job is done.
        findHandle.Complete();
    
        if (collisionsJob.Collision)
        {
            foreach(int ID in collisionsJob.ProjectileCollisionIDs)
            {
                Debug.Log(ID);
            }
            foreach (int ID in collisionsJob.ShipCollisionIDs)
            {
                Debug.LogWarning(ID);
            }
        }

    }
}
