using AYellowpaper.SerializedCollections;
using Base;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Combo", menuName = "Combo/Sequence", order = 0)]
    public class ComboSequence : ScriptableObject
    {
        [SerializedDictionary("Type of Attack", "Animation")] 
        public SerializedDictionary<AttackInput, AnimationClip> comboDictionary;
    }
}