using Unity.Entities;

[GenerateAuthoringComponent]
public struct Movement : IComponentData
{
    public int MovementSpeed;
}
