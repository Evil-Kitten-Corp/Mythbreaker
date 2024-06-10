using Abilities;
using UnityEngine;

public class AnimationEventHelper: MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] attackChainSounds;
    public ThrowAbility throwSkill;

    public void WeaponThrow()
    {
        throwSkill.WeaponThrow(); 
    }

    public void PlayAttackChainSound(int attack)
    {
        source.PlayOneShot(attackChainSounds[attack]);
    }
}