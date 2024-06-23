using Enemy.Refactored.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace FinalScripts.Refactored.SMBs
{
    public class MeleeSMBPursuit: SceneLinkedSMB<MeleeEnemy>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

            MonoBehaviour.FindTarget();

            if (MonoBehaviour.Controller.NavmeshAgent.pathStatus == NavMeshPathStatus.PathPartial 
                || MonoBehaviour.Controller.NavmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                MonoBehaviour.StopPursuit();
                return;
            }

            if (MonoBehaviour.Target == null)
            {
                MonoBehaviour.StopPursuit();
            }
            else
            {
                MonoBehaviour.RequestTargetPosition();

                Vector3 toTarget = MonoBehaviour.Target.transform.position - MonoBehaviour.transform.position;

                if (toTarget.sqrMagnitude < MonoBehaviour.AttackDistance * MonoBehaviour.AttackDistance)
                {
                    MonoBehaviour.TriggerAttack();
                }
                else if (MonoBehaviour.FollowerData.AssignedSlot != -1)
                {
                    Vector3 targetPoint = MonoBehaviour.Target.transform.position + 
                                          MonoBehaviour.FollowerData.Distributor.GetDirection(MonoBehaviour.FollowerData
                                              .AssignedSlot) * MonoBehaviour.AttackDistance * 0.9f;

                    MonoBehaviour.Controller.SetTarget(targetPoint);
                }
                else
                {
                    MonoBehaviour.StopPursuit();
                }
            }
        }
    }
}