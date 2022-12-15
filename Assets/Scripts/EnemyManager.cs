using Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemyManager : MonoBehaviour
{
    public List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    private int maxEnemies;
    private List<GameObject> unusedEnemies = new List<GameObject>();

    private static int currentIndex = 0;

    private ObjectPool enemyObjectPool;

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
        //if (currentIndex >= maxEnemies)
        //{
        //    currentIndex = 0;
        //}
        //Debug.Log("hopp");
        for (int i = 0; i < quantity; i++)
        {
            GameObject go = enemyObjectPool.GetPooledObject();
            if (go != null)
            {
                go.transform.position = GetEnemySpawnLocation();
                go.SetActive(true);
                activeEnemies.Add(go);
            }
            //unusedEnemies[i].transform.position = GetEnemySpawnLocation(currentIndex);
            //unusedEnemies[i].SetActive(true);
            //activeEnemies.Add(unusedEnemies[i]);
            //unusedEnemies.Remove(unusedEnemies[i]);
        }
        //currentIndex += quantity;

    }

    int test = 0;
    private Vector3 GetEnemySpawnLocation()
    {
        Vector3 spawnLocation = Vector3.zero;
        float randY = 30;
        float randZ = 40;
        if (test == 0) //curIndex <= MAX_QUANTITY_ENEMIES / 2)
        {
            randY = LCGRandomGenerator.RandomLCGfloat(-40, 40);
            randZ = LCGRandomGenerator.RandomLCGfloat(20, 100);
            test++;
        }
        else if (test == 1) //curIndex <= MAX_QUANTITY_ENEMIES)
        {
            randY = LCGRandomGenerator.RandomLCGfloat(-20, -40);
            randZ = LCGRandomGenerator.RandomLCGfloat(0, 50);
            test = 0;
        }
        spawnLocation.y = randY;
        spawnLocation.z = randZ;
        return spawnLocation;
    }
    // TODO change to eventsystem or smthing
    public void UpdateEnemyLists(GameObject enemyShip)
    {
        activeEnemies.Remove(enemyShip);
        unusedEnemies.Add(enemyShip);
    }
}
