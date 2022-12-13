using Unity.Entities;
using static UnityEngine.GraphicsBuffer;
using UnityEngine;

// Authoring MonoBehaviours are regular GameObject components.
// They constitute the inputs for the baking systems which generates ECS data.
class BulletAuthoring : MonoBehaviour
{
}

// Bakers convert authoring MonoBehaviours into entities and components.
//class BulletBaker : Baker<BulletAuthoring>
//{
//    public override void Bake(BulletAuthoring authoring)
//    {
//        AddComponent<Bullet>();
//    }
//}
