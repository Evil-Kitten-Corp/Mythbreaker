using UnityEngine;

public class ComboResetter : StateMachineBehaviour
{
    private ComboUI _comboUI;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _comboUI = FindObjectOfType<ComboUI>();
        var ac = FindObjectOfType<WeaponAndAttackManager>();

        _comboUI.ResetCombo();
        ac.SetEndCombo();
    }
}
