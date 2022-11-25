using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject ship;
    // aim adjustment
    public bool aimable = false;
    [SerializeField]
    private float aimTimer = 0.07f;
    private float lifeTime = 0;
    [SerializeField]
    private int bulletSpeed = 10;

    private Transform bulletTransform;

    // Start is called before the first frame update
    void Start()
    {
        bulletTransform = transform;
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
        
    }
}
