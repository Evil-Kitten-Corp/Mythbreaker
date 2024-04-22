using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RIP
{
    [CreateAssetMenu(fileName = "Combo", menuName = "Write New Combo", order = 0)]
    public class TCombo : ScriptableObject
    {
        [SerializedDictionary("Input", "Animation")]
        public SerializedDictionary<TAttackInput, AnimationClip> chain;

        public enum TAttackInput
        {
            LightAttack,
            HeavyAttack
        }
        
        public enum TComboState
        {
            Stop,
            Playing
        }
    }
}