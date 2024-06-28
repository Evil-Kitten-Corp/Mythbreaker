using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Navmesh/Seek")]
    public class MoveNavmeshAgent : Leaf
    {
        public TransformReference destination;
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
            agent.SetDestination(destination.Value.position);

			if (animator.Value != null) 
			{
				animator.Value.SetBool(animatorWalkBool, true);
			}
        }
        
        public override NodeResult Execute()
        {
            time += Time.deltaTime;
            // Update destination every given interval
            if (time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                agent.SetDestination(destination.Value.position);
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
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(destination.isInvalid || agent == null);
        }
    }

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

        public override void OnEnter()
        {
            time = 0;
            agent.isStopped = false; 

			UpdateDestination();

			if (animator.Value != null) 
			{
				animator.Value.SetBool(animatorWalkBool, true);
			}
        }

		private void UpdateDestination()
        {
            Vector3 targetDir = targetTransform.Value.position - agent.transform.position;

			float relativeHeading = Vector3.Angle(agent.transform.forward, agent.transform.TransformVector(targetTransform.Value.forward));

			float toTarget = Vector3.Angle(agent.transform.forward, agent.transform.TransformVector(targetDir));

			if ((toTarget > 90 && relativeHeading < 20) || targetTransform.Value.GetComponent<NavMeshAgent>().speed < 0.01f) 
			{	
				agent.SetDestination(targetTransform.Value.position);
				return;
			}

            float lookAhead = targetDir.magnitude / (agent.speed + targetTransform.Value.GetComponent<NavMeshAgent>().speed);
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
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(targetTransform.isInvalid || agent == null);
        }
    }
}
