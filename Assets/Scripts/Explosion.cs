using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float lifeExpancy = 1.5f;
    private float lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime < lifeExpancy)
        {
            lifeTime += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
