using UnityEngine;

namespace FinalScripts.Refactored.Attacker
{
    public class Bullet : Projectile
    {
        public float projectileSpeed;
        public int damageAmount = 1;
        
        public float explosionRadius;
        public float explosionTimer;
        
        public ParticleSystem explosionVFX;
        public bool vfxOnGround;

        public RandomAudioPlayer explosionPlayer;
        public RandomAudioPlayer bouncePlayer;

        private float _sinceFired;

        private RangedAttacker _shooter;
        private Rigidbody _rb;
        private ParticleSystem _vfx;

        private static readonly Collider[] ExplosionHitCache = new Collider[32];
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.detectCollisions = false;

            _vfx = Instantiate(explosionVFX);
            _vfx.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _rb.isKinematic = true;
            _sinceFired = 0.0f;
        }

        private void FixedUpdate()
        {
            _sinceFired += Time.deltaTime;

            if (_sinceFired > 0.2f)
            {
                _rb.detectCollisions = true;
            }

            if (explosionTimer > 0 && _sinceFired > explosionTimer)
            {
                Explosion();
            }
        }

        public void Explosion()
        {
            if (explosionPlayer)
            {
                explosionPlayer.transform.SetParent(null);
                explosionPlayer.PlayRandomClip();
            }

            int count = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, ExplosionHitCache);

            for (int i = 0; i < count; ++i)
            {
                AttributesManager d = ExplosionHitCache[i].GetComponentInChildren<AttributesManager>();

                if (d != null)
                    d.TakeDamage(damageAmount);
            }

            Pool.Free(this);

            Vector3 playPosition = transform.position;
            Vector3 playNormal = Vector3.up;
            
            if (vfxOnGround)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out var hit, 100.0f))
                {
                    playPosition = hit.point + hit.normal * 0.1f;
                    playNormal = hit.normal;
                }
            }

            _vfx.gameObject.transform.position = playPosition;
            _vfx.gameObject.transform.up = playNormal;
            _vfx.time = 0.0f;
            _vfx.gameObject.SetActive(true);
            _vfx.Play(true);
        }

        public void OnCollisionEnter()
        {
            if (bouncePlayer != null)
            {
                bouncePlayer.PlayRandomClip();
            }
            
            if (explosionTimer < 0)
            {
                Explosion();
            }
        }

        private Vector3 GetVelocity(Vector3 target)
        {
            Vector3 velocity = Vector3.zero;
            Vector3 toTarget = target - transform.position;

            float gSquared = Physics.gravity.sqrMagnitude;
            float b = projectileSpeed * projectileSpeed + Vector3.Dot(toTarget, Physics.gravity);
            float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

            if (discriminant < 0)
            {
                velocity = toTarget;
                velocity.y = 0;
                velocity.Normalize();
                velocity.y = 0.7f;

                Debug.DrawRay(transform.position, velocity * 3.0f, Color.blue);

                velocity *= projectileSpeed;
                return velocity;
            }

            float discRoot = Mathf.Sqrt(discriminant);
            float T = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
            velocity = toTarget / T - Physics.gravity * T / 2f;

            return velocity;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
#endif
        
        public override void Shot(Vector3 target, RangedAttacker shooter)
        {
            _rb.isKinematic = false;

            _shooter = shooter;


            _rb.velocity = GetVelocity(target);
            _rb.AddRelativeTorque(Vector3.right * -5500.0f);

            _rb.detectCollisions = false;

            transform.forward = target - transform.position;
        }
    }
}