using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TransformConverter : MonoBehaviour //, IConvertGameObjectToEntity
{
    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;

    void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        GameObjectConversionSettings settings = 
            GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(
            gameObject, settings);
        InstantiateEntity(float3.zero);
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
    // Update is called once per frame
    void Update()
    {
        
    }

    //public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    //{
    //    dstManager.AddComponentObject(entity, GetComponent<Transform>());
    //}
}
