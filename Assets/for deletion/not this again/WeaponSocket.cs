using UnityEngine;

namespace not_this_again
{
    public class WeaponSocket: MonoBehaviour
    {
        public SocketPosition socketSlot;
        public Transform socketTransform;

        public void Attach(GameObject obj)
        {
            obj.transform.SetParent(socketTransform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
    }
}