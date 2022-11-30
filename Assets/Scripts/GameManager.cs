using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;

public class GameManager : MonoBehaviour
{
    private const int MAX_QUANTITY_ENEMIES = 200;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private int spawnsPerWave = 5;

    ObjectFactory objectFactory = new ObjectFactory();
    EnemyManager enemyManager;

    // Start is called before the first frame update
    void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemies = enemyManager.CreateEnemyPool(MAX_QUANTITY_ENEMIES, enemyPrefab);
        CreateEnemies(MAX_QUANTITY_ENEMIES);
        enemyManager.SpawnEnemies(spawnsPerWave);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            Debug.Log("New wave incoming!!");
            enemyManager.SpawnEnemies(spawnsPerWave);
        }
    }


    void CreateEnemies(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            objectFactory.CreateItem(IGameFactory.Item.Enemy, enemies[i]);
        }
    }
}
