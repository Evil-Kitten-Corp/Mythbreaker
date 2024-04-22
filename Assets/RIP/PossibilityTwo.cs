using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RIP
{
    public class PossibilityTwo : MonoBehaviour
    {
        public Animator anim;
        public List<TAttackOne> attacks;
        private int comboNumber;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ExecuteAttack(attacks[comboNumber]);
            }
        }

        private void ExecuteAttack(TAttackOne i)
        {
            comboNumber++;
            AnimationPlayableUtilities.PlayClip(anim, i.clip, out var graph);
        }
    }
}