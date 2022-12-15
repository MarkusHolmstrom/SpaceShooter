using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class TransformConverter : MonoBehaviour//, IConvertGameObjectToEntity
{
    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;
    private Entity myEntity;

    void Awake()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        GameObjectConversionSettings settings =
            GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(
            gameObject, settings);
        //InstantiateEntity(transform.position);
    }
    public void InstantiateEntity(float3 position)
    {
        if (entityManager == null)
        {
            return;
        }

        myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
        entityManager.AddComponentObject(myEntity, GetComponent<Transform>());
    }

    public void RemoveEntities()
    {
        entityManager.DestroyEntity(myEntity);
    }

    //public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    //{
    //    dstManager.AddComponentObject(entity, GetComponent<Transform>());
    //}
}
