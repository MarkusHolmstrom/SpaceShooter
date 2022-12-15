using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject ship;
    [SerializeField]
    private GameObject explosionPrefab;

    public bool enemy = false;

    public ProjectileManager projManager;

    [SerializeField]
    private int yMax = 30;
    [SerializeField]
    private int zMax = 50;

    private Transform bulletTransform;

    // Start is called before the first frame update
    void Awake()
    {
        bulletTransform = transform;
        GameObject projManagerGO = GameObject.FindGameObjectWithTag("ProjectileManager");
        projManager = projManagerGO.GetComponent<ProjectileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOutOfBounds();
    }

    private void CheckIfOutOfBounds()
    {
        if (bulletTransform.position.z >= zMax || bulletTransform.position.y >= yMax ||
            bulletTransform.position.z <= -zMax || bulletTransform.position.y <= -yMax)
        {
            Remove();
        }
    }

    public void RemoveAtCollision()
    {
        GameObject expl = VFXManager.SharedInstance.ActivateExplosion(true);
        expl.transform.position = bulletTransform.position;
        Remove();
    }

    private void Remove()
    {
        GetComponent<TransformConverter>().RemoveEntities();
        gameObject.SetActive(false);
        projManager.DeActivateProjectile(gameObject);
    }

}
