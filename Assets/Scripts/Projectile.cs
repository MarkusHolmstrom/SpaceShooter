using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject ship;
    [SerializeField]
    private GameObject explosionPrefab;
    // aim adjustment
    public bool aimable = false;

    public bool enemy = false;

    public ProjectileManager projManager;

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
    void Start()
    {
        bulletTransform = transform;
        //TODO figure otu why this is needed!!
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        projManager = gameManager.GetComponent<ProjectileManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
                Debug.Log("this bullet was a miss!");
                projManager.DecreaseActiveQuantity(enemy, this.gameObject);
            }
            else
            {
                Debug.LogError("Error: couldnt find projectile manaager in this bullet!!");
            }
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
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        Instantiate(explosionPrefab, position, rotation);
        
        if (projManager != null)
        {
            projManager.DecreaseActiveQuantity(enemy, this.gameObject);
        }
        else
        {
            Debug.LogError("Error: couldnt find projectile manaager in this bullet!!");
        }
        //Destroy(gameObject);
    }
}
