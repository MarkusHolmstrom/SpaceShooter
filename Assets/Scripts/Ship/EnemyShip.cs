using Factory;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class EnemyShip : MonoBehaviour, IShip
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
    private GameObject explosionPrefab;

    private float reloadTime;

    private bool reloading = true;

    // enemy AI:
    [SerializeField]
    private float shipSpeed = 3.0f;

    [SerializeField]
    private int yMax = 14;
    [SerializeField]
    private int zMax = 22;

    [SerializeField]
    private float speedFoundZone = 0.5f;
    private bool foundZone = false;

    [SerializeField]
    private int Health = 4;

    private GameObject player;
    private EnemyManager enemyManager;
    private GameObject gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        shipTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        reloadTime = LCGRandomGenerator.RandomLCGfloat(-2.0f, RELOAD_TIMER);
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        enemyManager = gameManager.GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foundZone = IsWithinScreenZone();
        UpdateMovement();
        UpdateRotation();
        ShootCycle();
    }

    public void UpdateMovement()
    {
        if (foundZone)
        {
            shipTransform.Translate(speedFoundZone * Time.deltaTime * Vector3.forward);
            //shipTransform.position += new Vector3(0, 0, speedFoundZone * Time.deltaTime);
            //shipTransform.position += Vector3.forward * speedFoundZone * Time.deltaTime;
        }
        else
        {
            shipTransform.Translate(shipSpeed * Time.deltaTime * Vector3.forward);
            //shipTransform.position += new Vector3(0, 0, shipSpeed * Time.deltaTime);
            //shipTransform.position += Vector3.forward * shipSpeed * Time.deltaTime;
        }
    }

    private bool IsWithinScreenZone()
    {
        if (shipTransform.position.y < -yMax)
        {
            return false;
        }
        else if (shipTransform.position.y > yMax)
        {
            return false;
        }
        if (shipTransform.position.z < -zMax)
        {
            return false;
        }
        else if (shipTransform.position.z > zMax)
        {
            return false;
        }
        return true;
    }

    public void UpdateRotation()
    {
        shipTransform.LookAt(player.transform.position, Vector3.forward);
    }

    public void ShootCycle()
    {
        if (!foundZone)
        {
            return;
        }
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
        if (Health <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        GameObject expl = VFXManager.SharedInstance.ActivateExplosion(false);
        expl.transform.position = shipTransform.position;
        gameObject.SetActive(false);
        enemyManager.UpdateEnemyLists(this.gameObject);
    }
}
