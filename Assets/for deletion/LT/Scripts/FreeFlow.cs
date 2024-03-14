using UnityEngine;

public class FreeFlow : StateMachineBehaviour
{
	[SerializeField]
	private Character owner;

	[SerializeField]
	public EFlowType flowType;

	public bool isUpdated = true;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		owner = animator.gameObject.GetComponent<Character>();
		if ((bool)owner.GetComponent<FreeFlowComponent>())
		{
			if (!isUpdated)
			{
				owner.GetComponent<FreeFlowComponent>().OnFreeFlow();
			}
			else
			{
				owner.GetComponent<FreeFlowComponent>().UpdateTarget();
			}
		}
		else
		{
			Debug.LogError("Free Flow Component is null.");
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if ((bool)owner.GetComponent<FreeFlowComponent>() && isUpdated)
		{
			owner.GetComponent<FreeFlowComponent>().UpdateFreeFlow(flowType);
		}
		else
		{
			Debug.LogError("Free Flow Component is null.");
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if ((bool)owner.GetComponent<FreeFlowComponent>() && isUpdated)
		{
			owner.GetComponent<FreeFlowComponent>().OnReset();
		}
	}
}
