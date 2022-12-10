using Factory;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

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
    private GameObject explosionPrefab;

    private float reloadTime;

    private bool reloading = true;

    // enemy AI:
    [SerializeField]
    private float shipSpeed = 3.0f;

    // Screen zone as a target, struct instead?
    [SerializeField]
    private int xMax = 22;
    [SerializeField]
    private int yMax = 14;

    private bool foundZone = false;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int Health = 4;

    [SerializeField]
    private EnemyManager enemyManager;
    private GameObject gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        shipTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        // =?
        reloadTime = LCGRandomGenerator.RandomLCGfloat(-2.0f, RELOAD_TIMER);
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        enemyManager = gameManager.GetComponent<EnemyManager>();
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
            shipTransform.Translate(shipSpeed * 0.2f * Time.deltaTime * Vector3.forward);
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
        shipTransform.LookAt(player.transform.position, Vector3.up);
        
    }

    void ShootCycle()
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

    private void OnDeath()
    {
        GameObject expl = VFXManager.SharedInstance.ActivateExplosion(false);
        expl.transform.position = shipTransform.position;
        gameObject.SetActive(false);
        enemyManager.UpdateEnemyLists(this.gameObject);
    }
}
