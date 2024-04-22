using UnityEngine;

namespace RIP
{
    [System.Serializable]
    public class TAttackOne
    {
        public string triggerName;
        public AnimationClip clip;
        public TCombo.TAttackInput input;
        public float damageOnHit;
    }
    
    [System.Serializable]
    public class TAttackTwo
    {
        public AnimatorOverrideController animatorOC;
        public float damage;
    } 
}