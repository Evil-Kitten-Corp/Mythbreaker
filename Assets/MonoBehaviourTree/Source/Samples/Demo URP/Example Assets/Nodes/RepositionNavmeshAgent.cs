using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Navmesh/Reposition")]
    public class RepositionNavmeshAgent : Leaf
    {
        public TransformReference awayFrom;
        public TransformReference source;
        public AnimatorReference animator;
        public string animatorWalkBool;
        public NavMeshAgent agent;
        public float stopDistance = 2f;

        [Tooltip("How often target position should be updated")]
        public float updateInterval = 1f;

        private float time = 0;

        public override void OnEnter()
        {
            time = 0;
            agent.isStopped = false;

            CalculateAwayDirection();

            if (animator.Value != null)
            {
                animator.Value.SetBool(animatorWalkBool, true);
            }
        }

        private void CalculateAwayDirection()
        {
            /*float distanceToPlayer = Vector3.Distance(source.Value.position, awayFrom.Value.position);
            float randRadius = Random.Range(-3, 3);

            Vector3 randDir = Random.insideUnitSphere * randRadius;
            randDir += source.Value.position;

            NavMesh.SamplePosition(randDir, out var navHit, randRadius, -1);

            while (Vector3.Distance(navHit.position, awayFrom.Value.position) > distanceToPlayer)
            {
                distanceToPlayer = Vector3.Distance(source.Value.position, awayFrom.Value.position);

                randRadius = Random.Range(-3, 3);
                randDir = Random.insideUnitSphere * randRadius;
                randDir += transform.position;

                NavMesh.SamplePosition(randDir, out navHit, randRadius, -1);
            }*/

            agent.SetDestination(awayFrom.Value.forward * 2);
        }

        public override NodeResult Execute()
        {
            time += Time.deltaTime;
            // Update destination every given interval
            if (time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                CalculateAwayDirection();
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
            //
            agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(awayFrom.isInvalid || agent == null || source.isInvalid);
        }
    }
}