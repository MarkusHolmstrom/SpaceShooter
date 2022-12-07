using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float lifeExpancy = 1.5f;
    private float lifeTime = 0;

    private Transform explTransform;

    // Start is called before the first frame update
    void Awake()
    {
        explTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime < lifeExpancy)
        {
            explTransform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            lifeTime += Time.deltaTime;
        }
        else
        {
            explTransform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}
