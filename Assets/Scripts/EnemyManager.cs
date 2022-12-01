using Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int maxEnemies;

    private List<GameObject> enemies = new List<GameObject>();

    private static int currentIndex = 0;

    // TODO create system for enemies when they die
    public List<GameObject> CreateEnemyPool(int quantity, GameObject enemyPrefab)
    {
        maxEnemies = quantity;
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }
        return enemies;
    }

    public void SpawnEnemies(int quantity)
    {
        if (currentIndex >= maxEnemies)
        {
            currentIndex = 0;
        }

        quantity += currentIndex;
        for (int i = currentIndex; i < quantity && enemies.Count > i; i++)
        {
            enemies[i].transform.position = GetEnemySpawnLocation(currentIndex);
            enemies[i].SetActive(true);
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
            randX = LCGRandomGenerator.RandomLCGfloat(20, 40);
            randY = LCGRandomGenerator.RandomLCGfloat(-30, 30);
            test++;
        }
        else if (test == 1) //curIndex <= MAX_QUANTITY_ENEMIES)
        {
            randX = LCGRandomGenerator.RandomLCGfloat(-40, 40);
            randY = LCGRandomGenerator.RandomLCGfloat(-15, -30);
            test = 0;
        }
        spawnLocation.x = randX;
        spawnLocation.y = randY;
        return spawnLocation;
    }
}
