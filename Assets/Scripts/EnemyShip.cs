using Factory;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

struct ScreenZone
{
    public int xMax;
    public int yMax;
    public int xMin;
    public int yMin;
}

public class EnemyShip : MonoBehaviour
{
    [SerializeField]
    private LaserBlaster laserBlaster;
    // cache the transform, saves perf in update etc
    private Transform shipTransform;
    [SerializeField]
    private Transform muzzleLocation;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float RELOAD_TIMER = 2.0f;
    [SerializeField]
    private float reloadTime;

    private bool reloading = true;

    // enemy AI:
    [SerializeField]
    private float shipSpeed = 3.0f;

    // Screen zone as a target, struct instead?
    [SerializeField]
    private int xMax = 14;
    [SerializeField]
    private int yMax = 8;

    private bool foundZone = false;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int Health = 4;
    [SerializeField]
    private Transform RoofPart;

    // Start is called before the first frame update
    void Start()
    {
        shipTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        // =?
        reloadTime = Random.Range(-2.0f, RELOAD_TIMER);// LCGRandomGenerator.RandomLCGfloat(-2.0f, RELOAD_TIMER);
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
        foundZone = IsWithinScreenZone();
        if (foundZone)
        {
            shipTransform.Translate(shipSpeed * 0.1f * Time.deltaTime * Vector3.forward);
        }
        else
        {
            shipTransform.Translate(shipSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    private bool IsWithinScreenZone()
    {
        if (shipTransform.position.x < -xMax)
        {
            return false;
        }
        else if (shipTransform.position.x > xMax)
        {
            return false;
        }
        if (shipTransform.position.y < -yMax)
        {
            return false;
        }
        else if (shipTransform.position.y > yMax)
        {
            return false;
        }
        return true;
    }

    void UpdateRotation()
    {
        shipTransform.LookAt(Vector3.zero - player.transform.position, Vector3.up);
        
    }

    void ShootCycle()
    {
        if (!reloading && laserBlaster)
        {
            laserBlaster.ShootBullet(this.gameObject, true, muzzleLocation.position);
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
        }
    }

    public void DoDamage(int damage)
    {
        Health -= damage;
        //if (Health > 2)
        //{
        //    RoofPart.SetParent(null);
        //    Rigidbody rb = RoofPart.gameObject.AddComponent<Rigidbody>();
        //    rb.isKinematic = false;
        //    rb.useGravity = true;
        //}
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
