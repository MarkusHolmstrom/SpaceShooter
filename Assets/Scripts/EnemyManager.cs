using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemyManager : MonoBehaviour
{
    public List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();

    private ObjectPool enemyObjectPool;

    private int divider = 0;

    private void Awake()
    {
        enemyObjectPool = GetComponent<ObjectPool>();
    }

    public List<GameObject> CreateEnemyPool(int quantity, GameObject enemyPrefab)
    {
        enemyObjectPool.PoolObjects(quantity, enemyPrefab);
        enemies = enemyObjectPool.pooledObjects;
        return enemies;
    }

    public void SpawnEnemies(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject go = enemyObjectPool.GetPooledObject();
            if (go != null)
            {
                go.transform.position = GetEnemySpawnLocation();
                go.SetActive(true);
                activeEnemies.Add(go);
            }
        }
    }

    private Vector3 GetEnemySpawnLocation()
    {
        Vector3 spawnLocation = Vector3.zero;
        float randY = 30;
        float randZ = 40;
        // just to make two different places where enemies
        // spawn so they dont come from the same place
        if (divider == 0) 
        {
            randY = LCGRandomGenerator.RandomLCGfloat(-40, 40);
            randZ = LCGRandomGenerator.RandomLCGfloat(20, 100);
            divider++;
        }
        else if (divider == 1) 
        {
            randY = LCGRandomGenerator.RandomLCGfloat(-20, -40);
            randZ = LCGRandomGenerator.RandomLCGfloat(0, 50);
            divider = 0;
        }
        spawnLocation.y = randY;
        spawnLocation.z = randZ;
        return spawnLocation;
    }
    // TODO change to eventsystem or smthing
    public void UpdateEnemyLists(GameObject enemyShip)
    {
        activeEnemies.Remove(enemyShip);
    }
}
