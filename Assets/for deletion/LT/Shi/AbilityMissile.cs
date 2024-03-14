using UnityEngine;

namespace LT.Shi
{
    public class AbilityMissile : MonoBehaviour
    {
        public GameObject missilePrefab;
        public Target target;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var go = Instantiate(missilePrefab, transform.position + Vector3.up + Vector3.forward,
                    transform.rotation);
                go.GetComponent<Missile>().SetTarget(target); 
            }
        }
    }
}