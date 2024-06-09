using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class HomingBullet : Bullet
    {
        public AnimationCurve positionCurve;
        public AnimationCurve noiseCurve;
        public float yOffset = 1f;
        public Vector2 minNoise = new(-3f, -0.25f);
        public Vector2 maxNoise = new(3f, 1);

        private Coroutine _homingCoroutine;

        public override void Spawn(Vector3 forward, int damage, Transform Target)
        {
            this.damage = damage;
            this.Target = Target;

            if (_homingCoroutine != null)
            {
                StopCoroutine(_homingCoroutine);
            }

            _homingCoroutine = StartCoroutine(FindTarget());
        }

        private IEnumerator FindTarget()
        {
            Vector3 startPosition = transform.position;
        
            Vector2 noise = new Vector2(Random.Range(minNoise.x, maxNoise.x), Random.Range(minNoise.y, maxNoise.y));
        
            Vector3 bulletDirectionVector = new Vector3(Target.position.x, Target.position.y + yOffset, 
                Target.position.z) - startPosition;
        
            Vector3 horizontalNoiseVector = Vector3.Cross(bulletDirectionVector, Vector3.up).normalized;
        
            float time = 0;

            while (time < 1)
            {
                float noisePosition = noiseCurve.Evaluate(time);
                transform.position = Vector3.Lerp(startPosition, Target.position + new Vector3(0, yOffset, 0), 
                    positionCurve.Evaluate(time)) + new Vector3(horizontalNoiseVector.x * noisePosition * noise.x, 
                    noisePosition * noise.y, noisePosition * horizontalNoiseVector.z * noise.x);
                transform.LookAt(Target.position + new Vector3(0, yOffset, 0));

                time += Time.deltaTime * moveSpeed;

                yield return null;
            }
        }
    }
}