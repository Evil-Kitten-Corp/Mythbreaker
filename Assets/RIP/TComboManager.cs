using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RIP
{
    /*public class TComboManager : MonoBehaviour
    {
        public Animator playerAnimator; 
        
        public List<TCombo> comboSequences = new();
        
        [SerializedDictionary("Input", "Animation")] 
        public SerializedDictionary<TCombo.TAttackInput, AnimationClip> holdAnimations = new();

        private readonly List<TCombo.TAttackInput> _currentCombo = new();
        
        private bool isHoldingAttack = false;
        private float holdStartTime;
        private float holdDurationThreshold = 0.2f;

        private TCombo.TAttackInput _tempKey;

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left Mouse Button (LMB) for light attack
            {
                StartHoldTimer();
                _tempKey = TCombo.TAttackInput.LightAttack;
                AddInput(TCombo.TAttackInput.LightAttack);
            }
            else if (Input.GetMouseButtonDown(1)) // Right Mouse Button (RMB) for heavy attack
            {
                StartHoldTimer();
                _tempKey = TCombo.TAttackInput.HeavyAttack;
                AddInput(TCombo.TAttackInput.HeavyAttack);
            }
            else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) // Left or Right Mouse Button released
            {
                EndHoldTimer();
            }
        }

        void AddInput(TCombo.TAttackInput input)
        {
            if (isHoldingAttack && _currentCombo.Count > 0)
            {
                ResetCombo();
                return;
            }
            
            // Add input to current combo
            _currentCombo.Add(input);
            
            // Check if any combo sequence matches the current combo
            foreach (var combo in comboSequences.Where(MatchCombo))
            {
                TriggerAnimation(combo);
                return; // Exit loop if a match is found
            }
            
            ResetToDefaultAnimation(input);
        }
        
        void StartHoldTimer()
        {
            isHoldingAttack = true;
            holdStartTime = Time.time;
        }

        void EndHoldTimer()
        {
            isHoldingAttack = false;
            float holdDuration = Time.time - holdStartTime;

            // Check if hold duration exceeds threshold
            if (holdDuration >= holdDurationThreshold)
            {
                // Perform hold action
                PerformHoldAction(_tempKey);
            }
        }

        void PerformHoldAction(TCombo.TAttackInput input)
        {
            playerAnimator.SetTrigger(holdAnimations.First(x => x.Key == input).Value.name);
        }
        
        void ResetCombo()
        {
            _currentCombo.Clear();
        }

        bool MatchCombo(TCombo combo)
        {
            // Check if currentCombo matches any combo in the ComboSequence
            return combo.chain.Keys.All(input => 
                _currentCombo.Count <= combo.chain.Count && _currentCombo[^1] == input);
        }

        void TriggerAnimation(TCombo combo)
        {
            // Trigger animation for the matched combo
            var animationClip = combo.chain[_currentCombo[^1]];
            playerAnimator.SetTrigger(animationClip.name);
            
            // Clear current combo after triggering animation
            _currentCombo.Clear();
        }

        void ResetToDefaultAnimation(TCombo.TAttackInput input)
        {
            // If no match found, reset to default animation (e.g., heavy attack)
            // Example:
            // playerAnimator.SetTrigger(defaultAnimationTrigger);
            Debug.Log("Reset to default animation for input: " + input);
            
            // Clear current combo after resetting animation
            _currentCombo.Clear();
        }
    }*/

    public class TAltCombo : MonoBehaviour
    {
        public Animator playerAnimator; 
        
        private TCombo.TAttackInput _currentKey;
        private int _inputCount;
        private List<TCombo.TAttackInput> _currentChain;
        
        private float _inputDelayTime;
        private const float ResetTime = 1f;
        private const float InputTime = 0.1f;

        public TCombo.TComboState comboState;

        public List<TCombo> possibleCombos;
        private List<TCombo> _filterCombos;
        private List<AnimationClip> _saveCombos;
        private Coroutine _comboSequence;
        private static readonly int IsHold = Animator.StringToHash("IsHold");
        
        private float _holdStartTime;
        private readonly float _holdDurationThreshold = 0.2f;

        private void HoldAttack()
        {
            if (comboState == TCombo.TComboState.Stop)
            {
                playerAnimator.SetBool(IsHold, true);
                playerAnimator.CrossFadeInFixedTime("Hold Attack", 0.1f);
            }
            else
            {
                playerAnimator.SetBool(IsHold, false);
            }
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left Mouse Button (LMB) for light attack
            {
                StartHoldTimer();
            }
            else if (Input.GetMouseButtonDown(1)) // Right Mouse Button (RMB) for heavy attack
            {
                SetComboInput(TCombo.TAttackInput.HeavyAttack);
            }
            else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) // Left or Right Mouse Button released
            {
                EndHoldTimer();
            }
        }
        
        void StartHoldTimer()
        {
            _holdStartTime = Time.time;
        }

        void EndHoldTimer()
        {
            float holdDuration = Time.time - _holdStartTime;

            // Check if hold duration exceeds threshold
            if (holdDuration >= _holdDurationThreshold)
            {
                HoldAttack();
                return;
            }
            
            SetComboInput(_currentKey); 
        }

        public void SetComboInput(TCombo.TAttackInput key)
        {
            if (_inputDelayTime <= Time.time)
            {
                _inputDelayTime = Time.time + InputTime;
                _currentKey = key;
                _currentChain.Add(key);
                SaveCombo();
            }
        }
        
        private void SaveCombo()
        {
            if (FilterCombo().Count > 0)
            {
                if (FilterCombo()[0].chain.ElementAt(_inputCount).Key == _currentKey)
                {
                    _saveCombos.Add(FilterCombo()[0].chain.ElementAt(_inputCount).Value);
                    Debug.Log(FilterCombo()[0].chain.ElementAt(_inputCount).Value + " matches : " + _currentKey);
                    _inputCount++;
                }
                
                if (_comboSequence != null)
                {
                    StopCoroutine(_comboSequence);
                }
                
                _comboSequence = StartCoroutine(ComboSeq());
            }
        }
        
        private List<TCombo> FilterCombo()
        {
            _filterCombos = possibleCombos;
            _filterCombos.RemoveAll((TCombo data) => 
                data.chain.Count < _currentChain.Count || data.chain.ElementAt(_inputCount).Key 
                != _currentChain[_inputCount]);
            
            if (_filterCombos.Count <= 0)
            {
                Debug.LogError("Combo Data is null");
            }
            
            return _filterCombos;
        }

        public void ResetCombo()
        {
            comboState = TCombo.TComboState.Stop;
            playerAnimator.applyRootMotion = false;
            _currentChain.Clear();
            _saveCombos.Clear();
            _inputCount = 0;
            _filterCombos.Clear();
        }

        private IEnumerator ComboSeq()
        {
            yield return new WaitWhile(() => comboState == TCombo.TComboState.Playing);
            
            while (_saveCombos.Count > 0)
            {
                playerAnimator.CrossFadeInFixedTime(_saveCombos[0].name, 0.1f);
                _saveCombos.RemoveAt(0);
                yield return new WaitForEndOfFrame();
                yield return new WaitWhile(() => comboState == TCombo.TComboState.Playing);
            }
            
            yield return new WaitForSeconds(ResetTime);
            ResetCombo();
        }
        
        private void OnComboBegin()
        {
            comboState = TCombo.TComboState.Playing;
            playerAnimator.applyRootMotion = true;
        }

        private void OnComboEnd(int comboEnd)
        {
            comboState = TCombo.TComboState.Stop;
            playerAnimator.applyRootMotion = false;
            
            if (comboEnd > 0)
            {
                ResetCombo();
            }
        }
    }
}