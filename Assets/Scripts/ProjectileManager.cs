using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private int maxActive = 0;
    private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField]
    private GameObject projectilePrefab;
    private int quantityActive = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public List<GameObject> CreateProjectilePool(int quantity)
    {
        maxActive += quantity;
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemy = Instantiate(projectilePrefab);
            enemy.SetActive(false);
            projectiles.Add(enemy);
        }
        return projectiles;
    }

    public GameObject ShowNewProjectile(int index)
    {
        AddActiveQuantity();
        return projectiles[index];
    }

    private void AddActiveQuantity()
    {
        if (quantityActive < maxActive)
        {
            quantityActive++;
        }
        else
        {
            CreateProjectilePool(10);
        }
    }

    public void DecreaseActiveQuantity()
    {
        if (quantityActive > 0)
        {
            quantityActive--;
        }
    }
}
