using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TransformConverter : MonoBehaviour
{
    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;

    void Awake()
    {
        //defaultWorld = World.DefaultGameObjectInjectionWorld;
        //entityManager = defaultWorld.EntityManager;

        //GameObjectConversionSettings settings = 
        //    GameObjectConversionSettings.FromWorld(defaultWorld, null);
        //entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(
        //    gameObject, settings);
        //InstantiateEntity(transform.position);
    }
    private void InstantiateEntity(float3 position)
    {
        if (entityManager == null)
        {
            return;
        }

        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }
}
