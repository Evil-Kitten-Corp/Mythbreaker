using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.SMBs
{
    public class MeleeSMBIdle: SceneLinkedSMB<MeleeEnemy>
    {
        public float minimumIdleGruntTime = 2.0f;
        public float maximumIdleGruntTime = 5.0f;
        private float _remainingToNextGrunt;

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (minimumIdleGruntTime > maximumIdleGruntTime)
                minimumIdleGruntTime = maximumIdleGruntTime;

            _remainingToNextGrunt = Random.Range(minimumIdleGruntTime, maximumIdleGruntTime);
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

            _remainingToNextGrunt -= Time.deltaTime;

            if (_remainingToNextGrunt < 0)
            {
                _remainingToNextGrunt = Random.Range(minimumIdleGruntTime, maximumIdleGruntTime);
                MonoBehaviour.Grunt();
            }

            MonoBehaviour.FindTarget();
            
            if (MonoBehaviour.Target != null)
            {
                MonoBehaviour.StartPursuit();
            }
        }
    }
}