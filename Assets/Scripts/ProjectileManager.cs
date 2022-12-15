using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class ProjectileManager : MonoBehaviour
{
    public List<GameObject> activeProjectiles = new List<GameObject>();

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject enemyProjectilePrefab;
    [SerializeField]
    private int projectileQuantity = 5;
    
    private ObjectPool objectPool;
    private ObjectPool enemyObjectPool;

    // Start is called before the first frame update
    void Awake()
    {

        objectPool = GetComponent<ObjectPool>();
        // Create a new list of gameobjects for enemy projectiles
        enemyObjectPool = gameObject.AddComponent<ObjectPool>();

        CreateProjectilePool(projectileQuantity);
    }

    private void CreateProjectilePool(int quantity)
    {
        objectPool.PoolObjects(quantity, projectilePrefab);
        enemyObjectPool.PoolObjects(quantity, enemyProjectilePrefab);
    }

    public GameObject GetPrefab(bool enemy)
    {
        if (enemy)
        {
            GameObject go = enemyObjectPool.GetPooledObject();
            if (go != null) { activeProjectiles.Add(go); }
            return go;
        }
        else
        {
            GameObject go = objectPool.GetPooledObject();
            if (go != null) { activeProjectiles.Add(go); }
            return go;
        }
    }

    public void DeActivateProjectile(GameObject proj)
    {
        activeProjectiles.Remove(proj);
    }
}
