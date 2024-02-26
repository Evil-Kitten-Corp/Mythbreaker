using UnityEngine;

namespace Characters.Interfaces
{
    public interface ITarget
    {
        bool Targetable { get; }
        GameObject GetGameObject();
    }
}