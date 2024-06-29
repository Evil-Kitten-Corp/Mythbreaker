using Enemy.Refactored.Utilities;
using FinalScripts.Refactored;
using Invector.vCharacterController;
using MBT;
using TriInspector;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Navmesh/Pursuit")]
    public class NavmeshPursuit : Leaf
    {
        public TransformReference targetTransform;

        public AnimatorReference animator;
        public string animatorWalkBool;

        public NavMeshAgent agent;
        public float stopDistance = 2f;

        [Tooltip("How often target position should be updated")]
        public float updateInterval = 1f;
        private float time = 0;

        public bool registerAsFollower;
        [ShowIf(nameof(registerAsFollower))] public MeleeEnemyBT meleeBT;

        private TargetDistributor _distributor;

        public override void OnEnter()
        {
            time = 0;
            agent.isStopped = false; 

            UpdateDestination();

            if (animator.Value != null) 
            {
                animator.Value.SetBool(animatorWalkBool, true);
            }

            if (registerAsFollower && meleeBT != null)
            {
                _distributor = targetTransform.Value.GetComponentInChildren<TargetDistributor>();
                    
                if (_distributor != null)
                {
                    meleeBT.SetFollowerInstance(_distributor.RegisterNewFollower());
                }
            }
        }

        private void UpdateDestination()
        {
            Vector3 targetDir = targetTransform.Value.position - agent.transform.position;

            float relativeHeading = Vector3.Angle(agent.transform.forward, agent.transform.TransformVector(targetTransform.Value.forward));

            float toTarget = Vector3.Angle(agent.transform.forward, agent.transform.TransformVector(targetDir));

            if ((toTarget > 90 && relativeHeading < 20) || targetTransform.Value.GetComponent<vThirdPersonController>().freeSpeed.movementSmooth < 0.01f) 
            {	
                agent.SetDestination(targetTransform.Value.position);
                return;
            }

            float lookAhead = targetDir.magnitude / (agent.speed + targetTransform.Value.GetComponent<vThirdPersonController>().freeSpeed.movementSmooth);
            agent.SetDestination(targetTransform.Value.position + targetTransform.Value.forward * lookAhead * 3);
        }
        
        public override NodeResult Execute()
        {
            time += Time.deltaTime;
            // Update destination every given interval
            if (time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                UpdateDestination();
            }
            // Check if path is ready
            if (agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if (agent.remainingDistance < stopDistance)
            {
                return NodeResult.success;
            }
            // Check if there is any path (if not pending, it should be set)
            if (agent.hasPath)
            {
                return NodeResult.running;
            }
            // By default return failure
            return NodeResult.failure;
        }

        public override void OnExit()
        {
            agent.isStopped = true;

            if (animator.Value != null) 
            {
                animator.Value.SetBool(animatorWalkBool, false);
            }

            if (registerAsFollower)
            {
                meleeBT.UnregisterFollower();
            }
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(targetTransform.isInvalid || agent == null);
        }
    }
}