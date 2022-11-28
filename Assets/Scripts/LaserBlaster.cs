using Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootBullet(GameObject ship, GameObject bulletPrefab, Vector3 muzzlePosition)
    {
        // Add offset so the bullet spawns with correct rotation
        Quaternion newRotation = Quaternion.AngleAxis(90, Vector3.forward) * ship.transform.rotation;
        GameObject bulletSpawn = Instantiate(bulletPrefab, muzzlePosition, newRotation);
        Projectile proj = bulletSpawn.GetComponent<Projectile>();
        proj.ship = ship;
        proj.aimable = true;
    }
}