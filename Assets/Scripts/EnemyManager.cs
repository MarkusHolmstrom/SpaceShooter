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
        if (currentIndex >= maxEnemies)
        {
            currentIndex = 0;
        }
        Debug.Log("hopp");
        for (int i = 0; i < quantity && unusedEnemies.Count > i; i++)
        {
            GameObject go = enemyObjectPool.GetPooledObject();
            go.transform.position = GetEnemySpawnLocation(currentIndex);
            go.SetActive(true);
            //unusedEnemies[i].transform.position = GetEnemySpawnLocation(currentIndex);
            //unusedEnemies[i].SetActive(true);
            //activeEnemies.Add(unusedEnemies[i]);
            //unusedEnemies.Remove(unusedEnemies[i]);
        }
        currentIndex += quantity;

    }

    int test = 0;
    private Vector3 GetEnemySpawnLocation(int curIndex)
    {
        Vector3 spawnLocation = Vector3.zero;
        float randX = 40;
        float randY = 30;
        if (test == 0) //curIndex <= MAX_QUANTITY_ENEMIES / 2)
        {
            randX = LCGRandomGenerator.RandomLCGfloat(20, 50);
            randY = LCGRandomGenerator.RandomLCGfloat(-40, 40);
            test++;
        }
        else if (test == 1) //curIndex <= MAX_QUANTITY_ENEMIES)
        {
            randX = LCGRandomGenerator.RandomLCGfloat(-40, 50);
            randY = LCGRandomGenerator.RandomLCGfloat(-20, -40);
            test = 0;
        }
        spawnLocation.x = randX;
        spawnLocation.y = randY;
        return spawnLocation;
    }
    // TODO change to eventsystem or smthing
    public void UpdateEnemyLists(GameObject enemyShip)
    {
        activeEnemies.Remove(enemyShip);
        unusedEnemies.Add(enemyShip);
    }
}
