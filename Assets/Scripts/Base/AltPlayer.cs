using System;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Base
{
    public class AltPlayer : MonoBehaviour
    {
        public Animator playerAnimator;
        
        public GameObject abilityHolder;

        public event Action OnAbilityOne;
        public event Action OnAbilityTwo;
        public event Action OnPerformAttack;

        public Dictionary<AbilitySlot, Action> mapper;
        private readonly List<AttackInput> _currentCombo = new();
        public List<ComboSequence> comboSequences = new List<ComboSequence>();


        public void Start() 
        {
            mapper = new Dictionary<AbilitySlot, Action>()
            {
                { AbilitySlot.A1, OnAbilityOne },
                { AbilitySlot.A2 , OnAbilityTwo}
            };
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left Mouse Button (LMB) for light attack
            {
                OnPerformAttack?.Invoke();
                AddInput(AttackInput.LightAttack);
            }
            else if (Input.GetMouseButtonDown(1)) // Right Mouse Button (RMB) for heavy attack
            {
                OnPerformAttack?.Invoke();
                AddInput(AttackInput.HeavyAttack);
            }
        }
        
        void AddInput(AttackInput input)
        {
            // Add input to current combo
            _currentCombo.Add(input);
        
            // Check if any combo sequence matches the current combo
            foreach (var combo in comboSequences)
            {
                if (MatchCombo(combo))
                {
                    // Trigger animation for the matched combo
                    TriggerAnimation(combo);
                    return; // Exit loop if a match is found
                }
            }
        
            // If no match found, reset to default animation
            ResetToDefaultAnimation(input);
        }
        
        bool MatchCombo(ComboSequence combo)
        {
            // Check if currentCombo matches any combo in the ComboSequence
            // Iterate through the currentCombo and compare with the comboDictionary
            foreach (var input in combo.comboDictionary.Keys)
            {
                if (_currentCombo.Count > combo.comboDictionary.Count || _currentCombo[^1] != input)
                    return false;
            }
        
            // If all inputs match, return true
            return true;
        }

        void TriggerAnimation(ComboSequence combo)
        {
            // Retrieve animation clip from comboDictionary and trigger it
            AnimationClip animationClip = combo.comboDictionary[_currentCombo[^1]];
            playerAnimator.SetTrigger(animationClip.name);
        }

        void ResetToDefaultAnimation(AttackInput input)
        {
            playerAnimator.SetTrigger(input == AttackInput.LightAttack ? "LightAttack" : "HeavyAttack");
            _currentCombo.Clear();
        }
    }

    public enum AttackInput
             {
                 LightAttack,
                 HeavyAttack
             }
}