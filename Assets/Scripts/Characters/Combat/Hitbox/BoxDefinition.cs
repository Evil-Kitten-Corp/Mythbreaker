using Vector3 = System.Numerics.Vector3;

namespace Characters.Combat.Hitbox
{
    /// <summary>
    /// A class that defines a hitbox's shape, offset, size, rotation and radius.
    /// </summary>
    [System.Serializable]
    public class BoxDefinition : BoxDefinitionBase
    {
        public Vector3 Offset;
        public Vector3 Size;
        public Vector3 Rotation;
        public float radius;
        public float height;

        public BoxDefinition()
        {

        }

        public BoxDefinition(BoxDefinition other)
        {
            shape = other.shape;
            Offset = other.Offset;
            Size = other.Size;
            Rotation = other.Rotation;
            radius = other.radius;
            height = other.height;
        }
    }
}