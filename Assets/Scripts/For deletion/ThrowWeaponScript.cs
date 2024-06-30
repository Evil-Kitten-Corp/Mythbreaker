using FinalScripts;
using FinalScripts.Refactored;
using UnityEngine;

public class ThrowWeaponScript: MonoBehaviour
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
        if (activated && other.TryGetComponent<EnemyBT>(out var bt))
        {
            bt.TakeDamage(30 + FindObjectOfType<WeaponAndAttackManager>().baseAttack, true);
        }
        else if (activated && other.TryGetComponent<EnemyAppendage>(out var ea))
        {
            ea.TakeDamage(30 + FindObjectOfType<WeaponAndAttackManager>().baseAttack);
        }
    }
}