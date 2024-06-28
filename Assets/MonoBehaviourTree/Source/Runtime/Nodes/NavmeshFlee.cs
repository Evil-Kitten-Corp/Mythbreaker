﻿using UnityEngine;
using UnityEngine.AI;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Navmesh/Flee")]
    public class NavmeshFlee : Leaf
    {
        public TransformReference targetTransform;
        public NavMeshAgent agent;
        public float stopWhenDistance = 2f;
        
        [Tooltip("How often target position should be updated")]
        public float updateInterval = 1f;
        
        public AnimatorReference animator;
        public string animatorWalkBool;
        
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
            Vector3 fleeVector = targetTransform.Value.position- agent.transform.position;
            Vector3 newPos = agent.transform.position - fleeVector;
            agent.SetDestination(newPos);
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
            if (Vector3.Distance(agent.transform.position, targetTransform.Value.position) >= stopWhenDistance)
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