using UnityEngine;
using UnityEngine.UI;

namespace Survival
{
    public class SurvivalAttributes : MonoBehaviour
    {
        public AttributesManager protag;
        public bool allowHunger;
        public bool allowSleep;
        //public bool allowStamina;

        #region Hunger

        public float currentHunger;
        public float maxHunger;
        public float hungerDecay;
        public Slider hungerSlider;

        #endregion

        #region Sleep
        
        public float currentSleep;
        public float maxSleep;
        public float sleepDecay;
        public Slider sleepSlider;

        #endregion

        /*#region Stamina
        
        public float currentStamina;
        public float maxStamina;
        public float staminaDecay;
        public float staminaRegen;
        public Slider staminaBar;

        #endregion

        private Coroutine _staminaRegenCoroutine;*/

        private void Start()
        {
            if (allowHunger)
            {
                currentHunger = maxHunger;
                hungerSlider.maxValue = maxHunger;
                hungerSlider.value = currentHunger;
            }
            
            if (allowSleep)
            {
                currentSleep = maxSleep;
                sleepSlider.maxValue = maxSleep;
                sleepSlider.value = currentSleep;
            }
            
            /*if (allowStamina)
            {
                currentStamina = maxStamina;
                staminaBar.maxValue = maxStamina;
                staminaBar.value = currentStamina;
            }*/
        }

        private void Update()
        {
            if (allowHunger)
            {
                currentHunger -= hungerDecay * Time.deltaTime;
                hungerSlider.value = currentHunger;

                if (currentHunger < 0)
                {
                    currentHunger = 0;
                }

                if (currentHunger < 20)
                {
                    protag.hungerDebuff = true;
                }
            }
            
            if (allowSleep)
            {
                currentSleep -= sleepDecay * Time.deltaTime;
                sleepSlider.value = currentSleep;
                
                if (currentSleep < 0)
                {
                    currentSleep = 0;
                }
            }
        }

        /*private void UseStamina(float qty)
        {
            if (currentStamina - qty >= 0)
            {
                currentStamina -= qty;
                staminaBar.value = currentStamina;

                if (_staminaRegenCoroutine != null)
                {
                    StopCoroutine(_staminaRegenCoroutine);
                }
                
                _staminaRegenCoroutine = StartCoroutine(RegenStamina());
            }
            else
            {
                Debug.Log("Not enough stamina!");
            }
        }

        IEnumerator RegenStamina()
        {
            yield return new WaitForSeconds(2);

            while (currentStamina < maxStamina)
            {
                currentStamina += maxStamina / 100;
                staminaBar.value = currentStamina;
                yield return new WaitForSeconds(staminaRegen);
            }

            _staminaRegenCoroutine = null;
        }*/
    }
}
