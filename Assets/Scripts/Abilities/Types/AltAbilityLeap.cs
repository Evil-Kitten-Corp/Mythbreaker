using System.Collections;
using Abilities;
using DG.Tweening;
using UnityEngine;

namespace Base
{
    public class AltAbilityLeap : AltAbilityBase
    {
        private AbilityPreviewer _abilityPreviewer;
        private Vector3? _instPosition = null;
        
        public float leapHeight = 5f;
        public float leapDuration = 1f;
        public float landingDuration = 0.5f;
        public Ease easeType = Ease.OutQuad; 

        public override void Start()
        {
            _abilityPreviewer = player.GetComponent<AbilityPreviewer>();
            _abilityPreviewer.SubscribeToAbilityCast(AbilityCastComplete, this);
            base.Start();
        }

        protected override IEnumerator Activate()
        {
            while (_instPosition == null)
            {
                yield return new WaitForEndOfFrame();
            }
            
            player.playerAnimator.SetTrigger("BeginLeap");
            Vector3 leapDestination = (Vector3)_instPosition;
            leapDestination.y += leapHeight;

            // Leap to the destination position
            transform.DOMove(leapDestination, leapDuration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    player.playerAnimator.SetTrigger("EndLeap");
                    transform.DOMove((Vector3)_instPosition, landingDuration)
                        .SetEase(easeType);
                });

            yield return base.Activate();
        }

        private void AbilityCastComplete(Transform t)
        {
            _instPosition = t.position;
            Use();
        }
    }
}