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
    private float reloadTime = 0.0f;

    private bool reloading = false;

    // enemy AI:
    [SerializeField]
    private float shipSpeed = 3.0f;

    // Screen zone as a target, struct instead?
    [SerializeField]
    private int xMax = 14;
    [SerializeField]
    private int yMax = 8;

    private bool foundZone = false;

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
        foundZone = IsWithinScreenZone();
        if (foundZone)
        {
            shipTransform.Translate(shipSpeed * 0.5f * Time.deltaTime * Vector3.up);
        }
        else
        {
            shipTransform.Translate(shipSpeed * Time.deltaTime * Vector3.up);
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
        if (foundZone)
        {
            
        }
        else
        {
            shipTransform.LookAt(Vector3.zero);
            //shipTransform.rotation = Quaternion.Euler(new Vector3(0, 0, GetAngleTowardsScreen(shipTransform.rotation.z)));
        }
        
    }

    //private float GetAngleTowardsScreen(float currentAngle)
    //{
    //    if (muzzleLocation.position.x < shipTransform.position.x)
    //    {

    //    }
    //}

    void ShootCycle()
    {
        if (!reloading && laserBlaster)
        {
            laserBlaster.ShootBullet(this.gameObject, bullet, muzzleLocation.position);
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
}
