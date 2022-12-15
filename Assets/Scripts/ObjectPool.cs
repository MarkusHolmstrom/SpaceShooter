using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> pooledObjects;

    public void PoolObjects(int quantity, GameObject poolGO)
    {
        pooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < quantity; i++)
        {
            temp = Instantiate(poolGO);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}


