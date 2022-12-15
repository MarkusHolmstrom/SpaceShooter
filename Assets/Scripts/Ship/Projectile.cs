using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    public GameObject ship;
    [SerializeField]
    private GameObject explosionPrefab;
    // aim adjustment
    public bool aimable = false;

    public bool enemy = false;

    public ProjectileManager projManager;

    public TypeProjectile projectile;

    [SerializeField]
    private float aimTimer = 0.05f;
    private float lifeTime = 0;
    [SerializeField]
    private int bulletSpeed = 10;
    [SerializeField]
    private int damage = 1;
    
    [SerializeField]
    private int xMax = 30;
    [SerializeField]
    private int yMax = 20;

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
        //TODO figure otu why this is needed!!
        if (this.gameObject.activeInHierarchy && ship == null)
        {
            Debug.LogError("Error: unvalid projectile has spawned!!");
            Destroy(gameObject);
        }
        CheckIfOutOfBounds();
    }

    private void CheckIfOutOfBounds()
    {
        if (bulletTransform.position.x >= xMax || bulletTransform.position.y >= yMax ||
            bulletTransform.position.x <= -xMax || bulletTransform.position.y <= -yMax)
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

    //void OnCollisionEnter(Collision collision)
    //{
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    PlayerShip ps = collision.gameObject.GetComponent<PlayerShip>();
        //    ps.DoDamage(damage);
        //}
        //else if (collision.gameObject.tag == "AIShip")
        //{
        //    EnemyShip ps = collision.gameObject.GetComponent<EnemyShip>();
        //    ps.DoDamage(damage);
        //}
        //ContactPoint contact = collision.contacts[0];
        //Vector3 position = contact.point;
        //OnCollision(position);
    //}

}
