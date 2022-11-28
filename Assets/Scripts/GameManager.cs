using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;

public class GameManager : MonoBehaviour
{
    private const int MAX_QUANTITY_ENEMIES = 20;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private int spawnsPerWave = 5;
    private int currentIndex = 0;

    ObjectFactory objectFactory = new ObjectFactory();

    // Start is called before the first frame update
    void Awake()
    {
        CreateEnemyPool(MAX_QUANTITY_ENEMIES);
        CreateEnemies(MAX_QUANTITY_ENEMIES);
        SpawnEnemies(spawnsPerWave);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            Debug.Log("New wave incoming!!");
            SpawnEnemies(spawnsPerWave);
        }
    }

    List<GameObject> CreateEnemyPool(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }
        return enemies;
    }

    void CreateEnemies(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            objectFactory.CreateItem(IGameFactory.Item.Enemy, enemies[i]);
        }
    }

    void SpawnEnemies(int quantity)
    {
        if (currentIndex >= MAX_QUANTITY_ENEMIES)
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
    Vector3 GetEnemySpawnLocation(int curIndex)
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
