using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TypeProjectile
{
    public bool Enemy;
    public int QuantityActive;
    public int MaxActive;
}

public class ProjectileManager : MonoBehaviour
{
    private List<GameObject> enemyProjectiles = new List<GameObject>();
    private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject enemyProjectilePrefab;
    // TODO make pricate?
    public TypeProjectile enemyProjectile;
    public TypeProjectile projectile;

    // Start is called before the first frame update
    void Awake()
    {
        enemyProjectile = new TypeProjectile
        {
            Enemy = true,
            QuantityActive = 0
        };

        projectile = new TypeProjectile
        {
            Enemy = false,
            QuantityActive = 0
        };

        CreateProjectilePool(20, enemyProjectile);
        CreateProjectilePool(20, projectile);
    }

    public List<GameObject> CreateProjectilePool(int quantity, TypeProjectile typeProj)
    {
        typeProj.MaxActive += quantity;
        for (int i = 0; i < quantity; i++)
        {
            GameObject proj;
            if (typeProj.Enemy)
            {
                proj = Instantiate(enemyProjectilePrefab);
                enemyProjectiles.Add(proj);
            }
            else
            {
                proj = Instantiate(projectilePrefab);
                projectiles.Add(proj);
            }
            proj.SetActive(false);
        }
        return projectiles;
    }

    public GameObject GetPrefab(bool enemy)
    {
        if (enemy)
        {
            return GetNewProjectile(enemyProjectile.QuantityActive + 1, enemyProjectile);
        }
        else
        {
            return GetNewProjectile(projectile.QuantityActive + 1, projectile);
        }
    }

    private GameObject GetNewProjectile(int index, TypeProjectile typeProj)
    {
        AddActiveQuantity(typeProj);
        if (typeProj.Enemy)
        {
            enemyProjectiles[index].SetActive(true);
            return enemyProjectiles[index];
        }
        projectiles[index].SetActive(true);
        return projectiles[index];
    }

    private void AddActiveQuantity(TypeProjectile typeProj)
    {
        if (typeProj.QuantityActive < typeProj.MaxActive)
        {
            typeProj.QuantityActive++;
        }
        else
        {
            CreateProjectilePool(10, typeProj);
        }
    }

    public void DecreaseActiveQuantity(bool enemy, GameObject proj)
    {
        proj.SetActive(false);
        if (enemy)
        {
            if (enemyProjectile.QuantityActive > 0)
            {
                enemyProjectile.QuantityActive--;
            }
            else
            {
                enemyProjectile.QuantityActive = 0;
            }
        }
        else
        {
            if (projectile.QuantityActive > 0)
            {
                projectile.QuantityActive--;
            }
            else
            {
                projectile.QuantityActive = 0;
            }
        }
    }
}