using UnityEngine;

namespace Combat
{
    public class EntityTargeter : MonoBehaviour
    {
        private GameObject _target;

        public GameObject Target
        {
            get { return _target; }
            set { _target = value; }
        }
    }
}