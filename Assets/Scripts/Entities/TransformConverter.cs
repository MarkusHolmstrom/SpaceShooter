using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TransformConverter : MonoBehaviour
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

}
