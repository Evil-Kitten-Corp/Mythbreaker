using UnityEngine;

public class TWeaponScript: MonoBehaviour
{
    public bool activated;

    public float rotationSpeed;

    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        {
            print(collision.gameObject.name);
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            GetComponent<Rigidbody>().isKinematic = true;
            activated = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && activated)
        {
            if (other.GetComponent<AttributesManager>() != null)
            {
                other.GetComponent<AttributesManager>().TakeDamage(100);
            }
        }
    }
}