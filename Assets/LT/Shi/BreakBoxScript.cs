using UnityEngine;

namespace LT.Shi
{
    public class BreakBoxScript : MonoBehaviour
    {
        public GameObject brokenBox;

        public void Break()
        {
            GameObject broken = Instantiate(brokenBox, transform.position, transform.rotation);
            Rigidbody[] rbs = broken.GetComponentsInChildren<Rigidbody>();
            
            foreach (Rigidbody rb in rbs)
            {
                rb.AddExplosionForce(150, transform.position, 30);
            }
            
            Destroy(gameObject);
        }
    }
}