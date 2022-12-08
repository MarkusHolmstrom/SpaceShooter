using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private int expansionSpeed = 5;
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
            float exp = expansionSpeed * Time.deltaTime;
            explTransform.localScale += new Vector3(exp, exp, exp);
            lifeTime += Time.deltaTime;
        }
        else
        {
            explTransform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}
