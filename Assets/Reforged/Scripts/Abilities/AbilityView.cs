using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Abilities
{
    public class AbilityView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Transform _parentTransform;
        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        private AbilityInventory _abilityInventory;

        public Image abilityIconImage;
        public Image cooldownMask;
        public TMP_Text cooldownText;
        public GameObject activeAbilityVFX;
        public TMP_Text stacksCounter;
        
        public AbilityController abilityController;

        public AbilityData Data => abilityController.data;

        private void Start()
        {
            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(false);
            }

            cooldownMask.fillAmount = 0f;
            activeAbilityVFX.SetActive(false);
            
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponentInParent<CanvasGroup>();
            _abilityInventory = GameObject.FindWithTag("AbilityInventory").GetComponent<AbilityInventory>();
            
            abilityController.OnCooldownStart += Cooldown;
            abilityController.OnCooldownEnd += CooldownEnd;
            
            abilityController.OnRecast += () =>
            {
                activeAbilityVFX.SetActive(false);
                abilityIconImage.sprite = Data.icon;
                Debug.Log("Received recast. Disabling VFX now.");
            };
            
            abilityIconImage.sprite = Data.icon;

            if (Data.hasStacks)
            {
                stacksCounter.gameObject.SetActive(true);
            }
            else
            {
                stacksCounter.gameObject.SetActive(false);
            }
        }

        private void CooldownEnd()
        {
            cooldownText.gameObject.SetActive(false);
            cooldownMask.fillAmount = 0;
        }

        private void Cooldown()
        {
            cooldownText.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (abilityController.IsCooldown)
            {
                cooldownText.text = Mathf.RoundToInt(abilityController.CdTimer).ToString();
                cooldownMask.fillAmount = abilityController.CdTimer / abilityController.Cooldown;
            }

            if (abilityController.CanRecast)
            {
                activeAbilityVFX.SetActive(true);
                abilityIconImage.sprite = Data.altIcon;
            }

            if (Data.hasStacks)
            {
                SpikeAbility ab = (SpikeAbility)abilityController;
                stacksCounter.text = ab.GetStacks().ToString();
            }
        }

        public void ReturnToInventory()
        {
            _abilityInventory.AddAbility(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = .6f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //_rect.anchoredPosition = eventData.delta; // _canvas.scaleFactor;
            _rect.position = Input.mousePosition;
        }
    }
}