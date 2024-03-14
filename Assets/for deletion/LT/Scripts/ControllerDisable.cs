using UnityEngine;

public class ControllerDisable : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		MonoSingleton<InputSystemManager>.instance.enabled = false;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		MonoSingleton<InputSystemManager>.instance.enabled = true;
	}
}
