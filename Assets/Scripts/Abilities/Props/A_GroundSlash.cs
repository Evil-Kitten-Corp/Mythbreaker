using System.Collections;
using Base;
using UnityEngine;

namespace Abilities.Props
{
    public class A_GroundSlash : MonoBehaviour, IAbilityProp
    {
        public float speed = 30;
        public float slowDownRate = 0.01f;
        public float detectingDistance = 0.1f;
        public float destroyDelay = 5;

        private Rigidbody _rb;
        private bool _stopped;
        
        void Start()
        {
            var position = transform.position;
            
            position = new Vector3(position.x, 0, position.z);
            
            transform.position = position;

            if (GetComponent<Rigidbody>() != null)
            {
                _rb = GetComponent<Rigidbody>();
                StartCoroutine(SlowDown());
            }

            Destroy(gameObject, destroyDelay);
        }

        private void FixedUpdate()
        {
            if (!_stopped)
            {
                var position = transform.position;
                
                Vector3 distance = new Vector3(position.x, position.y + 1, position.z);
                
                transform.position = Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), 
                    out var hit, detectingDistance) ? new Vector3(position.x, hit.point.y, position.z) 
                    : new Vector3(position.x, 0, position.z);
                
                Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingDistance), Color.red);
            }
        }

        private IEnumerator SlowDown()
        {
            float t = 1;
            
            while (t > 0)
            {
                _rb.velocity = Vector3.Lerp(Vector3.zero, _rb.velocity, t);
                t -= slowDownRate;
                yield return new WaitForSeconds(0.1f);
            }

            _stopped = true;
        }
    }
}