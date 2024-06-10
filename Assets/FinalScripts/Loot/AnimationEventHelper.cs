using Abilities;
using UnityEngine;

public class AnimationEventHelper: MonoBehaviour
{
    public ThrowCopy throwSkill;
    
    public void WeaponThrow()
    {
        throwSkill.WeaponThrow(); 
    }
}