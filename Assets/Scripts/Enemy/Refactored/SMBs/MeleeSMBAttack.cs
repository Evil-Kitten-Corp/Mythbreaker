using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.SMBs
{
    public class MeleeSMBAttack: SceneLinkedSMB<MeleeEnemy>
    {
        private Vector3 _attackPosition;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);

            MonoBehaviour.Controller.SetFollowNavmeshAgent(false);

            _attackPosition = MonoBehaviour.Target.transform.position;
            Vector3 toTarget = _attackPosition - MonoBehaviour.transform.position;
            toTarget.y = 0;

            MonoBehaviour.transform.forward = toTarget.normalized;
            MonoBehaviour.Controller.SetForward(MonoBehaviour.transform.forward);

            if (MonoBehaviour.attackAudio != null)
            {
                MonoBehaviour.attackAudio.PlayRandomClip();
            }
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);

            if (MonoBehaviour.attackAudio != null)
            {
                MonoBehaviour.attackAudio.AudioSource.Stop();
            }

            MonoBehaviour.Controller.SetFollowNavmeshAgent(true);
        }
    }
}