using System.Collections;
using DG.Tweening;
using ExtEvents;
using UnityEngine;
using UnityEngine.UI;

namespace not_this_again.Abilities
{
    public abstract class Ability: MonoBehaviour
    {
        public AbilityInfo data;
        // ReSharper disable once InconsistentNaming
        public ExtEvent<float> OnAbilityUse;
        [SerializeField] private Image icon = default;
        [SerializeField] private Image coolDownImage = default;

        public void SetIcon(Sprite s)
        {
            icon.sprite = s;
        }
        public void ShowCoolDown(float cooldown)
        {
            transform.DOComplete();
            coolDownImage.fillAmount = 0;
            coolDownImage.DOFillAmount(1, cooldown).SetEase(Ease.Linear).OnComplete(() 
                => transform.DOPunchScale(Vector3.one/10, .2f, 10, 1));
        }
        
        public void TriggerAbility()
        {
            if (data.canUse)
            {
                OnAbilityUse.Invoke(data.cooldownTime);
                MyAbility();
                StartCooldown();
            }
        }
        
        public abstract void MyAbility();

        private void StartCooldown()
        {
            StartCoroutine(Cooldown());
            
            IEnumerator Cooldown()
            {
                data.canUse = false;
                yield return new WaitForSeconds(data.cooldownTime);
                data.canUse = true;
            }
        }
    }
}