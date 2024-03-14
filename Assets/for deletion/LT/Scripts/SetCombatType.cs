using UnityEngine;

public class SetCombatType : StateMachineBehaviour
{
	private Character character;

	[SerializeField]
	private ECombatType enterCombatType;

	[SerializeField]
	private ECombatType exitCombatType;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		character = animator.GetComponent<Character>();
		character.CombatData.CombatType = enterCombatType;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		character.CombatData.CombatType = exitCombatType;
	}
}
