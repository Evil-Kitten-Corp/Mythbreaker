using DG.Tweening;
using UnityEngine;

public class Jump : StateMachineBehaviour
{
	private Character Character;

	public AnimationCurve JumpCurve;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Character = animator.GetComponent<Character>();
		Character.LocomotionData.IsJump = true;
		Character.LocomotionData.IsGrounded = false;
		Character.CharacterAnim.SetBool("IsGrounded", false);
		Character.LocomotionData.MovementState = eMovementState.InAir;
		Character.FallingEvent();
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Character.AirControl(JumpCurve.Evaluate(stateInfo.normalizedTime) * Character.LocomotionData.JumpForce);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Character.LocomotionData.IsJump = false;
		Character.GetComponent<CharacterMovement>().AirMoveDirection = Vector3.zero;
		Character.transform.DORotate(new Vector3(0f, Character.transform.eulerAngles.y, 0f), 0.2f);
	}
}
