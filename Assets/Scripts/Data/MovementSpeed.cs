
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Movement : IComponentData
{
    public int MovementSpeed;
}

// https://github.com/UnityGameAcademy/DOTSComponentsAndSystems/tree/master/Assets/Scripts
