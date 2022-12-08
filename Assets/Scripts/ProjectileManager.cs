using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TypeProjectile
{
    public GameObject Projectile { get; set; }
    public bool Enemy { get; set; }
    public bool Active { get; set; }
    public int Index { get; set; }

    public void SetActive(bool active)
    {
        Active = active;
    }
    public void SetIndex(int index)
    {
        Index = index;
    }
}

public class ProjectileManager : MonoBehaviour
{
    public List<GameObject> activeProjectiles = new List<GameObject>();
    //private List<GameObject> enemyProjectiles = new List<GameObject>();
    //private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject enemyProjectilePrefab;
    [SerializeField]
    private int projectileQuantity = 5;
    
    public List<TypeProjectile> enemyProjectiles = new List<TypeProjectile>();
    public List<TypeProjectile> projectiles = new List<TypeProjectile>();

    // Start is called before the first frame update
    void Awake()
    {
        

        CreateProjectilePool(projectileQuantity);
    }

    private void CreateProjectilePool(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            // Spawn enemyproj for pool and add to struct and lists
            GameObject enemyProjGO = Instantiate(enemyProjectilePrefab);

            TypeProjectile enemyProjectile = new TypeProjectile
            {
                Projectile = enemyProjGO,
                Enemy = true,
                Active = false,
                Index = i
            };
            enemyProjGO.GetComponent<Projectile>().projectile = enemyProjectile;
            enemyProjGO.GetComponent<Projectile>().typeIndex = i;
            Debug.LogWarning(enemyProjGO.GetComponent<Projectile>().typeIndex);

            enemyProjGO.SetActive(false);
            enemyProjectiles.Add(enemyProjectile);

            // Spawn players proj for pool and add to struct and lists
            GameObject projGO = Instantiate(projectilePrefab);

            TypeProjectile projectile = new TypeProjectile
            {
                Projectile = projGO,
                Enemy = false,
                Active = false,
                Index = i
            };
            projGO.GetComponent<Projectile>().projectile = projectile;
            projGO.GetComponent<Projectile>().typeIndex = i;
            projGO.SetActive(false);
            projectiles.Add(projectile);

        }
    }

    public GameObject GetPrefab(bool enemy)
    {
        if (enemy)
        {
            return GetNewProjectile(enemyProjectiles);
        }
        else
        {
            return GetNewProjectile(projectiles);
        }
    }

    private GameObject GetNewProjectile(List<TypeProjectile> typeProjs)
    {
        for (int i = 0; i < typeProjs.Count; i++)
        {
            if (!typeProjs[i].Active)
            {
                typeProjs[i].SetActive(true);
                typeProjs[i].Projectile.SetActive(true);
                activeProjectiles.Add(typeProjs[i].Projectile);
                return typeProjs[i].Projectile;
            }
        }
        CreateProjectilePool(5);
        return GetNewProjectile(typeProjs);
    }

    public void DeActivateProjectile(bool enemy, int index, TypeProjectile proj)
    {
        if (enemy)
        {
            Deactivate(enemyProjectiles, index, proj);
        }
        else
        {
            Deactivate(projectiles, index, proj);
        }
    }

    private void Deactivate(List<TypeProjectile> typeProjs, int index, TypeProjectile proj)
    {
        for (int i = 0; i < typeProjs.Count; i++)
        {
            //Debug.Log(typeProjs[i].Index + " " + proj.Index);
            if (typeProjs[i].Active && typeProjs[i].Index == proj.Index)
            {
                Debug.LogWarning("match!");
                Debug.LogWarning(typeProjs[i].Projectile.name);
                typeProjs[i].SetActive(false);
                typeProjs[i].Projectile.SetActive(false);

            }
        }
    }
}
