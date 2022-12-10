using Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlaster : MonoBehaviour
{
    [SerializeField]
    private ProjectileManager projManager;

    private GameObject projManagerGO;

    // Start is called before the first frame update
    void Awake()
    {
        projManagerGO = GameObject.FindGameObjectWithTag("ProjectileManager");
        projManager = projManagerGO.GetComponent<ProjectileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShootBullet(GameObject ship, bool enemy, Vector3 muzzlePosition)
    {
        // Add offset so the bullet spawns with correct rotation
        Quaternion newRotation = Quaternion.AngleAxis(0, Vector3.forward) * ship.transform.rotation;
        GameObject bullet = Instantiate(projManager.GetPrefab(enemy), muzzlePosition, newRotation);
        bullet.SetActive(true);
        Projectile proj = bullet.GetComponent<Projectile>();
        proj.ship = ship;
        proj.aimable = true;
        proj.enemy = enemy;
    }
}
