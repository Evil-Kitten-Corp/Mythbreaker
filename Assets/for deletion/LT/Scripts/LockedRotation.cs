using UnityEngine;

public class LockedRotation : StateMachineBehaviour
{
	private Character owner;

	public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		owner = animator.GetComponent<Character>();
		owner.LocomotionData.LockedRotation = true;
	}

	public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		owner.LocomotionData.LockedRotation = false;
	}
}
