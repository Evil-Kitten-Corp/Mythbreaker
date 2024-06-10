using UnityEngine;

public class ChangeWeaponState : StateMachineBehaviour
{
    private WeaponAndAttackManager _attackControl;
    public Option stateToBe;

    public enum Option
    {
        Enable,
        Disable
    }
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackControl = animator.GetComponent<WeaponAndAttackManager>();

        if (stateToBe == Option.Enable)
        {
            _attackControl.EnableWeapon();
        }
        else
        {
            _attackControl.DisableWeapon();
        }
    }
}
