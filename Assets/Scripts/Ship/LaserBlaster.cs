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

    public void ShootBullet(GameObject ship, bool enemy, Vector3 muzzlePosition)
    {
        // Add offset so the bullet spawns with correct rotation
        Quaternion newRotation = Quaternion.AngleAxis(0, Vector3.forward) * ship.transform.rotation;

        GameObject bullet = projManager.GetPrefab(enemy);
        if (bullet == null)
        {
            Debug.LogError("Error: ship cant find new bullet: " + transform.parent.gameObject.name);
            return;
        }
        // some necessary values for the bullets
        bullet.GetComponent<TransformConverter>().InstantiateEntity(muzzlePosition);
        bullet.transform.SetPositionAndRotation(muzzlePosition, newRotation);

        bullet.SetActive(true);
        Projectile proj = bullet.GetComponent<Projectile>();
        proj.ship = ship;
        proj.enemy = enemy;
    }
}
