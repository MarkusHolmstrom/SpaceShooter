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
            Destroy(this.gameObject);
        }
        bulletTransform.Translate(bulletSpeed * Time.deltaTime * Vector3.up);
        if (aimable && lifeTime < aimTimer)
        {
            lifeTime += Time.deltaTime;
            // offset needed here aswell
            Quaternion newRotation = Quaternion.AngleAxis(90, Vector3.forward) * ship.transform.rotation;
            bulletTransform.rotation = newRotation;
        }
        CheckIfOutOfBounds();
    }

    private void CheckIfOutOfBounds()
    {
        if (bulletTransform.position.x >= xMax || bulletTransform.position.y >= yMax ||
            bulletTransform.position.x <= -xMax || bulletTransform.position.y <= -yMax)
        {
            if (projManager != null)
            {
                projManager.DeActivateProjectile(enemy, projectile);
            }
            else
            {
                Debug.LogError("Error: couldnt find projectile manaager in this bullet!!");
            }
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerShip ps = collision.gameObject.GetComponent<PlayerShip>();
            ps.DoDamage(damage);
        }
        else if (collision.gameObject.tag == "AIShip")
        {
            EnemyShip ps = collision.gameObject.GetComponent<EnemyShip>();
            ps.DoDamage(damage);
        }
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;
        OnCollision(position);
    }

    private void OnCollision(Vector3 position)
    {
        GameObject expl = VFXManager.SharedInstance.ActivateExplosion(true);
        expl.transform.position = position;
        //Instantiate(explosionPrefab, position, Quaternion.identity);
        if (projectile == null)
        {
            gameObject.SetActive(false);
            //TODO this is a temp thing, needs to call deactiveprojctiels aswell
            return;
        }
        if (projManager != null)
        {
            projManager.DeActivateProjectile(enemy, projectile);
        }
        else
        {
            Debug.LogError("Error: couldnt find projectile manaager in this bullet!!");
        }
    }
}
