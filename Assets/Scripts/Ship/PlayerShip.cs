using TMPro;
using UnityEngine;

public class PlayerShip : MonoBehaviour, IShip
{
    [SerializeField]
    private LaserBlaster laserBlaster;
    // cache the transform, saves perf in update etc
    private Transform shipTransform;
    [SerializeField]
    private Transform muzzleLocation;

    [SerializeField]
    private int moveSpeed = 5;
    [SerializeField]
    private int YMAX = 8;
    [SerializeField]
    private int ZMAX = 13;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float RELOAD_TIMER = 1.0f;
    [SerializeField]
    private float reloadTime = 0.0f;

    private bool reloading = false;

    Vector3 mousePos;
    Vector3 objectPos;

    [SerializeField]
    private int Health = 10;

    [SerializeField]
    private TMP_Text progressText;

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

    public void UpdateMovement()
    {
        shipTransform.position += new Vector3( 0 ,
            moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime,
            moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);

        // Constrains, moves the ship to opposite side of screen when passes YMax or ZMax
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

        if (shipTransform.position.z < -ZMAX)
        {
            shipTransform.position =
                new Vector3(shipTransform.position.x, shipTransform.position.y, ZMAX);
        }
        else if (shipTransform.position.z > ZMAX)
        {
            shipTransform.position =
                new Vector3(shipTransform.position.x, shipTransform.position.y, -ZMAX);
        }
    }

    public void UpdateRotation()
    {
        mousePos = Input.mousePosition;
        objectPos = Camera.main.WorldToScreenPoint(shipTransform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        shipTransform.rotation = Quaternion.Euler(new Vector3(-angle, 0, 0));
    }

    public void ShootCycle()
    {
        if (!reloading && Input.GetKey(KeyCode.Mouse0) && laserBlaster)
        {
            laserBlaster.ShootBullet(this.gameObject, false, muzzleLocation.position);
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
        if (progressText != null)
        {
            progressText.text = "Player is dead! NOOOOO stop playing! youre dead, you here me? STop!!! STOP this MADNESS now GOD Dammit!!!";
        }
    }

}
