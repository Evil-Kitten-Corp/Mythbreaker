using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ability_Behaviours.UI
{
    public class AbilityUI: MonoBehaviour
    {
        [Header("[ Ability Information ]")]
        public AbilitySlot slot;
        public AbilityData ability;
        
        [Header("[ UI Elements ]")]
        public TMP_Text input;
        public Image abilityImage;
        public Image cooldownMask;
        public TMP_Text cooldownText;
        

        private float _cdTimer;
        private bool _isCd;

        private void Start()
        {
            ability.OnCooldown.DynamicListeners += Cooldown;
            ability.OnCooldownEnd.DynamicListeners += ResetCooldown;
            abilityImage.sprite = ability.image;
            input.text = ability.input.bindings[0].ToString();
        }

        private void Update()
        {
            if (_isCd)
            {
                ApplyCd();
            }
        }

        public void ChangeAbility(AbilityData newAbility)
        {
            UnsubscribeFromAbility();
            ability = newAbility;
            abilityImage.sprite = ability.image;
            input.text = ability.input.bindings[0].ToString();
            ResetCooldown();
        }

        private void ResetCooldown()
        {
            ability.OnCooldownEnd.Invoke();
            _isCd = false;
            cooldownMask.fillAmount = 0;
            cooldownText.gameObject.SetActive(false);
        }

        private void Cooldown()
        {
            _isCd = true;
            _cdTimer = ability.cooldown;
        }

        private void ApplyCd()
        {
            _cdTimer -= Time.deltaTime;

            if (_cdTimer < 0.0f)
            {
                ResetCooldown();
            }
            else
            {
                cooldownText.text = Mathf.RoundToInt(_cdTimer).ToString();
                cooldownMask.fillAmount = _cdTimer / ability.cooldown;
            }
        }
        
        private void UnsubscribeFromAbility()
        {
            ability.OnCooldown.DynamicListeners -= Cooldown;
            ability.OnCooldownEnd.DynamicListeners -= ResetCooldown;
        }

        private void OnDestroy()
        {
            UnsubscribeFromAbility();
        }
    }
}