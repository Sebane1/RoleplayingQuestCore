using System.Numerics;

namespace RoleplayingQuestCore
{
    public interface IQuestGameObject
    {
        int TerritoryId { get; }
        string Name { get; }
        Vector3 Position { get; }
        Vector3 Rotation { get; }
    }
}
