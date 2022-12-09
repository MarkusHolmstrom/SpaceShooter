using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeProjectile
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
    public List<bool> actives = new List<bool>();
    public List<bool> enemyActives = new List<bool>();

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

            enemyProjGO.SetActive(false);
            enemyProjectiles.Add(enemyProjectile);
            enemyActives.Add(false);
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
            projGO.SetActive(false);
            projectiles.Add(projectile);
            actives.Add(false);
        }
    }

    public GameObject GetPrefab(bool enemy)
    {
        if (enemy)
        {
            return GetNewProjectile(enemy, enemyProjectiles);
        }
        else
        {
            return GetNewProjectile(enemy, projectiles);
        }
    }

    private GameObject GetNewProjectile(bool enemy, List<TypeProjectile> typeProjs)
    {
        for (int i = 0; i < typeProjs.Count; i++)
        {
            //if (!typeProjs[i].Active)
            if (enemy && !enemyActives[i])
            {
                typeProjs[i].SetActive(true);
                typeProjs[i].Projectile.SetActive(true);
                enemyActives[i] = true;
                activeProjectiles.Add(typeProjs[i].Projectile);
                return typeProjs[i].Projectile;
            }
            else if (!enemy && !actives[i])
            {
                typeProjs[i].SetActive(true);
                typeProjs[i].Projectile.SetActive(true);
                actives[i] = true;
                activeProjectiles.Add(typeProjs[i].Projectile);
                return typeProjs[i].Projectile;
            }
        }
        CreateProjectilePool(5);
        return GetNewProjectile(enemy,typeProjs);
    }

    public void DeActivateProjectile(bool enemy, TypeProjectile proj)
    {
        if (enemy)
        {
            Deactivate(enemyProjectiles, proj);
        }
        else
        {
            Deactivate(projectiles, proj);
        }
    }

    private void Deactivate(List<TypeProjectile> typeProjs, TypeProjectile proj)
    {
        for (int i = 0; i < typeProjs.Count; i++)
        {
            //Debug.Log(typeProjs[i].Index + " " + proj.Index);
            if (typeProjs[i].Active && typeProjs[i].Index == proj.Index)
            {
                Debug.LogWarning("match!");
                typeProjs[i].SetActive(false);
                typeProjs[i].Projectile.SetActive(false);

            }
        }
    }
}
