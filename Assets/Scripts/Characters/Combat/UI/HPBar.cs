using Characters.Interfaces;
using TNRD;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Combat.UI
{
    public class HpBar : MonoBehaviour
    {
        public SerializableInterface<IHit> Owner;
        public Image bar;
        public Image damagedBar;

        private float _damageShrinkTimer;

        private void Start()
        {
            Owner.Value.OnDamaged.DynamicListeners += DamagedOnDynamicListeners;
        }

        private void DamagedOnDynamicListeners()
        {
            _damageShrinkTimer = 1;
            SetHealth(Owner.Value.GetHealth());
        }

        private void SetHealth(float health)
        {
            bar.fillAmount = health;
        }

        private void Update()
        {
            _damageShrinkTimer -= Time.deltaTime;

            if (_damageShrinkTimer < 0)
            {
                if (bar.fillAmount < damagedBar.fillAmount)
                {
                    float shSpeed = 1f;
                    damagedBar.fillAmount -= shSpeed * Time.deltaTime;
                }
            }
        }
    }
}