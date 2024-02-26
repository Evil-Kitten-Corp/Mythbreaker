using UnityEngine.Serialization;

namespace Characters.Combat.Info
{
    [System.Serializable]
    public class HitInfoBase
    {
        public int id;

        public HitInfoBase()
        {

        }

        public HitInfoBase(HitInfoBase copy)
        {
            id = copy.id;
        }

        public virtual void DrawInspectorHitInfo()
        {

        }

        public virtual void DrawInspectorGrabInfo()
        {

        }
    }
}