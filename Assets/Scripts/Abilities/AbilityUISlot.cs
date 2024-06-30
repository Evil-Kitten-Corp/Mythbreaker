using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities
{
    public class AbilityUISlot: MonoBehaviour
    {
        public AbilityController ability;
        private AbilityData _data;
        
        [Header("[ UI Elements ]")]
        public Image abilityImage;
        public Image cooldownMask;
        public TMP_Text cooldownText;
        public TMP_Text stackText;
        public GameObject recastEffect;
        
        private float _cdTimer;
        private bool _isCd;

        private void Start()
        {
            _data = ability.data;
            abilityImage.sprite = _data.icon;

            if (ability.HasStacks)
            {
                stackText.gameObject.SetActive(true);
                stackText.text = "0";
            }

            ability.OnCooldownStart += Cooldown;
            ability.OnCooldownEnd += ResetCooldown;
            
            ResetCooldown();
        }

        private void Update()
        {
            if (_isCd)
            {
                ApplyCd();
            }

            if (ability.HasStacks)
            {
                stackText.text = $"{ability.Stacks()}";
            }

            switch (ability.IsRecastable)
            {
                case true when ability.CanRecast:
                    abilityImage.sprite = _data.altIcon;
                    recastEffect.SetActive(true);
                    break;
                case true when !ability.CanRecast:
                    abilityImage.sprite = _data.icon;
                    recastEffect.SetActive(false);
                    break;
            }
        }

        private void ResetCooldown()
        {
            _isCd = false;
            cooldownMask.fillAmount = 0;
            cooldownText.gameObject.SetActive(false);
        }

        private void Cooldown()
        {
            _isCd = true;
            _cdTimer = _data.cooldown;
            cooldownText.gameObject.SetActive(true);
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
                cooldownMask.fillAmount = _cdTimer / _data.cooldown;
            }
        }
        
        private void UnsubscribeFromAbility()
        {
            ability.OnCooldownStart -= Cooldown;
            ability.OnCooldownEnd -= ResetCooldown;
        }

        private void OnDestroy()
        {
            UnsubscribeFromAbility();
        }
    }
}