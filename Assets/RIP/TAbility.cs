using System.Collections;
using Invector.vCharacterController;
using UnityEngine;

public abstract class TAbility : MonoBehaviour
{
    [HideInInspector] public bool isActive;
    public bool canUseInAir;
    public AbilityCastBehaviour casting;
    public string animationTrigger;

    public bool CanUse => CanUseAbility();

    public virtual void Use(Vector3 v)
    {
        if (!CanUse)
        {
            return;
        }
        
        if (animationTrigger != null)
        {
            TriggerAnimation();
        }
    }

    public virtual void Use(TEnemy t)
    {
        if (!CanUse)
        {
            return;
        }
        
        if (animationTrigger != null)
        {
            TriggerAnimation();
        }
    }

    public virtual void Use()
    {
        if (!CanUse)
        {
            return;
        }
        
        if (animationTrigger != null)
        {
            TriggerAnimation();
        }
    }

    public virtual IEnumerator Use(bool isRoutine)
    {
        Use();
        yield break;
    }

    void TriggerAnimation()
    {
        GetComponentInParent<Animator>().SetTrigger(animationTrigger);
    }

    bool CanUseAbility()
    {
        if (!isActive)
        {
            return false;
        }
        
        if (!canUseInAir && !GetComponentInParent<vThirdPersonController>().isGrounded)
        {
            return false;
        }

        return true;
    }
}