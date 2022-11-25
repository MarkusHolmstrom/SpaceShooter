using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// https://www.youtube.com/watch?v=YxlY4_aj678 tut

public class PlayerShip : MonoBehaviour
{
    // cache the transform, saves perf in update etc
    [SerializeField]
    private Transform shipTransform;
    [SerializeField]
    private Transform muzzleLocation;

    [SerializeField]
    private int moveSpeed = 5;
    [SerializeField]
    private int YMAX = 8;
    [SerializeField]
    private int XMAX = 13;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float RELOAD_TIMER = 1.0f;
    [SerializeField]
    private float reloadTime = 0.0f;

    private bool reloading = false;

    Vector3 mousePos;
    Vector3 objectPos;


    // Start is called before the first frame update
    void Start()
    {
        shipTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateRotation();
        ShootCycle();
    }

    void UpdateMovement()
    {
        //shipTransform.Translate(Vector3.right * moveSpeed * 
        //    -Input.GetAxis("Horizontal") * Time.deltaTime);
        //shipTransform.Translate(Vector3.up * moveSpeed *
        //    Input.GetAxis("Vertical") * Time.deltaTime);

        shipTransform.position += new Vector3(
            moveSpeed *  -Input.GetAxis("Horizontal") * Time.deltaTime,
            moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 0 );

        // Constrains
        if (shipTransform.position.x < -XMAX)
        {
            shipTransform.position = 
                new Vector3(XMAX, shipTransform.position.y, shipTransform.position.z);
        }
        else if (shipTransform.position.x > XMAX)
        {
            shipTransform.position =
                new Vector3(-XMAX, shipTransform.position.y, shipTransform.position.z);
        }
        if (shipTransform.position.y < -YMAX)
        {
            shipTransform.position =
                new Vector3(shipTransform.position.x, YMAX, shipTransform.position.z);
        }
        else if (shipTransform.position.y > YMAX)
        {
            shipTransform.position =
                new Vector3(shipTransform.position.x, -YMAX, shipTransform.position.z);
        }

    }

    
    void UpdateRotation()
    {
        mousePos = Input.mousePosition;
        objectPos = Camera.main.WorldToScreenPoint(shipTransform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        shipTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }

    void ShootCycle()
    {
        if (!reloading && Input.GetKey(KeyCode.Mouse0))
        {
            ShootBullet();
            reloadTime = 0;
            reloading = true;
        }
        if (reloading && RELOAD_TIMER > reloadTime)
        {
            reloadTime += Time.deltaTime;
        }
        else if (reloading)
        {
            reloading = false;
            //Debug.Log("Done reloading, captain!");
        }
    }

    void ShootBullet()
    {
        // Add offset so the bullet spawns with correct rotation
        Quaternion newRotation = Quaternion.AngleAxis(90, Vector3.forward) * shipTransform.rotation;
        GameObject bulletSpawn = Instantiate(bullet, muzzleLocation.position, newRotation);
        Projectile proj = bulletSpawn.GetComponent<Projectile>();
        proj.ship = this.gameObject;
        proj.aimable = true;
    }
}
