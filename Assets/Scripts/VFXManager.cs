using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Unity.Burst;
//using Unity.Entities;
using UnityEngine.Jobs;
using Unity.Mathematics;

// https://docs.unity3d.com/ScriptReference/Unity.Jobs.IJob.html
// An example job which increments all the numbers of an array.

// another example with float3s:
// https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/DOTS_Guide/jobs_tutorial/README.md

[RequireComponent(typeof(ObjectPool))]
public class VFXManager : MonoBehaviour
{
    public static VFXManager SharedInstance;

    [SerializeField]
    private GameObject shipExplPrefab;
    [SerializeField]
    private GameObject projExplPrefab;
    [SerializeField]
    private int quantity = 100;
    private ObjectPool objectPool;
    private ObjectPool projObjectPool;

    private void Awake()
    {
        SharedInstance = this;
        objectPool = GetComponent<ObjectPool>();
        // Create a new list of gameobjects for the projectile explosions
        projObjectPool = gameObject.AddComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPrefabs(objectPool, quantity, shipExplPrefab);
        SpawnPrefabs(projObjectPool, quantity, projExplPrefab);
    }


    private void SpawnPrefabs(ObjectPool op, int quantity, GameObject explosionPrefab)
    {
        op.PoolObjects(quantity, explosionPrefab);
    }

    public GameObject ActivateExplosion(bool proj)
    {
        GameObject go;
        if (proj)
        {
            go = projObjectPool.GetPooledObject();
        }
        else
        {
            go = objectPool.GetPooledObject();
        }

        if (go != null)
        {
            go.SetActive(true);
            return go;
        }
        return null;
    }
}
